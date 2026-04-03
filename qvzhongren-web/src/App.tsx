import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ConfigProvider } from 'antd';
import zhCN from 'antd/locale/zh_CN';
import MainLayout from './layouts/MainLayout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import UserList from './pages/system/UserList';
import RoleList from './pages/system/RoleList';
import MenuList from './pages/system/MenuList';
import PermissionList from './pages/system/PermissionList';
import LogList from './pages/system/LogList';
import DeptList from './pages/system/DeptList';
import ServiceConfigList from './pages/system/ServiceConfigList';
import CategoryList from './pages/shop/CategoryList';
import ProductList from './pages/shop/ProductList';
import OrderList from './pages/shop/OrderList';

const App: React.FC = () => {
  return (
    <ConfigProvider
      locale={zhCN}
      theme={{
        token: {
          colorPrimary: '#6C5CE7',
          colorSuccess: '#00B894',
          colorWarning: '#FDCB6E',
          colorError: '#E17055',
          colorInfo: '#74B9FF',
          borderRadius: 10,
          fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "PingFang SC", "Microsoft YaHei", sans-serif',
        },
        components: {
          Button: { borderRadius: 8 },
          Card: { borderRadiusLG: 16 },
          Table: { borderRadiusLG: 12 },
          Input: { borderRadius: 8 },
          Select: { borderRadius: 8 },
        },
      }}
    >
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<MainLayout />}>
            <Route index element={<Navigate to="/dashboard" replace />} />
            <Route path="dashboard" element={<Dashboard />} />
            <Route path="system/users" element={<UserList />} />
            <Route path="system/roles" element={<RoleList />} />
            <Route path="system/menus" element={<MenuList />} />
            <Route path="system/permissions" element={<PermissionList />} />
            <Route path="system/logs" element={<LogList />} />
            <Route path="system/depts" element={<DeptList />} />
            <Route path="system/services" element={<ServiceConfigList />} />
            <Route path="shop/categories" element={<CategoryList />} />
            <Route path="shop/products" element={<ProductList />} />
            <Route path="shop/orders" element={<OrderList />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </ConfigProvider>
  );
};

export default App;
