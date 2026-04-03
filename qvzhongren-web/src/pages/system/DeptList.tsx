import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, InputNumber, Select, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { getDeptTree, createDept, updateDept, deleteDept } from '../../api/dept';
import type { DeptTreeDto, DeptCreateDto } from '../../api/dept';

const DeptList: React.FC = () => {
  const [data, setData] = useState<DeptTreeDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<DeptTreeDto | null>(null);
  const [parentCode, setParentCode] = useState('0');
  const [form] = Form.useForm();

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getDeptTree();
      setData(res.data.data ?? []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const handleAdd = (pid = '0') => {
    setEditing(null);
    setParentCode(pid);
    form.resetFields();
    form.setFieldsValue({ parentCode: pid, status: '1' });
    setModalOpen(true);
  };

  const handleEdit = (record: DeptTreeDto) => {
    setEditing(record);
    form.setFieldsValue(record);
    setModalOpen(true);
  };

  const handleDelete = (deptCode: string) => {
    Modal.confirm({
      title: '确认删除该部门？',
      content: '子部门需先删除',
      onOk: async () => {
        await deleteDept(deptCode);
        message.success('删除成功');
        fetchData();
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: DeptCreateDto = { ...values };
    if (editing) {
      dto.deptCode = editing.deptCode;
      await updateDept(dto);
      message.success('更新成功');
    } else {
      dto.parentCode = parentCode;
      await createDept(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData();
  };

  const columns = [
    { title: '部门名称', dataIndex: 'deptName', key: 'deptName' },
    { title: '部门编码', dataIndex: 'deptCode', key: 'deptCode', width: 160 },
    { title: '负责人', dataIndex: 'leader', key: 'leader', width: 120 },
    { title: '联系电话', dataIndex: 'phone', key: 'phone', width: 140 },
    { title: '排序', dataIndex: 'sortNo', key: 'sortNo', width: 70 },
    {
      title: '状态', dataIndex: 'status', key: 'status', width: 80,
      render: (s: string) => <Tag color={s === '1' ? '#00B894' : '#E17055'} style={{ borderRadius: 6 }}>{s === '1' ? '启用' : '停用'}</Tag>,
    },
    {
      title: '操作', key: 'action', width: 240,
      render: (_: unknown, record: DeptTreeDto) => (
        <Space>
          <Button type="link" size="small" icon={<PlusOutlined />} onClick={() => handleAdd(record.deptCode)}>添加子部门</Button>
          <Button type="link" size="small" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" size="small" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record.deptCode)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 12 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => handleAdd('0')}>新增顶级部门</Button>
      </div>
      <Table
        columns={columns}
        dataSource={data}
        rowKey="deptCode"
        loading={loading}
        pagination={false}
        expandable={{ childrenColumnName: 'children' }}
        size="small"
      />

      <Modal title={editing ? '编辑部门' : '新增部门'} open={modalOpen} onOk={handleSubmit} onCancel={() => setModalOpen(false)} destroyOnClose>
        <Form form={form} layout="vertical">
          {!editing && (
            <Form.Item name="deptCode" label="部门编码" rules={[{ required: true, message: '请输入部门编码' }]}>
              <Input placeholder="如 dept-dev" />
            </Form.Item>
          )}
          <Form.Item name="deptName" label="部门名称" rules={[{ required: true, message: '请输入部门名称' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="leader" label="负责人">
            <Input />
          </Form.Item>
          <Form.Item name="phone" label="联系电话">
            <Input />
          </Form.Item>
          <Form.Item name="sortNo" label="排序">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="status" label="状态" rules={[{ required: true }]}>
            <Select options={[{ value: '1', label: '启用' }, { value: '0', label: '停用' }]} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default DeptList;
