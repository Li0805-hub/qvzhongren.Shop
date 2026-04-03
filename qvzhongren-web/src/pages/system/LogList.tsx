import React, { useEffect, useState, useCallback } from 'react';
import { Table, Tag, Input, Select, DatePicker, Space, Card, Button, Tabs, Modal, message } from 'antd';
import { SearchOutlined, ReloadOutlined, SendOutlined, SwapOutlined, DeleteOutlined } from '@ant-design/icons';
import { getLogPage, deleteLogBatch } from '../../api/log';
import type { SysLog, LogQueryDto, LogDeleteDto } from '../../api/log';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

const typeOptions = [
  { value: '', label: '全部类型' },
  { value: 'Information', label: 'Info' },
  { value: 'Warning', label: 'Warning' },
  { value: 'Error', label: 'Error' },
  { value: 'Debug', label: 'Debug' },
];

const typeColorMap: Record<string, string> = {
  Information: '#6C5CE7',
  Warning: '#FDCB6E',
  Error: '#E17055',
  Debug: '#74B9FF',
};

const JsonBlock: React.FC<{ title: string; icon: React.ReactNode; color: string; data?: string }> = ({ title, icon, color, data }) => {
  if (!data) return <div style={{ color: '#bbb', fontSize: 12, padding: '8px 0' }}>（无数据）</div>;

  let formatted = data;
  try {
    formatted = JSON.stringify(JSON.parse(data), null, 2);
  } catch {
    // not JSON, keep as is
  }

  return (
    <div>
      <div style={{ display: 'flex', alignItems: 'center', gap: 6, marginBottom: 6 }}>
        <Tag icon={icon} color={color} style={{ borderRadius: 4, fontSize: 12 }}>{title}</Tag>
        <span style={{ fontSize: 11, color: '#aaa' }}>{data.length} 字符</span>
      </div>
      <pre style={{
        margin: 0, padding: 12,
        background: '#1A1145',
        color: '#E8E8E8',
        borderRadius: 8,
        fontSize: 12,
        lineHeight: 1.6,
        maxHeight: 280,
        overflow: 'auto',
        whiteSpace: 'pre-wrap',
        wordBreak: 'break-all',
      }}>
        {formatted}
      </pre>
    </div>
  );
};

const LogList: React.FC = () => {
  const [data, setData] = useState<SysLog[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);
  const [query, setQuery] = useState<LogQueryDto>({
    pageIndex: 1,
    pageSize: 20,
    type: '',
    keyword: '',
  });

  const fetchData = useCallback(async (q: LogQueryDto) => {
    setLoading(true);
    try {
      const res = await getLogPage(q);
      const page = res.data.data;
      setData(page?.values ?? []);
      setTotal(page?.totalCount ?? 0);
    } catch {
      // handled
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchData(query); }, [query, fetchData]);

  const handleReset = () => {
    setDateRange(null);
    setQuery({ pageIndex: 1, pageSize: 20, type: '', keyword: '' });
  };

  const handleDeleteByQuery = () => {
    const dto: LogDeleteDto = {};
    if (query.type) dto.type = query.type;
    if (query.keyword) dto.keyword = query.keyword;
    if (query.startTime) dto.startTime = query.startTime;
    if (query.endTime) dto.endTime = query.endTime;

    if (!dto.type && !dto.keyword && !dto.startTime && !dto.endTime) {
      message.warning('请先设置查询条件（类型、关键字或时间范围），再进行删除');
      return;
    }

    const conditions: string[] = [];
    if (dto.type) conditions.push(`类型 = ${dto.type}`);
    if (dto.keyword) conditions.push(`关键字含「${dto.keyword}」`);
    if (dto.startTime) conditions.push(`开始时间 ≥ ${dayjs(dto.startTime).format('YYYY-MM-DD HH:mm')}`);
    if (dto.endTime) conditions.push(`结束时间 ≤ ${dayjs(dto.endTime).format('YYYY-MM-DD HH:mm')}`);

    Modal.confirm({
      title: '确认批量删除日志？',
      content: (
        <div>
          <p>将删除满足以下条件的所有日志：</p>
          <ul style={{ margin: 0, paddingLeft: 20 }}>
            {conditions.map((c, i) => <li key={i}>{c}</li>)}
          </ul>
          <p style={{ color: '#E17055', marginTop: 8 }}>此操作不可撤销！</p>
        </div>
      ),
      okText: '确认删除',
      okButtonProps: { danger: true },
      onOk: async () => {
        const res = await deleteLogBatch(dto);
        message.success(res.data.message || `已删除 ${res.data.data} 条日志`);
        fetchData(query);
      },
    });
  };

  const columns = [
    {
      title: '类型', dataIndex: 'type', key: 'type', width: 90,
      render: (type: string) => (
        <Tag color={typeColorMap[type] || 'default'} style={{ borderRadius: 6, fontWeight: 500 }}>
          {type}
        </Tag>
      ),
    },
    {
      title: '接口', dataIndex: 'name', key: 'name', width: 280, ellipsis: true,
      render: (name: string) => <span style={{ fontSize: 13, fontFamily: 'monospace' }}>{name}</span>,
    },
    {
      title: '结果', dataIndex: 'content', key: 'content', ellipsis: true,
      render: (content: string) => <span style={{ fontSize: 13 }}>{content}</span>,
    },
    {
      title: '入参', dataIndex: 'requestBody', key: 'requestBody', width: 70, align: 'center' as const,
      render: (v: string | undefined) => v ? <Tag color="#6C5CE7" style={{ borderRadius: 4 }}>有</Tag> : <span style={{ color: '#ddd' }}>-</span>,
    },
    {
      title: '出参', dataIndex: 'responseBody', key: 'responseBody', width: 70, align: 'center' as const,
      render: (v: string | undefined) => v ? <Tag color="#00B894" style={{ borderRadius: 4 }}>有</Tag> : <span style={{ color: '#ddd' }}>-</span>,
    },
    {
      title: '时间', dataIndex: 'timestamp', key: 'timestamp', width: 170,
      render: (ts: string) => <span style={{ fontSize: 13, color: '#8C8C8C' }}>{dayjs(ts).format('YYYY-MM-DD HH:mm:ss')}</span>,
    },
  ];

  return (
    <div>
      <Card style={{ borderRadius: 16, border: 'none', marginBottom: 12 }} styles={{ body: { padding: '12px 20px' } }}>
        <Space wrap size={10}>
          <Select
            value={query.type}
            onChange={(val) => setQuery((p) => ({ ...p, type: val, pageIndex: 1 }))}
            options={typeOptions}
            style={{ width: 130 }}
          />
          <Input
            placeholder="搜索接口或内容"
            value={query.keyword}
            onChange={(e) => setQuery((p) => ({ ...p, keyword: e.target.value }))}
            onPressEnter={() => setQuery((p) => ({ ...p, pageIndex: 1 }))}
            style={{ width: 220 }}
            prefix={<SearchOutlined style={{ color: '#bbb' }} />}
            allowClear
          />
          <RangePicker
            showTime
            size="middle"
            value={dateRange}
            onChange={(dates) => {
              const range = dates as [dayjs.Dayjs, dayjs.Dayjs] | null;
              setDateRange(range);
              setQuery((p) => ({
                ...p,
                startTime: range?.[0]?.format('YYYY-MM-DD HH:mm:ss'),
                endTime: range?.[1]?.format('YYYY-MM-DD HH:mm:ss'),
                pageIndex: 1,
              }));
            }}
          />
          <Button type="primary" icon={<SearchOutlined />} onClick={() => setQuery((p) => ({ ...p, pageIndex: 1 }))}>查询</Button>
          <Button icon={<ReloadOutlined />} onClick={handleReset}>重置</Button>
          <Button danger icon={<DeleteOutlined />} onClick={handleDeleteByQuery}>按条件删除</Button>
        </Space>
      </Card>

      <Card style={{ borderRadius: 16, border: 'none' }} styles={{ body: { padding: 12 } }}>
        <Table
          columns={columns}
          dataSource={data}
          rowKey="id"
          loading={loading}
          size="small"
          pagination={{
            current: query.pageIndex,
            pageSize: query.pageSize,
            total,
            showSizeChanger: true,
            showQuickJumper: true,
            pageSizeOptions: ['10', '20', '50', '100'],
            showTotal: (t) => `共 ${t} 条`,
            size: 'small',
            onChange: (page, size) => setQuery((p) => ({ ...p, pageIndex: page, pageSize: size })),
          }}
          expandable={{
            expandedRowRender: (record) => (
              <div style={{ padding: '8px 4px' }}>
                <Tabs
                  size="small"
                  items={[
                    {
                      key: 'req',
                      label: <span><SendOutlined style={{ marginRight: 4 }} />入参 (Request)</span>,
                      children: <JsonBlock title="Request Body" icon={<SendOutlined />} color="#6C5CE7" data={record.requestBody} />,
                    },
                    {
                      key: 'res',
                      label: <span><SwapOutlined style={{ marginRight: 4 }} />出参 (Response)</span>,
                      children: <JsonBlock title="Response Body" icon={<SwapOutlined />} color="#00B894" data={record.responseBody} />,
                    },
                  ]}
                />
              </div>
            ),
            rowExpandable: (record) => !!(record.requestBody || record.responseBody),
          }}
        />
      </Card>
    </div>
  );
};

export default LogList;
