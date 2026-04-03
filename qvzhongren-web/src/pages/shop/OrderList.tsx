import React, { useEffect, useState, useCallback } from 'react';
import { Table, Button, Space, Tag, Modal, Tabs, Image, Descriptions, message } from 'antd';
import { EyeOutlined, CloseCircleOutlined, SendOutlined } from '@ant-design/icons';
import { getOrderPage, getOrderById, cancelOrder, updateOrderStatus } from '../../api/shop';
import type { OrderDto, OrderQueryDto, OrderItemDto } from '../../api/shop';
import dayjs from 'dayjs';

const statusMap: Record<string, { text: string; color: string }> = {
  '0': { text: '待付款', color: 'orange' },
  '1': { text: '待发货', color: 'blue' },
  '2': { text: '待收货', color: 'cyan' },
  '3': { text: '已完成', color: 'green' },
  '-1': { text: '已取消', color: 'red' },
};

const statusTabs = [
  { key: '', label: '全部' },
  { key: '0', label: '待付款' },
  { key: '1', label: '待发货' },
  { key: '2', label: '待收货' },
  { key: '3', label: '已完成' },
  { key: '-1', label: '已取消' },
];

const OrderList: React.FC = () => {
  const [data, setData] = useState<OrderDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [detailModalOpen, setDetailModalOpen] = useState(false);
  const [detailOrder, setDetailOrder] = useState<OrderDto | null>(null);
  const [query, setQuery] = useState<OrderQueryDto>({
    pageIndex: 1,
    pageSize: 10,
    status: undefined,
  });

  const fetchData = useCallback(async (q: OrderQueryDto) => {
    setLoading(true);
    try {
      const res = await getOrderPage(q);
      const page = res.data.data;
      setData(page?.values ?? []);
      setTotal(page?.totalCount ?? 0);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchData(query); }, [query, fetchData]);

  const handleTabChange = (key: string) => {
    setQuery((p) => ({ ...p, status: key || undefined, pageIndex: 1 }));
  };

  const handleViewDetail = async (orderId: string) => {
    const res = await getOrderById(orderId);
    setDetailOrder(res.data.data ?? null);
    setDetailModalOpen(true);
  };

  const handleCancel = (record: OrderDto) => {
    Modal.confirm({
      title: '确认取消该订单？',
      content: `订单号：${record.orderNo}`,
      onOk: async () => {
        await cancelOrder(record.orderId);
        message.success('订单已取消');
        fetchData(query);
      },
    });
  };

  const handleShip = (record: OrderDto) => {
    Modal.confirm({
      title: '确认发货？',
      content: `订单号：${record.orderNo}`,
      onOk: async () => {
        await updateOrderStatus({ orderId: record.orderId, status: '2' });
        message.success('已发货');
        fetchData(query);
      },
    });
  };

  const itemColumns = [
    {
      title: '商品图片', dataIndex: 'productImage', key: 'productImage', width: 60,
      render: (url: string) => (
        <Image
          src={url}
          width={40}
          height={40}
          style={{ objectFit: 'cover', borderRadius: 4 }}
          fallback="data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNDAiIGhlaWdodD0iNDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHJlY3Qgd2lkdGg9IjQwIiBoZWlnaHQ9IjQwIiBmaWxsPSIjZjBmMGYwIi8+PC9zdmc+"
        />
      ),
    },
    { title: '商品名称', dataIndex: 'productName', key: 'productName' },
    {
      title: '单价', dataIndex: 'price', key: 'price', width: 100,
      render: (v: number) => <span>&yen;{v.toFixed(2)}</span>,
    },
    { title: '数量', dataIndex: 'quantity', key: 'quantity', width: 80 },
    {
      title: '小计', dataIndex: 'subtotal', key: 'subtotal', width: 100,
      render: (v: number) => <span style={{ color: '#E17055', fontWeight: 500 }}>&yen;{v.toFixed(2)}</span>,
    },
  ];

  const columns = [
    {
      title: '订单号', dataIndex: 'orderNo', key: 'orderNo', width: 200,
      render: (no: string, record: OrderDto) => (
        <Button type="link" style={{ fontFamily: 'monospace', padding: 0 }} onClick={() => handleViewDetail(record.orderId)}>
          {no}
        </Button>
      ),
    },
    { title: '收货人', dataIndex: 'receiverName', key: 'receiverName', width: 100 },
    { title: '联系电话', dataIndex: 'receiverPhone', key: 'receiverPhone', width: 130 },
    { title: '收货地址', dataIndex: 'receiverAddress', key: 'receiverAddress', ellipsis: true },
    {
      title: '总金额', dataIndex: 'totalAmount', key: 'totalAmount', width: 110,
      render: (v: number) => <span style={{ color: '#E17055', fontWeight: 700 }}>&yen;{v.toFixed(2)}</span>,
    },
    {
      title: '状态', dataIndex: 'status', key: 'status', width: 90,
      render: (s: string) => {
        const info = statusMap[s] || { text: s, color: 'default' };
        return <Tag color={info.color}>{info.text}</Tag>;
      },
    },
    {
      title: '下单时间', dataIndex: 'createDate', key: 'createDate', width: 170,
      render: (ts: string) => ts ? <span style={{ fontSize: 13, color: '#8C8C8C' }}>{dayjs(ts).format('YYYY-MM-DD HH:mm:ss')}</span> : '-',
    },
    {
      title: '操作', key: 'action', width: 160,
      render: (_: unknown, record: OrderDto) => (
        <Space>
          <Button type="link" size="small" icon={<EyeOutlined />} onClick={() => handleViewDetail(record.orderId)}>详情</Button>
          {record.status === '0' && (
            <Button type="link" size="small" danger icon={<CloseCircleOutlined />} onClick={() => handleCancel(record)}>取消订单</Button>
          )}
          {record.status === '1' && (
            <Button type="link" size="small" icon={<SendOutlined />} onClick={() => handleShip(record)}>发货</Button>
          )}
        </Space>
      ),
    },
  ];

  return (
    <div>
      <Tabs
        activeKey={query.status ?? ''}
        onChange={handleTabChange}
        items={statusTabs}
        style={{ marginBottom: 12 }}
      />

      <Table
        columns={columns}
        dataSource={data}
        rowKey="orderId"
        loading={loading}
        size="small"
        pagination={{
          current: query.pageIndex,
          pageSize: query.pageSize,
          total,
          showSizeChanger: true,
          showQuickJumper: true,
          showTotal: (t) => `共 ${t} 条`,
          size: 'small',
          onChange: (page, size) => setQuery((p) => ({ ...p, pageIndex: page, pageSize: size })),
        }}
        expandable={{
          expandedRowRender: (record) => (
            <Table
              columns={itemColumns}
              dataSource={record.items ?? []}
              rowKey="itemId"
              pagination={false}
              size="small"
            />
          ),
          rowExpandable: (record) => !!(record.items && record.items.length > 0),
        }}
      />

      <Modal
        title="订单详情"
        open={detailModalOpen}
        onCancel={() => setDetailModalOpen(false)}
        footer={null}
        width={700}
      >
        {detailOrder && (
          <>
            <Descriptions bordered size="small" column={2} style={{ marginBottom: 16 }}>
              <Descriptions.Item label="订单号"><span style={{ fontFamily: 'monospace' }}>{detailOrder.orderNo}</span></Descriptions.Item>
              <Descriptions.Item label="状态">
                <Tag color={statusMap[detailOrder.status]?.color}>{statusMap[detailOrder.status]?.text}</Tag>
              </Descriptions.Item>
              <Descriptions.Item label="收货人">{detailOrder.receiverName}</Descriptions.Item>
              <Descriptions.Item label="联系电话">{detailOrder.receiverPhone}</Descriptions.Item>
              <Descriptions.Item label="收货地址" span={2}>{detailOrder.receiverAddress}</Descriptions.Item>
              <Descriptions.Item label="总金额">
                <span style={{ color: '#E17055', fontWeight: 700 }}>&yen;{detailOrder.totalAmount.toFixed(2)}</span>
              </Descriptions.Item>
              <Descriptions.Item label="下单时间">
                {detailOrder.createDate ? dayjs(detailOrder.createDate).format('YYYY-MM-DD HH:mm:ss') : '-'}
              </Descriptions.Item>
              {detailOrder.payTime && (
                <Descriptions.Item label="支付时间">{dayjs(detailOrder.payTime).format('YYYY-MM-DD HH:mm:ss')}</Descriptions.Item>
              )}
              {detailOrder.shipTime && (
                <Descriptions.Item label="发货时间">{dayjs(detailOrder.shipTime).format('YYYY-MM-DD HH:mm:ss')}</Descriptions.Item>
              )}
              {detailOrder.completeTime && (
                <Descriptions.Item label="完成时间">{dayjs(detailOrder.completeTime).format('YYYY-MM-DD HH:mm:ss')}</Descriptions.Item>
              )}
              {detailOrder.remark && (
                <Descriptions.Item label="备注" span={2}>{detailOrder.remark}</Descriptions.Item>
              )}
            </Descriptions>
            <Table
              columns={itemColumns}
              dataSource={detailOrder.items ?? []}
              rowKey="itemId"
              pagination={false}
              size="small"
              title={() => <strong>订单商品</strong>}
            />
          </>
        )}
      </Modal>
    </div>
  );
};

export default OrderList;
