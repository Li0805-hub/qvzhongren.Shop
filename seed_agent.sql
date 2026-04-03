-- Remove doctor menus
DELETE FROM "SYS_ROLE_MENU" WHERE "MENU_ID" IN ('menu-doctor', 'menu-allergies');
DELETE FROM "SYS_MENU" WHERE "MENU_ID" IN ('menu-doctor', 'menu-allergies');

-- Add agent menu
INSERT INTO "SYS_MENU" ("MENU_ID","MENU_NAME","PARENT_ID","PATH","COMPONENT","ICON","MENU_TYPE","PERMS","SORT_NO","STATUS","CREATE_CODE","CREATE_DATE","UPDATE_CODE","UPDATE_DATE") VALUES
('menu-agent','智能助手','0','/agent','agent/AgentChat','RobotOutlined','C','agent:chat',3,'1','admin',NOW(),'admin',NOW())
ON CONFLICT ("MENU_ID") DO NOTHING;

-- Assign to admin role
INSERT INTO "SYS_ROLE_MENU" ("ROLE_ID","MENU_ID")
SELECT 'role-admin', 'menu-agent'
WHERE NOT EXISTS (SELECT 1 FROM "SYS_ROLE_MENU" WHERE "ROLE_ID"='role-admin' AND "MENU_ID"='menu-agent');
