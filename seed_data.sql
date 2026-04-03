-- 插入菜单数据
INSERT INTO "SYS_MENU" ("MENU_ID","MENU_NAME","PARENT_ID","PATH","COMPONENT","ICON","MENU_TYPE","PERMS","SORT_NO","STATUS","CREATE_CODE","CREATE_DATE","UPDATE_CODE","UPDATE_DATE") VALUES
('menu-dashboard','仪表盘','0','/dashboard','Dashboard','DashboardOutlined','C',NULL,1,'1','admin',NOW(),'admin',NOW()),
('menu-system','系统管理','0','/system',NULL,'AppstoreOutlined','M',NULL,2,'1','admin',NOW(),'admin',NOW()),
('menu-users','用户管理','menu-system','/system/users','system/UserList','UserOutlined','C','system:user:list',1,'1','admin',NOW(),'admin',NOW()),
('menu-roles','角色管理','menu-system','/system/roles','system/RoleList','TeamOutlined','C','system:role:list',2,'1','admin',NOW(),'admin',NOW()),
('menu-menus','菜单管理','menu-system','/system/menus','system/MenuList','AppstoreOutlined','C','system:menu:list',3,'1','admin',NOW(),'admin',NOW()),
('menu-permissions','权限管理','menu-system','/system/permissions','system/PermissionList','SafetyOutlined','C','system:permission:list',4,'1','admin',NOW(),'admin',NOW()),
('menu-doctor','医生工作站','0','/doctor',NULL,'MedicineBoxOutlined','M',NULL,3,'1','admin',NOW(),'admin',NOW()),
('menu-allergies','过敏记录','menu-doctor','/doctor/allergies','doctor/AllergyList','MedicineBoxOutlined','C','doctor:allergy:list',1,'1','admin',NOW(),'admin',NOW())
ON CONFLICT ("MENU_ID") DO NOTHING;

-- 创建管理员角色
INSERT INTO "SYS_ROLE" ("ROLE_ID","ROLE_CODE","ROLE_NAME","DESCRIPTION","STATUS","SORT_NO","CREATE_CODE","CREATE_DATE","UPDATE_CODE","UPDATE_DATE") VALUES
('role-admin','admin','管理员','系统管理员，拥有所有权限','1',1,'admin',NOW(),'admin',NOW())
ON CONFLICT ("ROLE_ID") DO NOTHING;

-- 给管理员用户分配管理员角色
INSERT INTO "SYS_USER_ROLE" ("USER_ID","ROLE_ID")
SELECT u."USER_ID", 'role-admin' FROM "SYS_USER" u WHERE u."USER_NAME" = 'admin'
ON CONFLICT DO NOTHING;

-- 给管理员角色分配所有菜单
INSERT INTO "SYS_ROLE_MENU" ("ROLE_ID","MENU_ID")
SELECT 'role-admin', m."MENU_ID" FROM "SYS_MENU" m
ON CONFLICT DO NOTHING;
