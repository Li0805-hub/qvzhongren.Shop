import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, InputNumber, Select, message, Tooltip } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, CloudServerOutlined } from '@ant-design/icons';
import {
  getServiceConfigList,
  createServiceConfig,
  updateServiceConfig,
  deleteServiceConfig,
} from '../../api/serviceConfig';
import type { ServiceConfigDto, ServiceConfigCreateDto } from '../../api/serviceConfig';

const ServiceConfigList: React.FC = () => {
  const [data, setData] = useState<ServiceConfigDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ServiceConfigDto | null>(null);
  const [form] = Form.useForm();

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getServiceConfigList();
      setData(res.data.data ?? []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const handleAdd = () => {
    setEditing(null);
    form.resetFields();
    form.setFieldsValue({ status: '1', sortNo: 0 });
    setModalOpen(true);
  };

  const handleEdit = (record: ServiceConfigDto) => {
    setEditing(record);
    form.setFieldsValue(record);
    setModalOpen(true);
  };

  const handleDelete = (record: ServiceConfigDto) => {
    Modal.confirm({
      title: `确认删除服务「${record.serviceName}」？`,
      content: '删除后其他依赖此服务地址的模块可能无法正常通信，请谨慎操作。',
      onOk: async () => {
        await deleteServiceConfig(record.configId);
        message.success('删除成功');
        fetchData();
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: ServiceConfigCreateDto = { ...values };
    if (editing) {
      dto.configId = editing.configId;
      await updateServiceConfig(dto);
      message.success('更新成功');
    } else {
      dto.configId = crypto.randomUUID().replace(/-/g, '');
      await createServiceConfig(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData();
  };

  const columns = [
    {
      title: '服务名称',
      dataIndex: 'serviceName',
      key: 'serviceName',
      render: (text: string) => (
        <Space>
          <CloudServerOutlined style={{ color: '#6C5CE7' }} />
          <strong>{text}</strong>
        </Space>
      ),
    },
    {
      title: '服务地址',
      dataIndex: 'serviceUrl',
      key: 'serviceUrl',
      render: (text: string) => (
        <Tooltip title={text}>
          <code style={{ background: '#f5f5f5', padding: '2px 8px', borderRadius: 4 }}>{text}</code>
        </Tooltip>
      ),
    },
    {
      title: '描述',
      dataIndex: 'description',
      key: 'description',
      ellipsis: true,
    },
    {
      title: '状态',
      dataIndex: 'status',
      key: 'status',
      width: 80,
      render: (s: string) =>
        s === '1' ? <Tag color="green">启用</Tag> : <Tag color="red">禁用</Tag>,
    },
    {
      title: '排序',
      dataIndex: 'sortNo',
      key: 'sortNo',
      width: 70,
    },
    {
      title: '操作',
      key: 'action',
      width: 150,
      render: (_: unknown, record: ServiceConfigDto) => (
        <Space>
          <Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h2 style={{ margin: 0 }}>服务配置管理</h2>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>新增服务</Button>
      </div>

      <Table
        rowKey="configId"
        columns={columns}
        dataSource={data}
        loading={loading}
        pagination={false}
      />

      <Modal
        title={editing ? '编辑服务配置' : '新增服务配置'}
        open={modalOpen}
        onOk={handleSubmit}
        onCancel={() => setModalOpen(false)}
        destroyOnClose
        width={520}
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item
            name="serviceName"
            label="服务名称"
            rules={[{ required: true, message: '请输入服务名称' }]}
          >
            <Input placeholder="如 PermissionService" />
          </Form.Item>
          <Form.Item
            name="serviceUrl"
            label="服务地址"
            rules={[
              { required: true, message: '请输入服务地址' },
              { pattern: /^https?:\/\//, message: '请输入有效的 HTTP 地址' },
            ]}
          >
            <Input placeholder="如 http://localhost:5001" />
          </Form.Item>
          <Form.Item name="description" label="描述">
            <Input.TextArea rows={2} placeholder="服务描述" />
          </Form.Item>
          <Space style={{ width: '100%' }} size="large">
            <Form.Item name="status" label="状态" rules={[{ required: true }]}>
              <Select style={{ width: 120 }}>
                <Select.Option value="1">启用</Select.Option>
                <Select.Option value="0">禁用</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item name="sortNo" label="排序">
              <InputNumber min={0} />
            </Form.Item>
          </Space>
        </Form>
      </Modal>
    </div>
  );
};

export default ServiceConfigList;
