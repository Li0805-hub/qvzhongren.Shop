-- qvzhongren 数据库初始化脚本 (PostgreSQL)

-- 1. 会诊类型表
CREATE TABLE IF NOT EXISTS "COM_CONSULTATION_TYPE" (
    "TYPE_CODE"         VARCHAR(50) NOT NULL,
    "TYPE_NAME"         VARCHAR(100),
    "USE_FLAG"          VARCHAR(10),
    "CHARGE_ITEM_CODE"  VARCHAR(50),
    "ORDER_ITEM_CODE"   VARCHAR(50),
    "SERIAL_NO"         INTEGER,
    "HOS_UNIT_CODE"     VARCHAR(50),
    "UPDATE_CODE"       VARCHAR(50),
    "UPDATE_DATE"       TIMESTAMP,
    "CREATE_CODE"       VARCHAR(50),
    "CREATE_DATE"       TIMESTAMP,
    PRIMARY KEY ("TYPE_CODE")
);

-- 2. 会诊明细表
CREATE TABLE IF NOT EXISTS "MET_CONSULTATION_DETAIL" (
    "CNSL_NO"               VARCHAR(50) NOT NULL,
    "SUB_NO"                VARCHAR(50) NOT NULL,
    "CLINIC_NO"             VARCHAR(50),
    "PATIENT_NO"            VARCHAR(50),
    "CONSULTATION_DEPT"     VARCHAR(50),
    "CONSULTATION_DOCTOR"   VARCHAR(50),
    "COMMIT_DATE_TIME"      TIMESTAMP,
    "CONSULTATION_IDEA"     VARCHAR(2000),
    "COMMIT_FLAG"           VARCHAR(10),
    "HOS_UNIT_CODE"         VARCHAR(50),
    "UPDATE_CODE"           VARCHAR(50),
    "UPDATE_DATE"           TIMESTAMP,
    "CREATE_CODE"           VARCHAR(50),
    "CREATE_DATE"           TIMESTAMP,
    PRIMARY KEY ("CNSL_NO", "SUB_NO")
);

-- 3. 会诊主记录表
CREATE TABLE IF NOT EXISTS "MET_CONSULTATION_MASTER" (
    "CNSL_NO"                   VARCHAR(50) NOT NULL,
    "PATIENT_NO"                VARCHAR(50),
    "DEPT_CODE"                 VARCHAR(50),
    "CLINIC_NO"                 VARCHAR(50),
    "APPLY_DATE"                TIMESTAMP,
    "CONSULTATION_TYPE"         VARCHAR(50),
    "CONSULTATION_EXPLAIN"      VARCHAR(2000),
    "BEGIN_DATE"                TIMESTAMP,
    "END_DATE"                  TIMESTAMP,
    "CONSULTATION_STATUS"       VARCHAR(10),
    "APPLY_DOCTOR"              VARCHAR(50),
    "FINISH_DATE"               TIMESTAMP,
    "HOS_UNIT_CODE"             VARCHAR(50),
    "UPDATE_CODE"               VARCHAR(50),
    "UPDATE_DATE"               TIMESTAMP,
    "CREATE_CODE"               VARCHAR(50),
    "CREATE_DATE"               TIMESTAMP,
    PRIMARY KEY ("CNSL_NO")
);

-- 4. 诊断模板明细表
CREATE TABLE IF NOT EXISTS "COM_DIAG_TEMPLATE_DETAIL" (
    "DIAG_CODE"         VARCHAR(50) NOT NULL,
    "TEMPLATE_CODE"     VARCHAR(50) NOT NULL,
    "OPER_CODE"         VARCHAR(50),
    "OPER_DATE"         TIMESTAMP,
    "DIAG_CLASS"        VARCHAR(10),
    "DIAG_NAME"         VARCHAR(200),
    "SERIAL_NO"         NUMERIC,
    "HOS_UNIT_CODE"     VARCHAR(50),
    "UPDATE_CODE"       VARCHAR(50),
    "UPDATE_DATE"       TIMESTAMP,
    "CREATE_CODE"       VARCHAR(50),
    "CREATE_DATE"       TIMESTAMP,
    PRIMARY KEY ("DIAG_CODE", "TEMPLATE_CODE")
);

-- 5. 诊断模板主表
CREATE TABLE IF NOT EXISTS "COM_DIAG_TEMPLATE_MASTER" (
    "TEMPLATE_CODE"     VARCHAR(50) NOT NULL,
    "TEMPLATE_NAME"     VARCHAR(200),
    "OPER_CODE"         VARCHAR(50),
    "OPER_DATE"         TIMESTAMP,
    "DEPT_CODE"         VARCHAR(50),
    "DOCTOR_CODE"       VARCHAR(50),
    "HOS_UNIT_CODE"     VARCHAR(50),
    "UPDATE_CODE"       VARCHAR(50),
    "UPDATE_DATE"       TIMESTAMP,
    "CREATE_CODE"       VARCHAR(50),
    "CREATE_DATE"       TIMESTAMP,
    PRIMARY KEY ("TEMPLATE_CODE")
);

-- 6. 诊断记录表
CREATE TABLE IF NOT EXISTS "MET_COM_DIAGNOSIS" (
    "ID"                    VARCHAR(50) NOT NULL,
    "PATIENT_NO"            VARCHAR(50) NOT NULL,
    "CLINIC_NO"             VARCHAR(50) NOT NULL,
    "DIAG_TYPE"             VARCHAR(10) NOT NULL,
    "DIAG_CLASS"            VARCHAR(10),
    "DIAG_NO"               SMALLINT,
    "DIAG_CODE"             VARCHAR(50) NOT NULL,
    "DIAG_NAME"             VARCHAR(200),
    "DIAG_DESC"             VARCHAR(500),
    "DEPT_CODE"             VARCHAR(50),
    "DIAG_DOCTOR_CODE"      VARCHAR(50),
    "DIAG_DATE"             TIMESTAMP,
    "OPER_DATE"             TIMESTAMP,
    "DIAG_CHARGE_CODE"      VARCHAR(50),
    "CHARGE_SIGN_DATE"      TIMESTAMP,
    "CHARGE_OPER_DATE"      TIMESTAMP,
    "DIAG_CHIEF_CODE"       VARCHAR(50),
    "CHIEF_SIGN_DATE"       TIMESTAMP,
    "CHIEF_OPER_DATE"       TIMESTAMP,
    "LEVEL_FLAG"            VARCHAR(10),
    "MAIN_FLAG"             VARCHAR(10) NOT NULL,
    "SUSPECTED_FLAG"        VARCHAR(10) NOT NULL,
    "HOS_UNIT_CODE"         VARCHAR(50),
    "UPDATE_CODE"           VARCHAR(50),
    "UPDATE_DATE"           TIMESTAMP,
    "CREATE_CODE"           VARCHAR(50),
    "CREATE_DATE"           TIMESTAMP,
    PRIMARY KEY ("ID")
);

-- 7. 医嘱模板组表
CREATE TABLE IF NOT EXISTS "MET_COM_GROUP" (
    "GROUP_ID"          VARCHAR(50) NOT NULL,
    "GROUP_NAME"        VARCHAR(200) NOT NULL,
    "SPELL_CODE"        VARCHAR(100),
    "WB_CODE"           VARCHAR(100),
    "GROUP_TYPE"        VARCHAR(10),
    "GROUP_KIND"        VARCHAR(10) NOT NULL,
    "GROUP_CLASS"       VARCHAR(10),
    "PRESC_FLAG"        VARCHAR(10),
    "DEPT_CODE"         VARCHAR(50),
    "DOCTOR_CODE"       VARCHAR(50),
    "REMARK"            VARCHAR(500),
    "OPER_CODE"         VARCHAR(50),
    "OPER_DATE"         TIMESTAMP,
    "HOS_UNIT_CODE"     VARCHAR(50),
    "PRESC_ATTR"        VARCHAR(50),
    PRIMARY KEY ("GROUP_ID")
);

-- 8. 医嘱模板组明细表
CREATE TABLE IF NOT EXISTS "MET_COM_GROUPDETAIL" (
    "GROUP_ID"              VARCHAR(50) NOT NULL,
    "SEQ_ORDER"             VARCHAR(50) NOT NULL,
    "ITEM_CODE"             VARCHAR(50),
    "ITEM_NAME"             VARCHAR(200),
    "FREQUENCY_CODE"        VARCHAR(50),
    "USAGE_CODE"            VARCHAR(50),
    "ONCE_DOSE"             NUMERIC,
    "CLASS_CODE"            VARCHAR(50),
    "ORDER_NO"              SMALLINT,
    "ORDER_SUB_NO"          SMALLINT,
    "DECMPS_STATE"          VARCHAR(10),
    "FREQUENCY_COUNTS"      SMALLINT,
    "DRUG_CHARGE_ATTR"      VARCHAR(50),
    "DOCTOR_NOTE"           VARCHAR(500),
    "COMB_FLAG"             VARCHAR(10),
    "TOT_QTY"               NUMERIC,
    "ITEM_UNIT"             VARCHAR(50),
    "USE_DAYS"              SMALLINT,
    "EXEC_DEPT"             VARCHAR(50),
    "PHARMACY_CODE"         VARCHAR(50),
    "PACK_UNIT"             VARCHAR(50),
    "SPECS"                 VARCHAR(200),
    "PACK_QTY"              SMALLINT,
    "OPER_CODE"             VARCHAR(50),
    "OPER_DATE"             TIMESTAMP,
    "ADMINISTRATION_CODE"   VARCHAR(50),
    "DECOCTION_CODE"        VARCHAR(50),
    "MEDICINE_CODE"         VARCHAR(50),
    "MEDICINE_NOTE"         VARCHAR(500),
    "DOSAGE_COUNT"          SMALLINT,
    "PROCESS_CODE"          VARCHAR(50),
    "DOSAGE_FORM"           VARCHAR(50),
    "HOS_UNIT_CODE"         VARCHAR(50),
    "CHARGE_ATTR"           VARCHAR(10),
    "OPEN_UNIT"             VARCHAR(50),
    "OPEN_ONCE"             NUMERIC,
    "ITEM_PRICE"            NUMERIC,
    "BASE_DOSE"             NUMERIC,
    "DOSE_UNIT"             VARCHAR(50),
    "MIN_UNIT"              VARCHAR(50),
    "DRUG_FLAG"             VARCHAR(10),
    "TOT_COST"              NUMERIC,
    "TOT_UNIT"              VARCHAR(50),
    "DRUG_QUALITY"          VARCHAR(50),
    "ISCALU_FLAG"           VARCHAR(10),
    PRIMARY KEY ("GROUP_ID", "SEQ_ORDER")
);

-- 9. 过敏记录表
CREATE TABLE IF NOT EXISTS "FIN_IPR_ALLERGY" (
    "PATIENT_NO"        VARCHAR(50) NOT NULL,
    "DRUG_CODE"         VARCHAR(50) NOT NULL,
    "DRUG_NAME"         VARCHAR(200) NOT NULL,
    "SKIN_RESULT"       VARCHAR(10) NOT NULL,
    "HOS_UNIT_CODE"     VARCHAR(50),
    "UPDATE_CODE"       VARCHAR(50),
    "UPDATE_DATE"       TIMESTAMP,
    "CREATE_CODE"       VARCHAR(50),
    "CREATE_DATE"       TIMESTAMP,
    PRIMARY KEY ("PATIENT_NO", "DRUG_CODE")
);

-- ========== 权限管理表 ==========

-- 10. 用户账号表
CREATE TABLE IF NOT EXISTS "SYS_USER" (
    "USER_ID"       VARCHAR(50) NOT NULL,
    "USER_NAME"     VARCHAR(100) NOT NULL,
    "PASSWORD"      VARCHAR(200) NOT NULL,
    "REAL_NAME"     VARCHAR(100),
    "PHONE"         VARCHAR(20),
    "EMAIL"         VARCHAR(100),
    "DEPT_CODE"     VARCHAR(50),
    "STATUS"        VARCHAR(10) DEFAULT '1',
    "REMARK"        VARCHAR(500),
    "HOS_UNIT_CODE" VARCHAR(50),
    "UPDATE_CODE"   VARCHAR(50),
    "UPDATE_DATE"   TIMESTAMP,
    "CREATE_CODE"   VARCHAR(50),
    "CREATE_DATE"   TIMESTAMP,
    PRIMARY KEY ("USER_ID")
);

-- 11. 角色表
CREATE TABLE IF NOT EXISTS "SYS_ROLE" (
    "ROLE_ID"       VARCHAR(50) NOT NULL,
    "ROLE_CODE"     VARCHAR(50) NOT NULL,
    "ROLE_NAME"     VARCHAR(100) NOT NULL,
    "DESCRIPTION"   VARCHAR(500),
    "STATUS"        VARCHAR(10) DEFAULT '1',
    "SORT_NO"       INTEGER,
    "HOS_UNIT_CODE" VARCHAR(50),
    "UPDATE_CODE"   VARCHAR(50),
    "UPDATE_DATE"   TIMESTAMP,
    "CREATE_CODE"   VARCHAR(50),
    "CREATE_DATE"   TIMESTAMP,
    PRIMARY KEY ("ROLE_ID")
);

-- 12. 菜单表
CREATE TABLE IF NOT EXISTS "SYS_MENU" (
    "MENU_ID"       VARCHAR(50) NOT NULL,
    "MENU_NAME"     VARCHAR(100) NOT NULL,
    "PARENT_ID"     VARCHAR(50) DEFAULT '0',
    "PATH"          VARCHAR(200),
    "COMPONENT"     VARCHAR(200),
    "ICON"          VARCHAR(100),
    "MENU_TYPE"     VARCHAR(10) DEFAULT 'C',
    "PERMS"         VARCHAR(200),
    "SORT_NO"       INTEGER,
    "STATUS"        VARCHAR(10) DEFAULT '1',
    "HOS_UNIT_CODE" VARCHAR(50),
    "UPDATE_CODE"   VARCHAR(50),
    "UPDATE_DATE"   TIMESTAMP,
    "CREATE_CODE"   VARCHAR(50),
    "CREATE_DATE"   TIMESTAMP,
    PRIMARY KEY ("MENU_ID")
);

-- 13. 权限表
CREATE TABLE IF NOT EXISTS "SYS_PERMISSION" (
    "PERMISSION_ID"     VARCHAR(50) NOT NULL,
    "PERMISSION_CODE"   VARCHAR(100) NOT NULL,
    "PERMISSION_NAME"   VARCHAR(100) NOT NULL,
    "PERMISSION_TYPE"   VARCHAR(20),
    "DESCRIPTION"       VARCHAR(500),
    "STATUS"            VARCHAR(10) DEFAULT '1',
    "HOS_UNIT_CODE"     VARCHAR(50),
    "UPDATE_CODE"       VARCHAR(50),
    "UPDATE_DATE"       TIMESTAMP,
    "CREATE_CODE"       VARCHAR(50),
    "CREATE_DATE"       TIMESTAMP,
    PRIMARY KEY ("PERMISSION_ID")
);

-- 14. 用户角色关联表
CREATE TABLE IF NOT EXISTS "SYS_USER_ROLE" (
    "USER_ID"       VARCHAR(50) NOT NULL,
    "ROLE_ID"       VARCHAR(50) NOT NULL,
    "HOS_UNIT_CODE" VARCHAR(50),
    "UPDATE_CODE"   VARCHAR(50),
    "UPDATE_DATE"   TIMESTAMP,
    "CREATE_CODE"   VARCHAR(50),
    "CREATE_DATE"   TIMESTAMP,
    PRIMARY KEY ("USER_ID", "ROLE_ID")
);

-- 15. 角色菜单关联表
CREATE TABLE IF NOT EXISTS "SYS_ROLE_MENU" (
    "ROLE_ID"       VARCHAR(50) NOT NULL,
    "MENU_ID"       VARCHAR(50) NOT NULL,
    "HOS_UNIT_CODE" VARCHAR(50),
    "UPDATE_CODE"   VARCHAR(50),
    "UPDATE_DATE"   TIMESTAMP,
    "CREATE_CODE"   VARCHAR(50),
    "CREATE_DATE"   TIMESTAMP,
    PRIMARY KEY ("ROLE_ID", "MENU_ID")
);

-- 序列（供 GetSequenceValueAsync 使用）
CREATE SEQUENCE IF NOT EXISTS seq_consultation_applyno START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_com_diag_templatecode START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_met_com_diagnosis START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE IF NOT EXISTS seq_order_groupid START WITH 1 INCREMENT BY 1;

-- 服务配置表（微服务地址管理）
CREATE TABLE IF NOT EXISTS "SYS_SERVICE_CONFIG" (
    "CONFIG_ID"     VARCHAR(64) PRIMARY KEY,
    "SERVICE_NAME"  VARCHAR(100) NOT NULL,
    "SERVICE_URL"   VARCHAR(500) NOT NULL,
    "DESCRIPTION"   VARCHAR(500),
    "STATUS"        VARCHAR(10) DEFAULT '1',
    "SORT_NO"       INT DEFAULT 0,
    "CREATE_CODE"   VARCHAR(64),
    "CREATE_DATE"   TIMESTAMP,
    "UPDATE_CODE"   VARCHAR(64),
    "UPDATE_DATE"   TIMESTAMP,
    "HOS_UNIT_CODE" VARCHAR(64)
);

-- 预置服务配置
INSERT INTO "SYS_SERVICE_CONFIG" ("CONFIG_ID", "SERVICE_NAME", "SERVICE_URL", "DESCRIPTION", "STATUS", "SORT_NO", "CREATE_CODE", "CREATE_DATE", "UPDATE_CODE", "UPDATE_DATE")
VALUES
    ('svc_gateway',    'GatewayService',    'http://localhost:5000', 'API 网关',        '1', 1, 'system', NOW(), 'system', NOW()),
    ('svc_permission', 'PermissionService', 'http://localhost:5001', '权限认证服务',    '1', 2, 'system', NOW(), 'system', NOW()),
    ('svc_message',    'MessageService',    'http://localhost:5002', '消息服务',        '1', 3, 'system', NOW(), 'system', NOW()),
    ('svc_agent',      'AgentService',      'http://localhost:5003', 'AI 智能助手服务', '1', 4, 'system', NOW(), 'system', NOW()),
    ('svc_platform',   'PlatformService',   'http://localhost:5004', '平台公共服务',    '1', 5, 'system', NOW(), 'system', NOW()),
    ('svc_shop',       'ShopService',       'http://localhost:5005', 'Shop Service',    '1', 6, 'system', NOW(), 'system', NOW())
ON CONFLICT ("CONFIG_ID") DO NOTHING;
