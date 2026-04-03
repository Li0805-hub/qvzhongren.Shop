import request from './request';
import type { ResultDto, SysRoleCreateDto, SysRoleResponseDto, RoleMenuAssignDto } from '../types';

export function getRoleList() {
  return request.post<ResultDto<SysRoleResponseDto[]>>('/sysrole/getlist');
}

export function createRole(data: SysRoleCreateDto) {
  return request.post<ResultDto<boolean>>('/sysrole/create', data);
}

export function updateRole(data: SysRoleCreateDto) {
  return request.post<ResultDto<boolean>>('/sysrole/update', data);
}

export function deleteRole(roleId: string) {
  return request.post<ResultDto<boolean>>('/sysrole/delete', roleId, {
    headers: { 'Content-Type': 'application/json' },
  });
}

export function assignMenus(data: RoleMenuAssignDto) {
  return request.post<ResultDto<boolean>>('/sysrole/assignmenus', data);
}

export function getRoleMenus(roleId: string) {
  return request.post<ResultDto<string[]>>('/sysrole/getrolemenus', roleId, {
    headers: { 'Content-Type': 'application/json' },
  });
}
