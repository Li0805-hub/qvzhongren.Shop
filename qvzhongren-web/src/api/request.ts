import axios from 'axios';
import { message } from 'antd';
import { getToken, clearAuth } from '../utils/auth';
import type { ResultDto } from '../types';

const request = axios.create({
  baseURL: '/api',
  timeout: 15000,
  headers: { 'Content-Type': 'application/json' },
});

request.interceptors.request.use((config) => {
  const token = getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

request.interceptors.response.use(
  (response) => {
    const res = response.data as ResultDto<unknown>;
    if (!res.isSuccess) {
      message.error(res.message || '请求失败');
      return Promise.reject(new Error(res.message));
    }
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      clearAuth();
      window.location.href = '/login';
    } else {
      message.error(error.response?.data?.message || error.message || '网络错误');
    }
    return Promise.reject(error);
  },
);

export default request;
