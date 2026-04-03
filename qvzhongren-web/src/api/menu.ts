import request from './request';
import type { ResultDto, SysMenuCreateDto, SysMenuResponseDto } from '../types';

export function getMenuList() {
  return request.post<ResultDto<SysMenuResponseDto[]>>('/sysmenu/getlist');
}

export function getMenuTree() {
  return request.post<ResultDto<SysMenuResponseDto[]>>('/sysmenu/getmenutree');
}

export function createMenu(data: SysMenuCreateDto) {
  return request.post<ResultDto<boolean>>('/sysmenu/create', data);
}

export function updateMenu(data: SysMenuCreateDto) {
  return request.post<ResultDto<boolean>>('/sysmenu/update', data);
}

export function deleteMenu(menuId: string) {
  return request.post<ResultDto<boolean>>('/sysmenu/delete', menuId, {
    headers: { 'Content-Type': 'application/json' },
  });
}
