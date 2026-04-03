import request from './request';
import type { ResultDto, ListPageResultDto } from '../types';

export interface SysLog {
  id: string;
  name: string;
  type: string;
  content: string;
  requestBody?: string;
  responseBody?: string;
  timestamp: string;
}

export interface LogQueryDto {
  type?: string;
  keyword?: string;
  startTime?: string;
  endTime?: string;
  pageIndex: number;
  pageSize: number;
}

export interface LogDeleteDto {
  startTime?: string;
  endTime?: string;
  type?: string;
  keyword?: string;
}

export function getLogPage(query: LogQueryDto) {
  return request.post<ResultDto<ListPageResultDto<SysLog>>>('/log/getpage', query);
}

export function deleteLogBatch(dto: LogDeleteDto) {
  return request.post<ResultDto<number>>('/log/deletebatch', dto);
}
