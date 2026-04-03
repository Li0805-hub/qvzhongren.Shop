import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, Select, message, Transfer, TreeSelect } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, TeamOutlined, KeyOutlined } from '@ant-design/icons';
import { getUserList, createUser, updateUser, deleteUser, assignRoles, getUserRoles, resetPassword } from '../../api/user';
import { getRoleList } from '../../api/role';
import { getDeptTree } from '../../api/dept';
import type { DeptTreeDto } from '../../api/dept';
import type { SysUserResponseDto, SysUserCreateDto, SysRoleResponseDto } from '../../types';

const UserList: React.FC = () => {
  const [data, setData] = useState<SysUserResponseDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [roleModalOpen, setRoleModalOpen] = useState(false);
  const [editing, setEditing] = useState<SysUserResponseDto | null>(null);
  const [form] = Form.useForm();
  const [roles, setRoles] = useState<SysRoleResponseDto[]>([]);
  const [selectedRoleKeys, setSelectedRoleKeys] = useState<string[]>([]);
  const [currentUserId, setCurrentUserId] = useState('');
  const [deptTree, setDeptTree] = useState<DeptTreeDto[]>([]);
  const [deptMap, setDeptMap] = useState<Record<string, string>>({});

  const flattenDepts = (list: DeptTreeDto[], map: Record<string, string>) => {
    for (const d of list) {
      map[d.deptCode] = d.deptName;
      if (d.children) flattenDepts(d.children, map);
    }
  };

  const convertDeptToTreeData = (list: DeptTreeDto[]): any[] =>
    list.map((d) => ({
      value: d.deptCode,
      title: d.deptName,
      children: d.children ? convertDeptToTreeData(d.children) : undefined,
    }));

  useEffect(() => {
    getDeptTree().then((res) => {
      const tree = res.data.data ?? [];
      setDeptTree(tree);
      const m: Record<string, string> = {};
      flattenDepts(tree, m);
      setDeptMap(m);
    });
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getUserList();
      setData(res.data.data ?? []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const handleAdd = () => {
    setEditing(null);
    form.resetFields();
    form.setFieldsValue({ status: '1' });
    setModalOpen(true);
  };

  const handleEdit = (record: SysUserResponseDto) => {
    setEditing(record);
    form.setFieldsValue({ ...record, password: '' });
    setModalOpen(true);
  };

  const handleDelete = (userId: string) => {
    Modal.confirm({
      title: '确认删除该用户？',
      onOk: async () => {
        await deleteUser(userId);
        message.success('删除成功');
        fetchData();
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: SysUserCreateDto = { ...values };
    if (editing) {
      dto.userId = editing.userId;
      await updateUser(dto);
      message.success('更新成功');
    } else {
      dto.userId = crypto.randomUUID();
      await createUser(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData();
  };

  const handleAssignRoles = async (record: SysUserResponseDto) => {
    setCurrentUserId(record.userId);
    const [rolesRes, userRolesRes] = await Promise.all([
      getRoleList(),
      getUserRoles(record.userId),
    ]);
    setRoles(rolesRes.data.data ?? []);
    setSelectedRoleKeys(userRolesRes.data.data ?? []);
    setRoleModalOpen(true);
  };

  const handleSaveRoles = async () => {
    await assignRoles({ userId: currentUserId, roleIds: selectedRoleKeys });
    message.success('角色分配成功');
    setRoleModalOpen(false);
  };

  const columns = [
    { title: '用户名', dataIndex: 'userName', key: 'userName' },
    { title: '真实姓名', dataIndex: 'realName', key: 'realName' },
    { title: '手机号', dataIndex: 'phone', key: 'phone' },
    { title: '邮箱', dataIndex: 'email', key: 'email' },
    {
      title: '部门', dataIndex: 'deptCode', key: 'deptCode',
      render: (code: string) => deptMap[code] || code || '-',
    },
    {
      title: '状态', dataIndex: 'status', key: 'status',
      render: (s: string) => <Tag color={s === '1' ? 'green' : 'red'}>{s === '1' ? '启用' : '禁用'}</Tag>,
    },
    {
      title: '操作', key: 'action',
      render: (_: unknown, record: SysUserResponseDto) => (
        <Space>
          <Button type="link" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" icon={<TeamOutlined />} onClick={() => handleAssignRoles(record)}>分配角色</Button>
          <Button type="link" icon={<KeyOutlined />} onClick={() => {
            Modal.confirm({
              title: '确认重置密码？',
              content: `将 ${record.realName || record.userName} 的密码重置为 123456`,
              onOk: async () => {
                await resetPassword(record.userId);
                message.success('密码已重置为 123456');
              },
            });
          }}>重置密码</Button>
          <Button type="link" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record.userId)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>新增用户</Button>
      </div>
      <Table columns={columns} dataSource={data} rowKey="userId" loading={loading} />

      <Modal title={editing ? '编辑用户' : '新增用户'} open={modalOpen} onOk={handleSubmit} onCancel={() => setModalOpen(false)} destroyOnClose>
        <Form form={form} layout="vertical">
          <Form.Item name="userName" label="用户名" rules={[{ required: true, message: '请输入用户名' }]}>
            <Input />
          </Form.Item>
          {!editing && (
            <Form.Item name="password" label="密码" rules={[{ required: true, message: '请输入密码' }]}>
              <Input.Password />
            </Form.Item>
          )}
          <Form.Item name="realName" label="真实姓名">
            <Input />
          </Form.Item>
          <Form.Item name="phone" label="手机号">
            <Input />
          </Form.Item>
          <Form.Item name="email" label="邮箱">
            <Input />
          </Form.Item>
          <Form.Item name="deptCode" label="部门">
            <TreeSelect
              treeData={convertDeptToTreeData(deptTree)}
              placeholder="请选择部门"
              allowClear
              treeDefaultExpandAll
            />
          </Form.Item>
          <Form.Item name="status" label="状态" rules={[{ required: true }]}>
            <Select options={[{ value: '1', label: '启用' }, { value: '0', label: '禁用' }]} />
          </Form.Item>
          <Form.Item name="remark" label="备注">
            <Input.TextArea rows={2} />
          </Form.Item>
        </Form>
      </Modal>

      <Modal title="分配角色" open={roleModalOpen} onOk={handleSaveRoles} onCancel={() => setRoleModalOpen(false)}>
        <Transfer
          dataSource={roles.map((r) => ({ key: r.roleId, title: r.roleName, description: r.description }))}
          targetKeys={selectedRoleKeys}
          onChange={(keys) => setSelectedRoleKeys(keys as string[])}
          render={(item) => item.title!}
          titles={['可选角色', '已分配角色']}
          listStyle={{ width: 220, height: 300 }}
        />
      </Modal>
    </div>
  );
};

export default UserList;
