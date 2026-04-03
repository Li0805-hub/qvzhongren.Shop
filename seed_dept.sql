-- Add dept menu
INSERT INTO "SYS_MENU" ("MENU_ID","MENU_NAME","PARENT_ID","PATH","COMPONENT","ICON","MENU_TYPE","PERMS","SORT_NO","STATUS","CREATE_CODE","CREATE_DATE","UPDATE_CODE","UPDATE_DATE") VALUES
('menu-depts','部门管理','menu-system','/system/depts','system/DeptList','ApartmentOutlined','C','system:dept:list',6,'1','admin',NOW(),'admin',NOW())
ON CONFLICT ("MENU_ID") DO NOTHING;

INSERT INTO "SYS_ROLE_MENU" ("ROLE_ID","MENU_ID")
SELECT 'role-admin', 'menu-depts'
WHERE NOT EXISTS (SELECT 1 FROM "SYS_ROLE_MENU" WHERE "ROLE_ID"='role-admin' AND "MENU_ID"='menu-depts');

-- Seed some departments
INSERT INTO "SYS_DEPT" ("DEPT_CODE","DEPT_NAME","PARENT_CODE","SORT_NO","STATUS","CREATE_CODE","CREATE_DATE","UPDATE_CODE","UPDATE_DATE") VALUES
('dept-root','总部','0',1,'1','admin',NOW(),'admin',NOW()),
('dept-tech','技术部','dept-root',1,'1','admin',NOW(),'admin',NOW()),
('dept-product','产品部','dept-root',2,'1','admin',NOW(),'admin',NOW()),
('dept-hr','人事部','dept-root',3,'1','admin',NOW(),'admin',NOW()),
('dept-finance','财务部','dept-root',4,'1','admin',NOW(),'admin',NOW())
ON CONFLICT ("DEPT_CODE") DO NOTHING;
