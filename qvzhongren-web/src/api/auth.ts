import request from './request';
import type { ResultDto, LoginDto, LoginResponseDto } from '../types';

export function login(data: LoginDto) {
  return request.post<ResultDto<LoginResponseDto>>('/auth/login', data);
}

export function initAdmin() {
  return request.post<ResultDto<boolean>>('/auth/initadmin');
}
