-- Add log menu
INSERT INTO "SYS_MENU" ("MENU_ID","MENU_NAME","PARENT_ID","PATH","COMPONENT","ICON","MENU_TYPE","PERMS","SORT_NO","STATUS","CREATE_CODE","CREATE_DATE","UPDATE_CODE","UPDATE_DATE") VALUES
('menu-logs','系统日志','menu-system','/system/logs','system/LogList','FileTextOutlined','C','system:log:list',5,'1','admin',NOW(),'admin',NOW())
ON CONFLICT ("MENU_ID") DO NOTHING;

-- Assign to admin role
INSERT INTO "SYS_ROLE_MENU" ("ROLE_ID","MENU_ID")
SELECT 'role-admin', 'menu-logs'
WHERE NOT EXISTS (SELECT 1 FROM "SYS_ROLE_MENU" WHERE "ROLE_ID"='role-admin' AND "MENU_ID"='menu-logs');
