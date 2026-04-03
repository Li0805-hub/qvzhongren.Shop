import React, { useEffect } from 'react';
import { Layout, Menu, Button, Dropdown, Avatar } from 'antd';
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  UserOutlined,
  TeamOutlined,
  AppstoreOutlined,
  SafetyOutlined,
  DashboardOutlined,
  LogoutOutlined,
  FileTextOutlined,
  ApartmentOutlined,
  CloudServerOutlined,
  ShoppingOutlined,
  ShopOutlined,
  OrderedListOutlined,
  TagsOutlined,
  SettingOutlined,
  LockOutlined,
  RobotOutlined,
} from '@ant-design/icons';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../store';
import { toggleCollapsed } from '../store/menuSlice';
import { logout } from '../store/authSlice';
import ChatFloat from '../pages/chat/ChatFloat';
import type { MenuProps } from 'antd';

const { Header, Sider, Content } = Layout;

const menuItems: MenuProps['items'] = [
  { key: '/dashboard', icon: <DashboardOutlined />, label: '仪表盘' },
  {
    key: '/permission', icon: <LockOutlined />, label: '权限管理',
    children: [
      { key: '/system/users', icon: <UserOutlined />, label: '用户管理' },
      { key: '/system/roles', icon: <TeamOutlined />, label: '角色管理' },
      { key: '/system/menus', icon: <AppstoreOutlined />, label: '菜单管理' },
      { key: '/system/permissions', icon: <SafetyOutlined />, label: '权限管理' },
      { key: '/system/depts', icon: <ApartmentOutlined />, label: '部门管理' },
    ],
  },
  {
    key: '/system', icon: <SettingOutlined />, label: '系统管理',
    children: [
      { key: '/system/logs', icon: <FileTextOutlined />, label: '系统日志' },
      { key: '/system/services', icon: <CloudServerOutlined />, label: '服务配置' },
    ],
  },
  {
    key: '/shop', icon: <ShoppingOutlined />, label: '商城管理',
    children: [
      { key: '/shop/products', icon: <ShopOutlined />, label: '商品管理' },
      { key: '/shop/categories', icon: <TagsOutlined />, label: '分类管理' },
      { key: '/shop/orders', icon: <OrderedListOutlined />, label: '订单管理' },
    ],
  },
];

const MainLayout: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useAppDispatch();
  const collapsed = useAppSelector((s) => s.menu.collapsed);
  const realName = useAppSelector((s) => s.auth.realName || s.auth.userName || '用户');
  const token = useAppSelector((s) => s.auth.token);

  useEffect(() => {
    if (!token) navigate('/login', { replace: true });
  }, [token, navigate]);

  const handleMenuClick: MenuProps['onClick'] = ({ key }) => navigate(key);

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login', { replace: true });
  };

  const userMenuItems: MenuProps['items'] = [
    { key: 'logout', icon: <LogoutOutlined />, label: '退出登录', onClick: handleLogout },
  ];

  const selectedKeys = [location.pathname];
  const openKeys = ['/' + location.pathname.split('/')[1]];

  return (
    <Layout style={{ height: '100vh', overflow: 'hidden' }}>
      <Sider
        trigger={null} collapsible collapsed={collapsed}
        width={220} collapsedWidth={64}
        style={{
          background: 'linear-gradient(180deg, #2D1B69 0%, #1A1145 100%)',
          height: '100vh', overflow: 'auto', flexShrink: 0,
        }}
      >
        <div style={{
          height: 56, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 10,
          borderBottom: '1px solid rgba(255,255,255,0.08)',
        }}>
          <div style={{
            width: 32, height: 32, borderRadius: 8,
            background: 'linear-gradient(135deg, #A29BFE, #6C5CE7)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: '#fff', fontWeight: 'bold', fontSize: 15,
          }}>Q</div>
          {!collapsed && <span style={{ color: '#fff', fontSize: 15, fontWeight: 600 }}>Qvzhongren</span>}
        </div>
        <Menu
          mode="inline" selectedKeys={selectedKeys} defaultOpenKeys={openKeys}
          items={menuItems} onClick={handleMenuClick} theme="dark"
          style={{ background: 'transparent', borderRight: 'none', marginTop: 4 }}
        />
      </Sider>
      <Layout style={{ overflow: 'hidden', background: '#F4F2FF' }}>
        <Header style={{
          height: 56, lineHeight: '56px', padding: '0 20px', background: '#fff',
          display: 'flex', alignItems: 'center', justifyContent: 'space-between',
          boxShadow: '0 1px 4px rgba(108,92,231,0.06)', flexShrink: 0,
        }}>
          <Button type="text" icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => dispatch(toggleCollapsed())} style={{ fontSize: 16 }} />
          <Dropdown menu={{ items: userMenuItems }} placement="bottomRight">
            <div style={{ display: 'flex', alignItems: 'center', gap: 8, cursor: 'pointer' }}>
              <Avatar size={30} style={{ background: 'linear-gradient(135deg, #A29BFE, #6C5CE7)' }} icon={<UserOutlined />} />
              <span style={{ fontWeight: 500, fontSize: 14 }}>{realName}</span>
            </div>
          </Dropdown>
        </Header>
        <Content style={{ margin: 16, padding: 20, background: '#fff', borderRadius: 12, overflow: 'auto', flex: 1 }}>
          <Outlet />
        </Content>
      </Layout>

      <ChatFloat />
    </Layout>
  );
};

export default MainLayout;
