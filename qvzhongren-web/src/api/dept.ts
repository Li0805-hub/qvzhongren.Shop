import request from './request';
import type { ResultDto } from '../types';

export interface DeptTreeDto {
  deptCode: string;
  deptName: string;
  parentCode: string;
  sortNo?: number;
  status: string;
  leader?: string;
  phone?: string;
  createDate?: string;
  children?: DeptTreeDto[];
}

export interface DeptCreateDto {
  deptCode: string;
  deptName: string;
  parentCode?: string;
  sortNo?: number;
  status?: string;
  leader?: string;
  phone?: string;
}

export function getDeptTree() {
  return request.post<ResultDto<DeptTreeDto[]>>('/SysDept/GetTree');
}

export function createDept(data: DeptCreateDto) {
  return request.post<ResultDto<boolean>>('/SysDept/Create', data);
}

export function updateDept(data: DeptCreateDto) {
  return request.post<ResultDto<boolean>>('/SysDept/Update', data);
}

export function deleteDept(deptCode: string) {
  return request.post<ResultDto<boolean>>('/SysDept/Delete', deptCode, {
    headers: { 'Content-Type': 'application/json' },
  });
}
