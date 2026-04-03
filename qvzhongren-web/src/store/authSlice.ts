import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { getToken, getUserInfo, setToken, setUserInfo, clearAuth } from '../utils/auth';

interface AuthState {
  token: string | null;
  userId: string | null;
  userName: string | null;
  realName: string | null;
}

const userInfo = getUserInfo();

const initialState: AuthState = {
  token: getToken(),
  userId: userInfo?.userId ?? null,
  userName: userInfo?.userName ?? null,
  realName: userInfo?.realName ?? null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginSuccess(state, action: PayloadAction<{ token: string; userId: string; userName: string; realName?: string }>) {
      const { token, userId, userName, realName } = action.payload;
      state.token = token;
      state.userId = userId;
      state.userName = userName;
      state.realName = realName ?? null;
      setToken(token);
      setUserInfo({ userId, userName, realName });
    },
    logout(state) {
      state.token = null;
      state.userId = null;
      state.userName = null;
      state.realName = null;
      clearAuth();
    },
  },
});

export const { loginSuccess, logout } = authSlice.actions;
export default authSlice.reducer;
