const TOKEN_KEY = 'qvzhongren_token';
const USER_KEY = 'qvzhongren_user';

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token);
}

export function removeToken(): void {
  localStorage.removeItem(TOKEN_KEY);
}

export function getUserInfo(): { userId: string; userName: string; realName?: string } | null {
  const raw = localStorage.getItem(USER_KEY);
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

export function setUserInfo(info: { userId: string; userName: string; realName?: string }): void {
  localStorage.setItem(USER_KEY, JSON.stringify(info));
}

export function removeUserInfo(): void {
  localStorage.removeItem(USER_KEY);
}

export function clearAuth(): void {
  removeToken();
  removeUserInfo();
}
