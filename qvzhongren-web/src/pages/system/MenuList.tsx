import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, InputNumber, Select, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { getMenuTree, createMenu, updateMenu, deleteMenu } from '../../api/menu';
import type { SysMenuResponseDto, SysMenuCreateDto } from '../../types';

const menuTypeOptions = [
  { value: 'M', label: '目录' },
  { value: 'C', label: '菜单' },
  { value: 'F', label: '按钮' },
];

const MenuList: React.FC = () => {
  const [data, setData] = useState<SysMenuResponseDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<SysMenuResponseDto | null>(null);
  const [parentId, setParentId] = useState('0');
  const [form] = Form.useForm();

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getMenuTree();
      setData(res.data.data ?? []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const handleAdd = (pid = '0') => {
    setEditing(null);
    setParentId(pid);
    form.resetFields();
    form.setFieldsValue({ parentId: pid, menuType: 'C', status: '1' });
    setModalOpen(true);
  };

  const handleEdit = (record: SysMenuResponseDto) => {
    setEditing(record);
    form.setFieldsValue(record);
    setModalOpen(true);
  };

  const handleDelete = (menuId: string) => {
    Modal.confirm({
      title: '确认删除该菜单？',
      content: '子菜单也会被删除',
      onOk: async () => {
        await deleteMenu(menuId);
        message.success('删除成功');
        fetchData();
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: SysMenuCreateDto = { ...values };
    if (editing) {
      dto.menuId = editing.menuId;
      await updateMenu(dto);
      message.success('更新成功');
    } else {
      dto.menuId = crypto.randomUUID();
      dto.parentId = parentId;
      await createMenu(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData();
  };

  const columns = [
    { title: '菜单名称', dataIndex: 'menuName', key: 'menuName' },
    { title: '路径', dataIndex: 'path', key: 'path' },
    { title: '组件', dataIndex: 'component', key: 'component' },
    { title: '图标', dataIndex: 'icon', key: 'icon' },
    {
      title: '类型', dataIndex: 'menuType', key: 'menuType',
      render: (t: string) => {
        const map: Record<string, { color: string; label: string }> = {
          M: { color: 'blue', label: '目录' },
          C: { color: 'green', label: '菜单' },
          F: { color: 'orange', label: '按钮' },
        };
        const v = map[t] || { color: 'default', label: t };
        return <Tag color={v.color}>{v.label}</Tag>;
      },
    },
    { title: '排序', dataIndex: 'sortNo', key: 'sortNo', width: 80 },
    {
      title: '状态', dataIndex: 'status', key: 'status', width: 80,
      render: (s: string) => <Tag color={s === '1' ? 'green' : 'red'}>{s === '1' ? '显示' : '隐藏'}</Tag>,
    },
    {
      title: '操作', key: 'action', width: 260,
      render: (_: unknown, record: SysMenuResponseDto) => (
        <Space>
          <Button type="link" icon={<PlusOutlined />} onClick={() => handleAdd(record.menuId)}>添加子菜单</Button>
          <Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record.menuId)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => handleAdd('0')}>新增顶级菜单</Button>
      </div>
      <Table
        columns={columns}
        dataSource={data}
        rowKey="menuId"
        loading={loading}
        pagination={false}
        expandable={{ childrenColumnName: 'children' }}
      />

      <Modal title={editing ? '编辑菜单' : '新增菜单'} open={modalOpen} onOk={handleSubmit} onCancel={() => setModalOpen(false)} destroyOnClose>
        <Form form={form} layout="vertical">
          <Form.Item name="menuName" label="菜单名称" rules={[{ required: true, message: '请输入菜单名称' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="menuType" label="菜单类型" rules={[{ required: true }]}>
            <Select options={menuTypeOptions} />
          </Form.Item>
          <Form.Item name="path" label="路由路径">
            <Input placeholder="如 /system/users" />
          </Form.Item>
          <Form.Item name="component" label="组件路径">
            <Input placeholder="如 system/UserList" />
          </Form.Item>
          <Form.Item name="icon" label="图标">
            <Input placeholder="图标名称" />
          </Form.Item>
          <Form.Item name="perms" label="权限标识">
            <Input placeholder="如 system:user:list" />
          </Form.Item>
          <Form.Item name="sortNo" label="排序">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="status" label="状态" rules={[{ required: true }]}>
            <Select options={[{ value: '1', label: '显示' }, { value: '0', label: '隐藏' }]} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default MenuList;
