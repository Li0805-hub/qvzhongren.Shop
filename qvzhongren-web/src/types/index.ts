// ==================== Common ====================

export interface ResultDto<T> {
  isSuccess: boolean;
  code: number;
  message: string;
  data: T;
}

export interface ListPageResultDto<T> {
  totalCount: number;
  pageIndex: number;
  pageSize: number;
  values: T[];
}

export interface QueryPageDto {
  startDateTime?: string;
  endDateTime?: string;
  pageIndex: number;
  pageSize: number;
}

// ==================== Permission Module ====================

export interface SysUser {
  userId: string;
  userName: string;
  password?: string;
  realName?: string;
  phone?: string;
  email?: string;
  deptCode?: string;
  status: string;
  remark?: string;
  createCode?: string;
  createDate?: string;
  updateCode?: string;
  updateDate?: string;
}

export interface SysUserCreateDto {
  userId: string;
  userName: string;
  password: string;
  realName?: string;
  phone?: string;
  email?: string;
  deptCode?: string;
  status: string;
  remark?: string;
}

export interface SysUserResponseDto {
  userId: string;
  userName: string;
  realName?: string;
  phone?: string;
  email?: string;
  deptCode?: string;
  status: string;
  remark?: string;
  createCode?: string;
  createDate?: string;
}

export interface UserRoleAssignDto {
  userId: string;
  roleIds: string[];
}

// ==================== Role ====================

export interface SysRoleCreateDto {
  roleId: string;
  roleCode: string;
  roleName: string;
  description?: string;
  status: string;
  sortNo?: number;
}

export interface SysRoleResponseDto {
  roleId: string;
  roleCode: string;
  roleName: string;
  description?: string;
  status: string;
  sortNo?: number;
  createCode?: string;
  createDate?: string;
}

export interface RoleMenuAssignDto {
  roleId: string;
  menuIds: string[];
}

// ==================== Menu ====================

export interface SysMenuCreateDto {
  menuId: string;
  menuName: string;
  parentId: string;
  path?: string;
  component?: string;
  icon?: string;
  menuType: string;
  perms?: string;
  sortNo?: number;
  status: string;
}

export interface SysMenuResponseDto {
  menuId: string;
  menuName: string;
  parentId: string;
  path?: string;
  component?: string;
  icon?: string;
  menuType: string;
  perms?: string;
  sortNo?: number;
  status: string;
  children?: SysMenuResponseDto[];
  createCode?: string;
  createDate?: string;
}

// ==================== Permission ====================

export interface SysPermissionCreateDto {
  permissionId: string;
  permissionCode: string;
  permissionName: string;
  permissionType?: string;
  description?: string;
  status: string;
}

export interface SysPermissionResponseDto {
  permissionId: string;
  permissionCode: string;
  permissionName: string;
  permissionType?: string;
  description?: string;
  status: string;
  createCode?: string;
  createDate?: string;
}

// ==================== Doctor Module ====================

export interface FinIprAllergyResponseDto {
  patientNo: string;
  drugCode: string;
  drugName: string;
  skinResult: string;
}

export interface FinIprAllergyCreateDto {
  patientNo: string;
  drugCode: string;
  drugName: string;
  skinResult: string;
}

export interface ConsultationMasterResponseDto {
  cnslNo: string;
  patientNo: string;
  deptCode: string;
  clinicNo: string;
  applyDate?: string;
  consultationType: string;
  consultationExplain: string;
  beginDate?: string;
  endDate?: string;
  consultationStatus: string;
  applyDoctor: string;
  finishDate?: string;
}

export interface ConsultationDetailResponseDto {
  cnslNo: string;
  clinicNo: string;
  patientNo: string;
  subNo: string;
  consultationDept: string;
  consultationDoctor: string;
  commitDateTime?: string;
  consultationIdea: string;
  commitFlag: string;
}

// ==================== Auth ====================

export interface LoginDto {
  userName: string;
  password: string;
}

export interface LoginResponseDto {
  token: string;
  userId: string;
  userName: string;
  realName?: string;
}
