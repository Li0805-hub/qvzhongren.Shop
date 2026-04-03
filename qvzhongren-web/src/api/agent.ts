import request from './request';
import type { ResultDto } from '../types';

export interface AgentMessage {
  role: 'user' | 'assistant';
  content: string;
}

export interface AgentToolCall {
  name: string;
  arguments: string;
  result: string;
}

export interface AgentResponse {
  content: string;
  navigateTo?: string;
  toolCalls?: AgentToolCall[];
}

export function agentChat(messages: AgentMessage[]) {
  return request.post<ResultDto<AgentResponse>>('/agent/chat', { messages }, { timeout: 60000 });
}
