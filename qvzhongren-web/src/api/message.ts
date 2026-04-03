import request from './request';
import type { ResultDto, ListPageResultDto } from '../types';

export interface ConversationDto {
  userId: string;
  userName: string;
  lastMessage: string;
  lastTime: string;
  unreadCount: number;
}

export interface MessageItemDto {
  id: string;
  senderId: string;
  receiverId: string;
  content: string;
  msgType: string;
  fileName?: string;
  fileUrl?: string;
  isRead: string;
  sendTime: string;
  isMine: boolean;
}

export interface SimpleUserDto {
  userId: string;
  userName: string;
  realName?: string;
}

export interface SendMessageParams {
  senderId: string;
  receiverId: string;
  content: string;
  msgType?: string;
  fileName?: string;
  fileUrl?: string;
}

export function sendMessage(params: SendMessageParams) {
  return request.post<ResultDto<boolean>>('/message/send', params);
}

export function uploadFile(file: File) {
  const formData = new FormData();
  formData.append('file', file);
  return request.post<{ isSuccess: boolean; data: { fileName: string; fileUrl: string } }>('/file/upload', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
}

export function getConversations(userId: string) {
  return request.post<ResultDto<ConversationDto[]>>('/message/getconversations', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function getMessages(userId: string, otherUserId: string, pageIndex = 1, pageSize = 50) {
  return request.post<ResultDto<ListPageResultDto<MessageItemDto>>>('/message/getmessages', {
    userId, otherUserId, pageIndex, pageSize,
  });
}

export function markRead(userId: string, otherUserId: string) {
  return request.post<ResultDto<boolean>>('/message/markread', { userId, otherUserId });
}

export function getUnreadCount(userId: string) {
  return request.post<ResultDto<number>>('/message/getunreadcount', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function getMessageUserList(userId: string) {
  return request.post<ResultDto<SimpleUserDto[]>>('/message/getuserlist', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}
