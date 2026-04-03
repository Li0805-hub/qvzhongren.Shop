import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import type { SysMenuResponseDto } from '../types';

interface MenuState {
  menuTree: SysMenuResponseDto[];
  collapsed: boolean;
}

const initialState: MenuState = {
  menuTree: [],
  collapsed: false,
};

const menuSlice = createSlice({
  name: 'menu',
  initialState,
  reducers: {
    setMenuTree(state, action: PayloadAction<SysMenuResponseDto[]>) {
      state.menuTree = action.payload;
    },
    toggleCollapsed(state) {
      state.collapsed = !state.collapsed;
    },
  },
});

export const { setMenuTree, toggleCollapsed } = menuSlice.actions;
export default menuSlice.reducer;
