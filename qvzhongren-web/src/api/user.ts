import request from './request';
import type { ResultDto, SysUserCreateDto, SysUserResponseDto, UserRoleAssignDto } from '../types';

export function getUserList() {
  return request.post<ResultDto<SysUserResponseDto[]>>('/sysuser/getlist');
}

export function getUserById(userId: string) {
  return request.post<ResultDto<SysUserResponseDto>>('/sysuser/getbyid', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function createUser(data: SysUserCreateDto) {
  return request.post<ResultDto<boolean>>('/sysuser/create', data);
}

export function updateUser(data: SysUserCreateDto) {
  return request.post<ResultDto<boolean>>('/sysuser/update', data);
}

export function deleteUser(userId: string) {
  return request.post<ResultDto<boolean>>('/sysuser/delete', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function assignRoles(data: UserRoleAssignDto) {
  return request.post<ResultDto<boolean>>('/sysuser/assignroles', data);
}

export function getUserRoles(userId: string) {
  return request.post<ResultDto<string[]>>('/sysuser/getuserroles', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function assignUserMenus(data: { userId: string; menuIds: string[] }) {
  return request.post<ResultDto<boolean>>('/sysuser/assignmenus', data);
}

export function getUserMenus(userId: string) {
  return request.post<ResultDto<string[]>>('/sysuser/getusermenus', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function resetPassword(userId: string) {
  return request.post<ResultDto<boolean>>('/auth/resetpassword', userId, {
    headers: { 'Content-Type': 'application/json' },
  });
}
