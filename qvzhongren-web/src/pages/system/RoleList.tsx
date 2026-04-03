import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, InputNumber, Select, message, Tree } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, AppstoreOutlined } from '@ant-design/icons';
import { getRoleList, createRole, updateRole, deleteRole, assignMenus, getRoleMenus } from '../../api/role';
import { getMenuTree } from '../../api/menu';
import type { SysRoleResponseDto, SysRoleCreateDto, SysMenuResponseDto } from '../../types';
import type { DataNode } from 'antd/es/tree';

const RoleList: React.FC = () => {
  const [data, setData] = useState<SysRoleResponseDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [menuModalOpen, setMenuModalOpen] = useState(false);
  const [editing, setEditing] = useState<SysRoleResponseDto | null>(null);
  const [form] = Form.useForm();
  const [menuTree, setMenuTree] = useState<DataNode[]>([]);
  const [checkedMenuKeys, setCheckedMenuKeys] = useState<React.Key[]>([]);
  const [currentRoleId, setCurrentRoleId] = useState('');

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getRoleList();
      setData(res.data.data ?? []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const convertToTreeData = (menus: SysMenuResponseDto[]): DataNode[] =>
    menus.map((m) => ({
      key: m.menuId,
      title: m.menuName,
      children: m.children ? convertToTreeData(m.children) : undefined,
    }));

  const handleAdd = () => {
    setEditing(null);
    form.resetFields();
    form.setFieldsValue({ status: '1' });
    setModalOpen(true);
  };

  const handleEdit = (record: SysRoleResponseDto) => {
    setEditing(record);
    form.setFieldsValue(record);
    setModalOpen(true);
  };

  const handleDelete = (roleId: string) => {
    Modal.confirm({
      title: '确认删除该角色？',
      onOk: async () => {
        await deleteRole(roleId);
        message.success('删除成功');
        fetchData();
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: SysRoleCreateDto = { ...values };
    if (editing) {
      dto.roleId = editing.roleId;
      await updateRole(dto);
      message.success('更新成功');
    } else {
      dto.roleId = crypto.randomUUID();
      await createRole(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData();
  };

  const handleAssignMenus = async (record: SysRoleResponseDto) => {
    setCurrentRoleId(record.roleId);
    const [treeRes, roleMenusRes] = await Promise.all([
      getMenuTree(),
      getRoleMenus(record.roleId),
    ]);
    setMenuTree(convertToTreeData(treeRes.data.data ?? []));
    setCheckedMenuKeys(roleMenusRes.data.data ?? []);
    setMenuModalOpen(true);
  };

  const handleSaveMenus = async () => {
    await assignMenus({ roleId: currentRoleId, menuIds: checkedMenuKeys as string[] });
    message.success('菜单分配成功');
    setMenuModalOpen(false);
  };

  const columns = [
    { title: '角色编码', dataIndex: 'roleCode', key: 'roleCode' },
    { title: '角色名称', dataIndex: 'roleName', key: 'roleName' },
    { title: '描述', dataIndex: 'description', key: 'description' },
    { title: '排序', dataIndex: 'sortNo', key: 'sortNo' },
    {
      title: '状态', dataIndex: 'status', key: 'status',
      render: (s: string) => <Tag color={s === '1' ? 'green' : 'red'}>{s === '1' ? '启用' : '禁用'}</Tag>,
    },
    {
      title: '操作', key: 'action',
      render: (_: unknown, record: SysRoleResponseDto) => (
        <Space>
          <Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" icon={<AppstoreOutlined />} onClick={() => handleAssignMenus(record)}>分配菜单</Button>
          <Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record.roleId)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>新增角色</Button>
      </div>
      <Table columns={columns} dataSource={data} rowKey="roleId" loading={loading} />

      <Modal title={editing ? '编辑角色' : '新增角色'} open={modalOpen} onOk={handleSubmit} onCancel={() => setModalOpen(false)} destroyOnClose>
        <Form form={form} layout="vertical">
          <Form.Item name="roleCode" label="角色编码" rules={[{ required: true, message: '请输入角色编码' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="roleName" label="角色名称" rules={[{ required: true, message: '请输入角色名称' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="描述">
            <Input.TextArea rows={2} />
          </Form.Item>
          <Form.Item name="sortNo" label="排序">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="status" label="状态" rules={[{ required: true }]}>
            <Select options={[{ value: '1', label: '启用' }, { value: '0', label: '禁用' }]} />
          </Form.Item>
        </Form>
      </Modal>

      <Modal title="分配菜单" open={menuModalOpen} onOk={handleSaveMenus} onCancel={() => setMenuModalOpen(false)} width={500}>
        <Tree
          checkable
          treeData={menuTree}
          checkedKeys={checkedMenuKeys}
          onCheck={(keys) => setCheckedMenuKeys(keys as React.Key[])}
          defaultExpandAll
          style={{ maxHeight: 400, overflow: 'auto' }}
        />
      </Modal>
    </div>
  );
};

export default RoleList;
