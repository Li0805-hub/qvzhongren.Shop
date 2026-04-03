import request from './request';
import type { ResultDto } from '../types';

export interface ServiceConfigDto {
  configId: string;
  serviceName: string;
  serviceUrl: string;
  description?: string;
  status: string;
  sortNo?: number;
  createCode?: string;
  createDate?: string;
  updateCode?: string;
  updateDate?: string;
}

export interface ServiceConfigCreateDto {
  configId?: string;
  serviceName: string;
  serviceUrl: string;
  description?: string;
  status: string;
  sortNo?: number;
}

export function getServiceConfigList() {
  return request.post<ResultDto<ServiceConfigDto[]>>('/ServiceConfig/GetList');
}

export function createServiceConfig(data: ServiceConfigCreateDto) {
  return request.post<ResultDto<boolean>>('/ServiceConfig/Create', data);
}

export function updateServiceConfig(data: ServiceConfigCreateDto) {
  return request.post<ResultDto<boolean>>('/ServiceConfig/Update', data);
}

export function deleteServiceConfig(configId: string) {
  return request.post<ResultDto<boolean>>('/ServiceConfig/Delete', configId, {
    headers: { 'Content-Type': 'application/json' },
  });
}
