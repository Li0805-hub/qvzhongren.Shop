import React from 'react';
import { Form, Input, Button, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useAppDispatch } from '../store';
import { loginSuccess } from '../store/authSlice';
import { login } from '../api/auth';
import type { LoginDto } from '../types';

const LoginIllustration: React.FC = () => (
  <svg viewBox="0 0 500 500" fill="none" style={{ width: '80%', maxWidth: 400 }}>
    <defs>
      <linearGradient id="g1" x1="0" y1="0" x2="1" y2="1">
        <stop offset="0%" stopColor="#A29BFE" />
        <stop offset="100%" stopColor="#6C5CE7" />
      </linearGradient>
      <linearGradient id="g2" x1="0" y1="0" x2="0" y2="1">
        <stop offset="0%" stopColor="#DFE6E9" />
        <stop offset="100%" stopColor="#B2BEC3" />
      </linearGradient>
    </defs>
    {/* Floor */}
    <ellipse cx="250" cy="430" rx="200" ry="30" fill="#EDE8FF" opacity="0.5" />
    {/* Desk */}
    <rect x="100" y="280" rx="8" width="300" height="12" fill="url(#g2)" />
    <rect x="130" y="292" width="12" height="120" rx="4" fill="#B2BEC3" />
    <rect x="358" y="292" width="12" height="120" rx="4" fill="#B2BEC3" />
    {/* Monitor */}
    <rect x="155" y="160" rx="12" width="190" height="120" fill="#2D1B69" />
    <rect x="163" y="168" rx="6" width="174" height="96" fill="#1A1145" />
    {/* Screen content */}
    <rect x="178" y="188" rx="3" width="60" height="6" fill="#6C5CE7" opacity="0.8" />
    <rect x="178" y="200" rx="3" width="100" height="4" fill="#A29BFE" opacity="0.5" />
    <rect x="178" y="210" rx="3" width="80" height="4" fill="#A29BFE" opacity="0.5" />
    <rect x="178" y="224" rx="4" width="40" height="14" fill="url(#g1)" />
    <rect x="224" y="224" rx="4" width="40" height="14" fill="#00B894" opacity="0.7" />
    <circle cx="300" cy="215" r="22" fill="url(#g1)" opacity="0.3" />
    <path d="M290 215 L297 225 L312 205" stroke="#A29BFE" strokeWidth="3" fill="none" />
    {/* Monitor stand */}
    <rect x="235" y="280" width="30" height="4" rx="2" fill="#B2BEC3" />
    <rect x="245" y="270" width="10" height="14" rx="2" fill="#B2BEC3" />
    {/* Keyboard */}
    <rect x="185" y="288" rx="3" width="130" height="8" fill="#DFE6E9" />
    {/* Coffee cup */}
    <rect x="340" y="262" rx="4" width="22" height="18" fill="#FDCB6E" />
    <path d="M362 266 Q372 270 362 276" stroke="#FDCB6E" strokeWidth="2.5" fill="none" />
    <path d="M346 258 Q348 250 350 258" stroke="#B2BEC3" strokeWidth="1.5" fill="none" opacity="0.6" />
    <path d="M352 256 Q354 246 356 256" stroke="#B2BEC3" strokeWidth="1.5" fill="none" opacity="0.4" />
    {/* Plant */}
    <rect x="108" y="260" rx="4" width="24" height="20" fill="#E17055" opacity="0.7" />
    <ellipse cx="120" cy="252" rx="16" ry="14" fill="#00B894" />
    <ellipse cx="112" cy="248" rx="10" ry="12" fill="#00B894" opacity="0.8" />
    <ellipse cx="128" cy="250" rx="10" ry="10" fill="#55EFC4" opacity="0.6" />
    {/* Person - body */}
    <circle cx="250" cy="100" r="32" fill="#FFEAA7" />
    <ellipse cx="250" cy="88" rx="28" ry="20" fill="#2D1B69" />
    <circle cx="240" cy="104" r="3" fill="#2D1B69" />
    <circle cx="260" cy="104" r="3" fill="#2D1B69" />
    <path d="M245 114 Q250 119 255 114" stroke="#E17055" strokeWidth="2" fill="none" />
    {/* Body */}
    <path d="M220 132 Q250 145 280 132 L290 200 Q250 215 210 200 Z" fill="url(#g1)" />
    {/* Arms */}
    <path d="M215 145 Q180 180 195 240 L205 238 Q195 185 225 155" fill="#A29BFE" />
    <path d="M285 145 Q320 180 305 240 L295 238 Q305 185 275 155" fill="#A29BFE" />
    {/* Hands on keyboard */}
    <circle cx="200" cy="242" r="8" fill="#FFEAA7" />
    <circle cx="300" cy="242" r="8" fill="#FFEAA7" />
    {/* Floating elements */}
    <circle cx="80" cy="100" r="6" fill="#6C5CE7" opacity="0.2" />
    <circle cx="420" cy="80" r="8" fill="#00B894" opacity="0.2" />
    <circle cx="400" cy="180" r="5" fill="#FDCB6E" opacity="0.3" />
    <rect x="60" y="200" rx="3" width="20" height="20" fill="#A29BFE" opacity="0.1" transform="rotate(15 70 210)" />
    <rect x="410" y="300" rx="3" width="16" height="16" fill="#6C5CE7" opacity="0.1" transform="rotate(30 418 308)" />
  </svg>
);

const Login: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const [loading, setLoading] = React.useState(false);

  const onFinish = async (values: LoginDto) => {
    setLoading(true);
    try {
      const { data: res } = await login(values);
      if (res.isSuccess && res.data) {
        dispatch(loginSuccess({
          token: res.data.token,
          userId: res.data.userId,
          userName: res.data.userName,
          realName: res.data.realName,
        }));
        message.success('登录成功');
        navigate('/dashboard', { replace: true });
      }
    } catch {
      // handled by interceptor
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      display: 'flex',
      minHeight: '100vh',
      background: 'linear-gradient(135deg, #F4F2FF 0%, #EDE8FF 50%, #E8F8F5 100%)',
    }}>
      {/* Left - Illustration */}
      <div style={{
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        padding: 40,
      }}>
        <LoginIllustration />
        <h2 style={{ color: '#2D1B69', marginTop: 24, fontSize: 22, fontWeight: 600 }}>
          智能管理，高效协同
        </h2>
        <p style={{ color: '#636E72', fontSize: 14, marginTop: 8 }}>
          全方位系统管理平台，助力数字化转型
        </p>
      </div>

      {/* Right - Login Form */}
      <div style={{
        flex: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        padding: 40,
      }}>
        <div style={{
          width: '100%',
          maxWidth: 400,
          padding: '48px 40px',
          background: '#fff',
          borderRadius: 24,
          boxShadow: '0 20px 60px rgba(108,92,231,0.1)',
        }}>
          <div style={{ textAlign: 'center', marginBottom: 40 }}>
            <div style={{
              width: 56, height: 56, borderRadius: 16, margin: '0 auto 16px',
              background: 'linear-gradient(135deg, #A29BFE, #6C5CE7)',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              color: '#fff', fontSize: 24, fontWeight: 'bold',
            }}>Q</div>
            <h1 style={{ fontSize: 24, fontWeight: 700, color: '#2D1B69', margin: 0 }}>
              Qvzhongren
            </h1>
            <p style={{ color: '#A0A0B0', fontSize: 14, marginTop: 8 }}>管理系统</p>
          </div>

          <Form name="login" onFinish={onFinish} autoComplete="off" size="large">
            <Form.Item name="userName" rules={[{ required: true, message: '请输入用户名' }]}>
              <Input
                prefix={<UserOutlined style={{ color: '#A29BFE' }} />}
                placeholder="用户名"
                style={{ height: 48, borderRadius: 12 }}
              />
            </Form.Item>
            <Form.Item name="password" rules={[{ required: true, message: '请输入密码' }]}>
              <Input.Password
                prefix={<LockOutlined style={{ color: '#A29BFE' }} />}
                placeholder="密码"
                style={{ height: 48, borderRadius: 12 }}
              />
            </Form.Item>
            <Form.Item style={{ marginBottom: 16 }}>
              <Button
                type="primary"
                htmlType="submit"
                loading={loading}
                block
                style={{
                  height: 48,
                  borderRadius: 12,
                  fontSize: 16,
                  fontWeight: 600,
                  background: 'linear-gradient(135deg, #A29BFE, #6C5CE7)',
                  border: 'none',
                  boxShadow: '0 8px 24px rgba(108,92,231,0.3)',
                }}
              >
                登 录
              </Button>
            </Form.Item>
          </Form>
        </div>
      </div>
    </div>
  );
};

export default Login;
