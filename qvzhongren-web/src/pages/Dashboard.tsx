import React, { useEffect, useState } from 'react';
import { Card, Col, Row, Table, Tag } from 'antd';
import {
  UserOutlined,
  TeamOutlined,
  AppstoreOutlined,
  SafetyOutlined,
  ArrowUpOutlined,
} from '@ant-design/icons';
import {
  AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer,
  BarChart, Bar,
  PieChart, Pie, Cell,
} from 'recharts';
import { getUserList } from '../api/user';
import { getRoleList } from '../api/role';
import { getMenuList } from '../api/menu';
import { getPermissionList } from '../api/permission';
import type { SysUserResponseDto } from '../types';

const trendData = [
  { name: '1月', 用户: 4, 操作: 24 },
  { name: '2月', 用户: 6, 操作: 38 },
  { name: '3月', 用户: 8, 操作: 55 },
  { name: '4月', 用户: 12, 操作: 72 },
  { name: '5月', 用户: 15, 操作: 90 },
  { name: '6月', 用户: 18, 操作: 110 },
  { name: '7月', 用户: 22, 操作: 135 },
];

const moduleData = [
  { name: '用户管理', value: 35 },
  { name: '角色管理', value: 20 },
  { name: '菜单管理', value: 25 },
  { name: '医生工作站', value: 20 },
];

const weeklyData = [
  { name: '周一', 操作: 32 },
  { name: '周二', 操作: 45 },
  { name: '周三', 操作: 28 },
  { name: '周四', 操作: 56 },
  { name: '周五', 操作: 42 },
  { name: '周六', 操作: 18 },
  { name: '周日', 操作: 12 },
];

const PIE_COLORS = ['#6C5CE7', '#00B894', '#FDCB6E', '#E17055'];

interface StatCardProps {
  title: string;
  value: number;
  icon: React.ReactNode;
  gradient: string;
  trend: string;
  loading: boolean;
}

const StatCard: React.FC<StatCardProps> = ({ title, value, icon, gradient, trend, loading }) => (
  <Card
    loading={loading}
    style={{ borderRadius: 16, border: 'none', overflow: 'hidden' }}
    styles={{ body: { padding: '20px 24px' } }}
  >
    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
      <div>
        <div style={{ color: '#8C8C8C', fontSize: 14, marginBottom: 8 }}>{title}</div>
        <div style={{ fontSize: 32, fontWeight: 700, color: '#2D1B69' }}>{value}</div>
        <div style={{ marginTop: 8, fontSize: 12 }}>
          <ArrowUpOutlined style={{ color: '#00B894', marginRight: 4 }} />
          <span style={{ color: '#00B894' }}>{trend}</span>
        </div>
      </div>
      <div style={{
        width: 52, height: 52, borderRadius: 14,
        background: gradient,
        display: 'flex', alignItems: 'center', justifyContent: 'center',
        fontSize: 22, color: '#fff',
      }}>
        {icon}
      </div>
    </div>
  </Card>
);

const Dashboard: React.FC = () => {
  const [counts, setCounts] = useState({ users: 0, roles: 0, menus: 0, permissions: 0 });
  const [recentUsers, setRecentUsers] = useState<SysUserResponseDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [usersRes, rolesRes, menusRes, permsRes] = await Promise.all([
          getUserList(),
          getRoleList(),
          getMenuList(),
          getPermissionList(),
        ]);
        setCounts({
          users: usersRes.data.data?.length ?? 0,
          roles: rolesRes.data.data?.length ?? 0,
          menus: menusRes.data.data?.length ?? 0,
          permissions: permsRes.data.data?.length ?? 0,
        });
        setRecentUsers((usersRes.data.data ?? []).slice(0, 5));
      } catch {
        // handled by interceptor
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  const columns = [
    { title: '用户名', dataIndex: 'userName', key: 'userName' },
    { title: '真实姓名', dataIndex: 'realName', key: 'realName' },
    { title: '手机号', dataIndex: 'phone', key: 'phone' },
    { title: '邮箱', dataIndex: 'email', key: 'email' },
    {
      title: '状态', dataIndex: 'status', key: 'status',
      render: (status: string) => (
        <Tag color={status === '1' ? '#00B894' : '#E17055'} style={{ borderRadius: 6 }}>
          {status === '1' ? '启用' : '禁用'}
        </Tag>
      ),
    },
  ];

  return (
    <div>
      {/* Stat Cards */}
      <Row gutter={20} style={{ marginBottom: 20 }}>
        <Col span={6}>
          <StatCard
            title="用户总数" value={counts.users} loading={loading}
            icon={<UserOutlined />}
            gradient="linear-gradient(135deg, #A29BFE, #6C5CE7)"
            trend="+12%"
          />
        </Col>
        <Col span={6}>
          <StatCard
            title="角色总数" value={counts.roles} loading={loading}
            icon={<TeamOutlined />}
            gradient="linear-gradient(135deg, #55EFC4, #00B894)"
            trend="+5%"
          />
        </Col>
        <Col span={6}>
          <StatCard
            title="菜单总数" value={counts.menus} loading={loading}
            icon={<AppstoreOutlined />}
            gradient="linear-gradient(135deg, #FFEAA7, #FDCB6E)"
            trend="+8%"
          />
        </Col>
        <Col span={6}>
          <StatCard
            title="权限总数" value={counts.permissions} loading={loading}
            icon={<SafetyOutlined />}
            gradient="linear-gradient(135deg, #FAB1A0, #E17055)"
            trend="+3%"
          />
        </Col>
      </Row>

      {/* Charts Row */}
      <Row gutter={20} style={{ marginBottom: 20 }}>
        <Col span={16}>
          <Card
            title="增长趋势"
            style={{ borderRadius: 16, border: 'none' }}
            styles={{ header: { borderBottom: 'none', fontWeight: 600 } }}
          >
            <ResponsiveContainer width="100%" height={300}>
              <AreaChart data={trendData}>
                <defs>
                  <linearGradient id="colorUsers" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#6C5CE7" stopOpacity={0.3} />
                    <stop offset="95%" stopColor="#6C5CE7" stopOpacity={0} />
                  </linearGradient>
                  <linearGradient id="colorOps" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#00B894" stopOpacity={0.3} />
                    <stop offset="95%" stopColor="#00B894" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#F0F0F0" />
                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{ fill: '#8C8C8C', fontSize: 12 }} />
                <YAxis axisLine={false} tickLine={false} tick={{ fill: '#8C8C8C', fontSize: 12 }} />
                <Tooltip
                  contentStyle={{ borderRadius: 12, border: 'none', boxShadow: '0 4px 20px rgba(0,0,0,0.08)' }}
                />
                <Area type="monotone" dataKey="用户" stroke="#6C5CE7" strokeWidth={2.5} fillOpacity={1} fill="url(#colorUsers)" />
                <Area type="monotone" dataKey="操作" stroke="#00B894" strokeWidth={2.5} fillOpacity={1} fill="url(#colorOps)" />
              </AreaChart>
            </ResponsiveContainer>
          </Card>
        </Col>
        <Col span={8}>
          <Card
            title="模块使用分布"
            style={{ borderRadius: 16, border: 'none' }}
            styles={{ header: { borderBottom: 'none', fontWeight: 600 } }}
          >
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={moduleData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={95}
                  paddingAngle={4}
                  dataKey="value"
                  stroke="none"
                >
                  {moduleData.map((_, index) => (
                    <Cell key={`cell-${index}`} fill={PIE_COLORS[index % PIE_COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip
                  contentStyle={{ borderRadius: 12, border: 'none', boxShadow: '0 4px 20px rgba(0,0,0,0.08)' }}
                />
              </PieChart>
            </ResponsiveContainer>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: 12, justifyContent: 'center', marginTop: -8 }}>
              {moduleData.map((item, i) => (
                <div key={item.name} style={{ display: 'flex', alignItems: 'center', gap: 6, fontSize: 12 }}>
                  <div style={{ width: 8, height: 8, borderRadius: 4, background: PIE_COLORS[i] }} />
                  <span style={{ color: '#8C8C8C' }}>{item.name}</span>
                </div>
              ))}
            </div>
          </Card>
        </Col>
      </Row>

      {/* Bottom Row */}
      <Row gutter={20}>
        <Col span={10}>
          <Card
            title="本周活跃度"
            style={{ borderRadius: 16, border: 'none' }}
            styles={{ header: { borderBottom: 'none', fontWeight: 600 } }}
          >
            <ResponsiveContainer width="100%" height={240}>
              <BarChart data={weeklyData}>
                <CartesianGrid strokeDasharray="3 3" stroke="#F0F0F0" vertical={false} />
                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{ fill: '#8C8C8C', fontSize: 12 }} />
                <YAxis axisLine={false} tickLine={false} tick={{ fill: '#8C8C8C', fontSize: 12 }} />
                <Tooltip
                  contentStyle={{ borderRadius: 12, border: 'none', boxShadow: '0 4px 20px rgba(0,0,0,0.08)' }}
                />
                <Bar dataKey="操作" radius={[6, 6, 0, 0]} fill="url(#barGradient)" barSize={28}>
                  <defs>
                    <linearGradient id="barGradient" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="0%" stopColor="#A29BFE" />
                      <stop offset="100%" stopColor="#6C5CE7" />
                    </linearGradient>
                  </defs>
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          </Card>
        </Col>
        <Col span={14}>
          <Card
            title="最近用户"
            style={{ borderRadius: 16, border: 'none' }}
            styles={{ header: { borderBottom: 'none', fontWeight: 600 } }}
          >
            <Table
              columns={columns}
              dataSource={recentUsers}
              rowKey="userId"
              pagination={false}
              loading={loading}
              size="small"
            />
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Dashboard;
