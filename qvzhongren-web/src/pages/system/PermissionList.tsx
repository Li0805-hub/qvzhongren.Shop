import React, { useEffect, useState, useCallback } from 'react';
import { Tabs, Card, List, Avatar, Tree, Button, Empty, Spin, message, Tag } from 'antd';
import { TeamOutlined, UserOutlined, SaveOutlined } from '@ant-design/icons';
import { getRoleList } from '../../api/role';
import { assignMenus, getRoleMenus } from '../../api/role';
import { getUserList, assignUserMenus, getUserMenus } from '../../api/user';
import { getMenuTree } from '../../api/menu';
import type { SysRoleResponseDto, SysUserResponseDto, SysMenuResponseDto } from '../../types';
import type { DataNode } from 'antd/es/tree';

const convertToTreeData = (menus: SysMenuResponseDto[]): DataNode[] =>
  menus.map((m) => ({
    key: m.menuId,
    title: m.menuName,
    children: m.children ? convertToTreeData(m.children) : undefined,
  }));

const PermissionList: React.FC = () => {
  const [menuTree, setMenuTree] = useState<DataNode[]>([]);
  const [loadingTree, setLoadingTree] = useState(false);

  // Role tab state
  const [roles, setRoles] = useState<SysRoleResponseDto[]>([]);
  const [selectedRoleId, setSelectedRoleId] = useState<string | null>(null);
  const [roleCheckedKeys, setRoleCheckedKeys] = useState<React.Key[]>([]);
  const [roleLoading, setRoleLoading] = useState(false);
  const [roleSaving, setRoleSaving] = useState(false);

  // User tab state
  const [users, setUsers] = useState<SysUserResponseDto[]>([]);
  const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
  const [userCheckedKeys, setUserCheckedKeys] = useState<React.Key[]>([]);
  const [userLoading, setUserLoading] = useState(false);
  const [userSaving, setUserSaving] = useState(false);

  useEffect(() => {
    const init = async () => {
      setLoadingTree(true);
      try {
        const [treeRes, rolesRes, usersRes] = await Promise.all([
          getMenuTree(),
          getRoleList(),
          getUserList(),
        ]);
        setMenuTree(convertToTreeData(treeRes.data.data ?? []));
        setRoles(rolesRes.data.data ?? []);
        setUsers(usersRes.data.data ?? []);
      } finally {
        setLoadingTree(false);
      }
    };
    init();
  }, []);

  // Load role's menus when selected
  const handleSelectRole = useCallback(async (roleId: string) => {
    setSelectedRoleId(roleId);
    setRoleLoading(true);
    try {
      const res = await getRoleMenus(roleId);
      setRoleCheckedKeys(res.data.data ?? []);
    } finally {
      setRoleLoading(false);
    }
  }, []);

  const handleSaveRoleMenus = async () => {
    if (!selectedRoleId) return;
    setRoleSaving(true);
    try {
      await assignMenus({ roleId: selectedRoleId, menuIds: roleCheckedKeys as string[] });
      message.success('角色菜单权限保存成功');
    } finally {
      setRoleSaving(false);
    }
  };

  // Load user's menus when selected
  const handleSelectUser = useCallback(async (userId: string) => {
    setSelectedUserId(userId);
    setUserLoading(true);
    try {
      const res = await getUserMenus(userId);
      setUserCheckedKeys(res.data.data ?? []);
    } finally {
      setUserLoading(false);
    }
  }, []);

  const handleSaveUserMenus = async () => {
    if (!selectedUserId) return;
    setUserSaving(true);
    try {
      await assignUserMenus({ userId: selectedUserId, menuIds: userCheckedKeys as string[] });
      message.success('用户菜单权限保存成功');
    } finally {
      setUserSaving(false);
    }
  };

  const listItemStyle = (selected: boolean): React.CSSProperties => ({
    padding: '10px 16px',
    cursor: 'pointer',
    borderRadius: 10,
    marginBottom: 4,
    transition: 'all 0.2s',
    background: selected ? 'linear-gradient(135deg, #F4F2FF, #EDE8FF)' : 'transparent',
    border: selected ? '1px solid #D6CEFF' : '1px solid transparent',
  });

  const renderPermPanel = (
    selected: string | null,
    checkedKeys: React.Key[],
    setCheckedKeys: (keys: React.Key[]) => void,
    loading: boolean,
    saving: boolean,
    onSave: () => void,
    label: string,
  ) => (
    <Card
      title={selected ? `${label}菜单权限` : `请选择${label}`}
      style={{ borderRadius: 16, border: 'none', height: '100%' }}
      styles={{ header: { borderBottom: 'none' } }}
      extra={
        selected && (
          <Button
            type="primary"
            icon={<SaveOutlined />}
            onClick={onSave}
            loading={saving}
            style={{ borderRadius: 8 }}
          >
            保存权限
          </Button>
        )
      }
    >
      {!selected ? (
        <Empty description={`请从左侧选择一个${label}`} />
      ) : loading || loadingTree ? (
        <div style={{ textAlign: 'center', padding: 60 }}><Spin /></div>
      ) : (
        <Tree
          checkable
          treeData={menuTree}
          checkedKeys={checkedKeys}
          onCheck={(keys) => setCheckedKeys(keys as React.Key[])}
          defaultExpandAll
          style={{ maxHeight: 500, overflow: 'auto' }}
        />
      )}
    </Card>
  );

  const tabItems = [
    {
      key: 'role',
      label: <span><TeamOutlined style={{ marginRight: 6 }} />角色-菜单权限</span>,
      children: (
        <div style={{ display: 'flex', gap: 20 }}>
          <Card
            title="角色列表"
            style={{ width: 300, flexShrink: 0, borderRadius: 16, border: 'none' }}
            styles={{ header: { borderBottom: 'none' }, body: { padding: '8px 12px' } }}
          >
            <List
              dataSource={roles}
              renderItem={(role) => (
                <div
                  style={listItemStyle(selectedRoleId === role.roleId)}
                  onClick={() => handleSelectRole(role.roleId)}
                >
                  <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                    <Avatar
                      size={36}
                      icon={<TeamOutlined />}
                      style={{
                        background: selectedRoleId === role.roleId
                          ? 'linear-gradient(135deg, #A29BFE, #6C5CE7)'
                          : '#E8E8E8',
                      }}
                    />
                    <div>
                      <div style={{ fontWeight: 500 }}>{role.roleName}</div>
                      <div style={{ fontSize: 12, color: '#8C8C8C' }}>{role.roleCode}</div>
                    </div>
                    <Tag
                      color={role.status === '1' ? '#00B894' : '#E17055'}
                      style={{ marginLeft: 'auto', borderRadius: 6 }}
                    >
                      {role.status === '1' ? '启用' : '禁用'}
                    </Tag>
                  </div>
                </div>
              )}
            />
          </Card>
          <div style={{ flex: 1 }}>
            {renderPermPanel(selectedRoleId, roleCheckedKeys, setRoleCheckedKeys, roleLoading, roleSaving, handleSaveRoleMenus, '角色')}
          </div>
        </div>
      ),
    },
    {
      key: 'user',
      label: <span><UserOutlined style={{ marginRight: 6 }} />用户-菜单权限</span>,
      children: (
        <div style={{ display: 'flex', gap: 20 }}>
          <Card
            title="用户列表"
            style={{ width: 300, flexShrink: 0, borderRadius: 16, border: 'none' }}
            styles={{ header: { borderBottom: 'none' }, body: { padding: '8px 12px' } }}
          >
            <List
              dataSource={users}
              renderItem={(user) => (
                <div
                  style={listItemStyle(selectedUserId === user.userId)}
                  onClick={() => handleSelectUser(user.userId)}
                >
                  <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                    <Avatar
                      size={36}
                      icon={<UserOutlined />}
                      style={{
                        background: selectedUserId === user.userId
                          ? 'linear-gradient(135deg, #A29BFE, #6C5CE7)'
                          : '#E8E8E8',
                      }}
                    />
                    <div>
                      <div style={{ fontWeight: 500 }}>{user.realName || user.userName}</div>
                      <div style={{ fontSize: 12, color: '#8C8C8C' }}>{user.userName}</div>
                    </div>
                    <Tag
                      color={user.status === '1' ? '#00B894' : '#E17055'}
                      style={{ marginLeft: 'auto', borderRadius: 6 }}
                    >
                      {user.status === '1' ? '启用' : '禁用'}
                    </Tag>
                  </div>
                </div>
              )}
            />
          </Card>
          <div style={{ flex: 1 }}>
            {renderPermPanel(selectedUserId, userCheckedKeys, setUserCheckedKeys, userLoading, userSaving, handleSaveUserMenus, '用户')}
          </div>
        </div>
      ),
    },
  ];

  return (
    <Tabs
      items={tabItems}
      defaultActiveKey="role"
      style={{ minHeight: 500 }}
    />
  );
};

export default PermissionList;
