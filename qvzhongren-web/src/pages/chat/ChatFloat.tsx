import React, { useState, useEffect, useRef, useCallback } from 'react';
import { Input, Button, Avatar, Badge, Empty, Spin, Tag, Modal, List, Upload, message as antMsg, Image } from 'antd';
import {
  SendOutlined, UserOutlined, PlusOutlined, RobotOutlined,
  MessageOutlined, CloseOutlined, MinusOutlined,
  CompassOutlined, DatabaseOutlined, PaperClipOutlined, PictureOutlined,
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useAppSelector } from '../../store';
import {
  getConversations, getMessages, sendMessage, markRead,
  getMessageUserList, getUnreadCount, uploadFile,
} from '../../api/message';
import { agentChat } from '../../api/agent';
import type { ConversationDto, MessageItemDto, SimpleUserDto } from '../../api/message';
import type { AgentMessage, AgentToolCall } from '../../api/agent';
import * as signalR from '@microsoft/signalr';
import dayjs from 'dayjs';

const AGENT_ID = '__agent__';
const IMG_EXTS = ['.png', '.jpg', '.jpeg', '.gif', '.webp', '.bmp', '.svg'];

interface ChatMsg {
  id: string; content: string; isMine: boolean; time: string;
  msgType?: string; fileName?: string; fileUrl?: string;
  toolCalls?: AgentToolCall[]; navigateTo?: string; loading?: boolean;
}

const toolNameMap: Record<string, string> = {
  navigate_to_page: '导航', query_users: '用户', query_roles: '角色',
  query_menus: '菜单', query_logs: '日志', query_statistics: '统计',
};

const ChatFloat: React.FC = () => {
  const currentUserId = useAppSelector((s) => s.auth.userId) ?? '';
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);
  const [conversations, setConversations] = useState<ConversationDto[]>([]);
  const [selectedId, setSelectedId] = useState<string>(AGENT_ID);
  const [selectedName, setSelectedName] = useState('智能助手');
  const [msgHistory, setMsgHistory] = useState<MessageItemDto[]>([]);
  const [msgLoading, setMsgLoading] = useState(false);
  const [agentHistory, setAgentHistory] = useState<ChatMsg[]>([]);
  const [agentLoading, setAgentLoading] = useState(false);
  const [input, setInput] = useState('');
  const [sending, setSending] = useState(false);
  const [unread, setUnread] = useState(0);
  const [userPickerOpen, setUserPickerOpen] = useState(false);
  const [allUsers, setAllUsers] = useState<SimpleUserDto[]>([]);
  const [onlineUsers, setOnlineUsers] = useState<Set<string>>(new Set());
  const [typingUser, setTypingUser] = useState<string | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const selectedIdRef = useRef(selectedId);
  const typingTimerRef = useRef<ReturnType<typeof setTimeout>>();
  selectedIdRef.current = selectedId;

  const scrollToBottom = () => setTimeout(() => messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' }), 80);

  const fetchUnread = useCallback(async () => {
    if (!currentUserId) return;
    try { const r = await getUnreadCount(currentUserId); if (r.data.isSuccess) setUnread(r.data.data ?? 0); } catch { /* */ }
  }, [currentUserId]);

  const fetchConversations = useCallback(async () => {
    if (!currentUserId) return;
    try { const r = await getConversations(currentUserId); setConversations(r.data.data ?? []); } catch { /* */ }
  }, [currentUserId]);

  const fetchMessages = useCallback(async (otherUserId: string) => {
    if (!currentUserId) return;
    setMsgLoading(true);
    try {
      const r = await getMessages(currentUserId, otherUserId);
      setMsgHistory(r.data.data?.values ?? []);
      scrollToBottom();
      await markRead(currentUserId, otherUserId);
      fetchConversations(); fetchUnread();
    } finally { setMsgLoading(false); }
  }, [currentUserId, fetchConversations, fetchUnread]);

  // SignalR
  useEffect(() => {
    if (!currentUserId) return;
    fetchUnread(); fetchConversations();

    const conn = new signalR.HubConnectionBuilder().withUrl('/hubs/chat').withAutomaticReconnect().build();

    conn.on('ReceiveMessage', (msg: { senderId: string; receiverId: string }) => {
      fetchConversations(); fetchUnread();
      const sel = selectedIdRef.current;
      if (sel && sel !== AGENT_ID && (msg.senderId === sel || msg.receiverId === sel)) {
        getMessages(currentUserId, sel).then((r) => { setMsgHistory(r.data.data?.values ?? []); scrollToBottom(); markRead(currentUserId, sel); });
      }
    });
    conn.on('UserOnline', (uid: string) => setOnlineUsers((s) => new Set(s).add(uid)));
    conn.on('UserOffline', (uid: string) => setOnlineUsers((s) => { const n = new Set(s); n.delete(uid); return n; }));
    conn.on('UserTyping', () => { setTypingUser(selectedIdRef.current); clearTimeout(typingTimerRef.current); typingTimerRef.current = setTimeout(() => setTypingUser(null), 3000); });
    conn.on('UserStopTyping', () => setTypingUser(null));

    conn.start().then(async () => {
      await conn.invoke('Register', currentUserId);
      const list: string[] = await conn.invoke('GetOnlineUsers');
      setOnlineUsers(new Set(list));
    }).catch((e) => console.log('SignalR error:', e));

    connectionRef.current = conn;
    return () => { conn.stop(); };
  }, [currentUserId, fetchUnread, fetchConversations]);

  const handleSelect = (id: string, name: string) => {
    setSelectedId(id); setSelectedName(name); setInput(''); setTypingUser(null);
    if (id !== AGENT_ID) fetchMessages(id); else scrollToBottom();
  };

  // Typing notification
  const handleInputChange = (val: string) => {
    setInput(val);
    if (selectedId !== AGENT_ID && connectionRef.current?.state === signalR.HubConnectionState.Connected) {
      connectionRef.current.invoke('Typing', selectedId);
      clearTimeout(typingTimerRef.current);
      typingTimerRef.current = setTimeout(() => {
        connectionRef.current?.invoke('StopTyping', selectedId);
      }, 2000);
    }
  };

  // File upload handler
  const handleFileUpload = async (file: File) => {
    if (!selectedId || selectedId === AGENT_ID) return;
    setSending(true);
    try {
      const res = await uploadFile(file);
      if (res.data.isSuccess) {
        const { fileName, fileUrl } = res.data.data;
        const isImg = IMG_EXTS.some((ext) => fileName.toLowerCase().endsWith(ext));
        await sendMessage({ senderId: currentUserId, receiverId: selectedId, content: isImg ? '[图片]' : `[文件] ${fileName}`, msgType: isImg ? 'image' : 'file', fileName, fileUrl });
      }
    } finally { setSending(false); }
  };

  const handleSend = async () => {
    const text = input.trim();
    if (!text || sending) return;
    setInput('');
    if (selectedId === AGENT_ID) {
      const userMsg: ChatMsg = { id: Date.now().toString(), content: text, isMine: true, time: new Date().toISOString() };
      setAgentHistory((p) => [...p, userMsg, { id: 'loading', content: '', isMine: false, time: '', loading: true }]);
      setAgentLoading(true); scrollToBottom();
      try {
        const agentMsgs: AgentMessage[] = agentHistory.filter((c) => !c.loading).map((c): AgentMessage => ({ role: c.isMine ? 'user' : 'assistant', content: c.content }));
        agentMsgs.push({ role: 'user', content: text });
        const msgs = agentMsgs;
        const { data: res } = await agentChat(msgs);
        if (res.isSuccess && res.data) {
          setAgentHistory((p) => [...p.slice(0, -1), { id: Date.now().toString(), content: res.data.content, isMine: false, time: new Date().toISOString(), toolCalls: res.data.toolCalls, navigateTo: res.data.navigateTo }]);
          if (res.data.navigateTo) { antMsg.info(`正在导航到 ${res.data.navigateTo}`); setTimeout(() => navigate(res.data.navigateTo!), 1200); }
        } else { setAgentHistory((p) => [...p.slice(0, -1), { id: Date.now().toString(), content: res.message || '失败', isMine: false, time: new Date().toISOString() }]); }
      } catch { setAgentHistory((p) => [...p.slice(0, -1), { id: Date.now().toString(), content: '网络错误', isMine: false, time: new Date().toISOString() }]); } finally { setAgentLoading(false); scrollToBottom(); }
    } else {
      setSending(true);
      try { await sendMessage({ senderId: currentUserId, receiverId: selectedId, content: text }); } finally { setSending(false); }
      connectionRef.current?.invoke('StopTyping', selectedId);
    }
  };

  const handlePickUser = (user: SimpleUserDto) => {
    setUserPickerOpen(false); handleSelect(user.userId, user.realName || user.userName);
    if (!conversations.find((c) => c.userId === user.userId)) {
      setConversations((p) => [{ userId: user.userId, userName: user.realName || user.userName, lastMessage: '', lastTime: new Date().toISOString(), unreadCount: 0 }, ...p]);
    }
  };

  const isAgent = selectedId === AGENT_ID;
  const accentGrad = isAgent ? 'linear-gradient(135deg, #A29BFE, #6C5CE7)' : 'linear-gradient(135deg, #55EFC4, #00B894)';

  const chatMsgs: ChatMsg[] = isAgent ? agentHistory
    : msgHistory.map((m) => ({ id: m.id, content: m.content, isMine: m.isMine, time: m.sendTime, msgType: m.msgType, fileName: m.fileName, fileUrl: m.fileUrl }));

  // Status text for selected user
  const statusText = isAgent ? 'AI 在线' : typingUser === selectedId ? '正在输入...' : onlineUsers.has(selectedId) ? '在线' : '离线';
  const statusColor = isAgent ? '#00B894' : typingUser === selectedId ? '#FDCB6E' : onlineUsers.has(selectedId) ? '#00B894' : '#bbb';

  const renderMsgContent = (msg: ChatMsg) => {
    if (msg.loading) return <Spin size="small" />;
    if (msg.msgType === 'image' && msg.fileUrl) {
      return <Image src={`/api/../${msg.fileUrl}`} alt={msg.fileName} style={{ maxWidth: 200, maxHeight: 200, borderRadius: 8 }} preview={{ mask: '查看' }} />;
    }
    if (msg.msgType === 'file' && msg.fileUrl) {
      return (
        <a href={msg.fileUrl} target="_blank" rel="noreferrer" style={{ color: msg.isMine ? '#fff' : '#6C5CE7', textDecoration: 'none', display: 'flex', alignItems: 'center', gap: 6 }}>
          <PaperClipOutlined /> {msg.fileName || '下载文件'}
        </a>
      );
    }
    return <>{msg.toolCalls && msg.toolCalls.length > 0 && (
      <div style={{ marginBottom: 4, display: 'flex', gap: 3, flexWrap: 'wrap' }}>
        {msg.toolCalls.map((tc, j) => <Tag key={j} icon={tc.name === 'navigate_to_page' ? <CompassOutlined /> : <DatabaseOutlined />} color={tc.name === 'navigate_to_page' ? '#6C5CE7' : '#00B894'} style={{ borderRadius: 4, fontSize: 10, lineHeight: '16px', padding: '0 5px' }}>{toolNameMap[tc.name] || tc.name}</Tag>)}
      </div>
    )}{msg.navigateTo && <Tag icon={<CompassOutlined />} color="#6C5CE7" style={{ borderRadius: 4, fontSize: 10, cursor: 'pointer', marginBottom: 4 }} onClick={() => navigate(msg.navigateTo!)}>{msg.navigateTo}</Tag>}<span style={{ whiteSpace: 'pre-wrap' }}>{msg.content}</span></>;
  };

  return (
    <>
      {!open && (
        <div onClick={() => { setOpen(true); fetchConversations(); }} style={{ position: 'fixed', right: 28, bottom: 28, zIndex: 1000, width: 52, height: 52, borderRadius: 16, background: 'linear-gradient(135deg, #A29BFE, #6C5CE7)', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer', boxShadow: '0 6px 20px rgba(108,92,231,0.4)', transition: 'transform 0.2s' }}
          onMouseEnter={(e) => (e.currentTarget.style.transform = 'scale(1.1)')} onMouseLeave={(e) => (e.currentTarget.style.transform = 'scale(1)')}>
          <Badge count={unread} size="small" offset={[-2, 2]}><MessageOutlined style={{ fontSize: 24, color: '#fff' }} /></Badge>
        </div>
      )}

      {open && (
        <div style={{ position: 'fixed', right: 28, bottom: 28, zIndex: 1000, width: 620, height: 540, background: '#fff', borderRadius: 20, boxShadow: '0 12px 40px rgba(108,92,231,0.16)', display: 'flex', overflow: 'hidden' }}>
          {/* Left */}
          <div style={{ width: 200, flexShrink: 0, borderRight: '1px solid #F0EEF9', display: 'flex', flexDirection: 'column', background: '#FAFAFF' }}>
            <div style={{ height: 48, padding: '0 12px', background: 'linear-gradient(135deg, #A29BFE, #6C5CE7)', display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexShrink: 0 }}>
              <span style={{ color: '#fff', fontWeight: 600, fontSize: 13 }}>会话</span>
              <div style={{ display: 'flex', gap: 2 }}>
                <Button type="text" size="small" icon={<PlusOutlined />} style={{ color: 'rgba(255,255,255,0.9)' }} onClick={async () => { const r = await getMessageUserList(currentUserId); setAllUsers(r.data.data ?? []); setUserPickerOpen(true); }} />
                <Button type="text" size="small" icon={<MinusOutlined />} style={{ color: 'rgba(255,255,255,0.8)' }} onClick={() => setOpen(false)} />
                <Button type="text" size="small" icon={<CloseOutlined />} style={{ color: 'rgba(255,255,255,0.8)' }} onClick={() => { setOpen(false); setSelectedId(AGENT_ID); setSelectedName('智能助手'); }} />
              </div>
            </div>
            <div style={{ flex: 1, overflow: 'auto' }}>
              {/* Agent row - always first */}
              <div onClick={() => handleSelect(AGENT_ID, '智能助手')} style={{ padding: '10px 12px', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: 8, background: selectedId === AGENT_ID ? '#EDE8FF' : 'transparent', borderLeft: selectedId === AGENT_ID ? '3px solid #6C5CE7' : '3px solid transparent' }}>
                <Badge dot status="success" offset={[-4, 28]}><Avatar size={34} icon={<RobotOutlined />} style={{ background: selectedId === AGENT_ID ? 'linear-gradient(135deg, #A29BFE, #6C5CE7)' : '#D6CEFF' }} /></Badge>
                <div><div style={{ fontWeight: 500, fontSize: 13 }}>智能助手</div><div style={{ fontSize: 11, color: '#00B894' }}>AI 在线</div></div>
              </div>
              {/* Users */}
              {conversations.map((conv) => {
                const online = onlineUsers.has(conv.userId);
                return (
                  <div key={conv.userId} onClick={() => handleSelect(conv.userId, conv.userName)} style={{ padding: '10px 12px', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: 8, background: selectedId === conv.userId ? '#E8F8F5' : 'transparent', borderLeft: selectedId === conv.userId ? '3px solid #00B894' : '3px solid transparent', transition: 'all 0.15s' }}>
                    <Badge count={conv.unreadCount} size="small" offset={[-2, 2]}>
                      <Badge dot status={online ? 'success' : 'default'} offset={[-4, 28]}>
                        <Avatar size={34} icon={<UserOutlined />} style={{ background: selectedId === conv.userId ? 'linear-gradient(135deg, #55EFC4, #00B894)' : '#D6CEFF' }} />
                      </Badge>
                    </Badge>
                    <div style={{ flex: 1, overflow: 'hidden' }}>
                      <div style={{ fontWeight: 500, fontSize: 13 }}>{conv.userName}</div>
                      <div style={{ fontSize: 11, color: online ? '#00B894' : '#bbb', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                        {typingUser === conv.userId ? '正在输入...' : online ? '在线' : conv.lastMessage || '离线'}
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          </div>

          {/* Right */}
          <div style={{ flex: 1, display: 'flex', flexDirection: 'column', overflow: 'hidden' }}>
            <div style={{ height: 48, padding: '0 16px', background: accentGrad, display: 'flex', alignItems: 'center', gap: 8, color: '#fff', flexShrink: 0 }}>
              {isAgent ? <RobotOutlined /> : <UserOutlined />}
              <div>
                <div style={{ fontWeight: 600, fontSize: 14 }}>{selectedName}</div>
                <div style={{ fontSize: 10, opacity: 0.85 }}>
                  <span style={{ display: 'inline-block', width: 6, height: 6, borderRadius: 3, background: statusColor, marginRight: 4 }} />
                  {statusText}
                </div>
              </div>
            </div>

            <div style={{ flex: 1, overflow: 'auto', padding: 12, display: 'flex', flexDirection: 'column', gap: 10, background: '#FAFAFF' }}>
              {(isAgent ? agentLoading && chatMsgs.length === 0 : msgLoading) ? (
                <div style={{ textAlign: 'center', padding: 30 }}><Spin size="small" /></div>
              ) : chatMsgs.length === 0 ? (
                <div style={{ textAlign: 'center', color: '#bbb', padding: 30, fontSize: 12 }}>
                  {isAgent ? (<><RobotOutlined style={{ fontSize: 32, color: '#D6CEFF', marginBottom: 8, display: 'block' }} />查询数据 / 导航页面 / 系统问答<div style={{ display: 'flex', gap: 6, justifyContent: 'center', flexWrap: 'wrap', marginTop: 12 }}>{['系统概览', '查询用户', '打开日志'].map((q) => <Button key={q} size="small" style={{ borderRadius: 14, borderColor: '#D6CEFF', color: '#6C5CE7', fontSize: 11 }} onClick={() => setInput(q)}>{q}</Button>)}</div></>) : '发送第一条消息吧'}
                </div>
              ) : chatMsgs.map((msg) => (
                <div key={msg.id} style={{ display: 'flex', justifyContent: msg.isMine ? 'flex-end' : 'flex-start', gap: 6 }}>
                  {!msg.isMine && <Avatar size={28} icon={isAgent ? <RobotOutlined /> : <UserOutlined />} style={{ background: isAgent ? 'linear-gradient(135deg, #A29BFE, #6C5CE7)' : '#D6CEFF', flexShrink: 0 }} />}
                  <div style={{ maxWidth: '78%' }}>
                    <div style={{ padding: msg.msgType === 'image' ? 4 : '8px 12px', borderRadius: msg.isMine ? '12px 12px 4px 12px' : '12px 12px 12px 4px', background: msg.msgType === 'image' ? 'transparent' : msg.isMine ? accentGrad : '#fff', color: msg.isMine ? '#fff' : '#2D1B69', fontSize: 13, lineHeight: 1.5, boxShadow: msg.msgType === 'image' ? 'none' : '0 1px 3px rgba(0,0,0,0.04)', wordBreak: 'break-word' }}>
                      {renderMsgContent(msg)}
                    </div>
                    {!msg.loading && msg.time && <div style={{ fontSize: 10, color: '#bbb', marginTop: 3, textAlign: msg.isMine ? 'right' : 'left' }}>{dayjs(msg.time).format('HH:mm')}</div>}
                  </div>
                  {msg.isMine && <Avatar size={28} icon={<UserOutlined />} style={{ background: accentGrad, flexShrink: 0 }} />}
                </div>
              ))}
              <div ref={messagesEndRef} />
            </div>

            {/* Input bar */}
            <div style={{ padding: '8px 10px', borderTop: '1px solid #F0EEF9', display: 'flex', gap: 6, flexShrink: 0, background: '#fff', alignItems: 'center' }}>
              {!isAgent && (
                <>
                  <Upload showUploadList={false} beforeUpload={(file) => { handleFileUpload(file); return false; }} accept="image/*">
                    <Button type="text" icon={<PictureOutlined />} size="small" style={{ color: '#8C8C8C' }} title="发送图片" />
                  </Upload>
                  <Upload showUploadList={false} beforeUpload={(file) => { handleFileUpload(file); return false; }}>
                    <Button type="text" icon={<PaperClipOutlined />} size="small" style={{ color: '#8C8C8C' }} title="发送文件" />
                  </Upload>
                </>
              )}
              <Input
                value={input} onChange={(e) => handleInputChange(e.target.value)} onPressEnter={handleSend}
                placeholder={isAgent ? '问我任何问题...' : '输入消息...'}
                style={{ borderRadius: 10, border: '1.5px solid #EDE8FF', fontSize: 13 }}
                disabled={sending || agentLoading}
              />
              <Button type="primary" icon={<SendOutlined />} onClick={handleSend} loading={sending || agentLoading}
                style={{ borderRadius: 10, width: 38, flexShrink: 0, background: accentGrad, border: 'none' }} />
            </div>
          </div>
        </div>
      )}

      <Modal title="选择联系人" open={userPickerOpen} onCancel={() => setUserPickerOpen(false)} footer={null} width={320}>
        <List size="small" dataSource={allUsers} renderItem={(user) => (
          <List.Item style={{ cursor: 'pointer', padding: '8px 10px', borderRadius: 8 }} onClick={() => handlePickUser(user)}>
            <List.Item.Meta avatar={<Badge dot status={onlineUsers.has(user.userId) ? 'success' : 'default'} offset={[-4, 28]}><Avatar size={32} icon={<UserOutlined />} style={{ background: '#D6CEFF' }} /></Badge>}
              title={<span style={{ fontSize: 13 }}>{user.realName || user.userName}</span>}
              description={<span style={{ fontSize: 11, color: onlineUsers.has(user.userId) ? '#00B894' : '#bbb' }}>{onlineUsers.has(user.userId) ? '在线' : '离线'}</span>} />
          </List.Item>
        )} />
      </Modal>
    </>
  );
};

export default ChatFloat;
