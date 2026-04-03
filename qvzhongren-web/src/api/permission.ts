import request from './request';
import type { ResultDto, SysPermissionCreateDto, SysPermissionResponseDto } from '../types';

export function getPermissionList() {
  return request.post<ResultDto<SysPermissionResponseDto[]>>('/syspermission/getlist');
}

export function createPermission(data: SysPermissionCreateDto) {
  return request.post<ResultDto<boolean>>('/syspermission/create', data);
}

export function updatePermission(data: SysPermissionCreateDto) {
  return request.post<ResultDto<boolean>>('/syspermission/update', data);
}

export function deletePermission(permissionId: string) {
  return request.post<ResultDto<boolean>>('/syspermission/delete', permissionId, {
    headers: { 'Content-Type': 'application/json' },
  });
}
