# 曲终人商城系统

基于 .NET 8 微服务架构的全栈商城系统，包含后端 API、管理后台、UniApp 小程序三端。

## 项目结构

```
├── QvzhongrenApiC#/                  # 后端（.NET 8 + ABP Framework）
│   ├── Permission/                   # 权限模块（用户/角色/菜单/权限/部门/认证）
│   ├── Message/                      # 消息模块（实时聊天 + SignalR）
│   ├── Agent/                        # AI 智能助手模块（DeepSeek 对接）
│   ├── Platform/                     # 平台模块（日志/文件上传/服务配置）
│   ├── Shop/                         # 商城模块（商品/分类/购物车/订单/地址/评价）
│   ├── Gateway/                      # YARP API 网关
│   ├── qvzhongren.Application/       # 基础服务层（BaseService、CrudService）
│   ├── qvzhongren.Model/             # 基础实体层
│   ├── qvzhongren.Repository/        # 数据访问层（SqlSugar）
│   ├── qvzhongren.Shared/            # 工具类
│   ├── qvzhongren.Contracts/         # 跨服务契约（DTO + 接口）
│   ├── qvzhongren.Web/               # 单体模式启动入口
│   ├── docker-compose.yml            # Docker 部署（基础版）
│   ├── docker-compose.full.yml       # Docker 部署（含 PostgreSQL）
│   └── init.sql                      # 数据库初始化脚本
│
├── qvzhongren-web/                   # 管理后台（React + TypeScript + Ant Design）
│
└── qvzhongren-miniapp/               # 小程序/H5（UniApp + Vue3 + TypeScript）
```

## 技术栈

| 层 | 技术 |
|---|---|
| 后端框架 | .NET 8 + ABP Framework + Autofac |
| ORM | SqlSugar |
| 数据库 | PostgreSQL |
| 认证 | JWT |
| 实时通信 | SignalR |
| AI | DeepSeek API（可在管理后台动态配置 Key） |
| API 网关 | YARP（Yet Another Reverse Proxy） |
| 管理后台 | React 18 + TypeScript + Ant Design 5 + Vite |
| 小程序 | UniApp + Vue 3 + TypeScript（支持微信小程序 / H5） |
| 容器化 | Docker + Docker Compose |

## 微服务架构

```
                    ┌──────────────┐
                    │  API Gateway │  端口 5000
                    │   (YARP)     │
                    └──┬───┬───┬──┬──┐
                       │   │   │  │  │
          ┌────────────┘   │   │  │  └────────────┐
          ▼                ▼   ▼  ▼               ▼
    ┌──────────┐   ┌─────────┐ ┌─────┐ ┌────────┐ ┌──────┐
    │Permission│   │ Message │ │Agent│ │Platform│ │ Shop │
    │  :5001   │   │  :5002  │ │:5003│ │ :5004  │ │:5005 │
    └──────────┘   └─────────┘ └─────┘ └────────┘ └──────┘
          │              │        │        │          │
          └──────────────┴────────┴────────┴──────────┘
                              │
                        ┌─────┴─────┐
                        │ PostgreSQL│
                        └───────────┘
```

支持两种部署模式：
- **单体模式**：`dotnet run --project qvzhongren.Web`（一个进程）
- **分布式模式**：分别启动 Gateway + 5 个服务（6 个进程）

## 功能模块

### 权限管理
- 用户管理（CRUD、角色分配、菜单分配、密码重置）
- 角色管理（CRUD、菜单权限分配）
- 菜单管理（树形结构、目录/菜单/按钮类型）
- 权限管理
- 部门管理（树形结构）
- JWT 认证（登录、Token 生成/验证）

### 系统管理
- 系统日志（分页查询、按时间/类型/关键字筛选、批量删除）
- 服务配置（数据库存储各微服务地址、AI Key 配置）
- 文件上传

### 消息系统
- 实时聊天（SignalR WebSocket）
- 会话管理、消息已读/未读
- 在线状态

### AI 智能助手
- DeepSeek 大模型对接
- 工具调用（查询用户/角色/菜单/日志/统计）
- 页面导航
- API Key 从数据库动态读取，管理后台可修改

### 商城系统
- **商品管理**：分类、商品 CRUD、图片、库存、价格
- **购物车**：添加、数量编辑、删除、全选、结算
- **订单**：下单、沙盒支付、发货、确认收货、取消
- **收货地址**：CRUD、默认地址
- **评价**：星级评分、文字评价
- **搜索**：关键字搜索、搜索历史

## 快速开始

### 环境要求

- .NET 8 SDK
- Node.js 18+
- PostgreSQL 14+

### 1. 数据库初始化

```bash
# 创建数据库
psql -U postgres -c "CREATE DATABASE xiaoli;"

# 执行初始化脚本
psql -U postgres -d xiaoli -f QvzhongrenApiC#/init.sql
```

### 2. 启动后端

```bash
cd QvzhongrenApiC#
dotnet run --project qvzhongren.Web
# 访问 http://localhost:8096/swagger
```

### 3. 启动管理后台

```bash
cd qvzhongren-web
npm install
npm run dev
# 访问 http://localhost:5173
```

### 4. 启动小程序（H5 模式）

```bash
cd qvzhongren-miniapp
npm install
npm run dev:h5
# 访问 http://localhost:5174
```

### 5. 启动小程序（微信小程序）

```bash
cd qvzhongren-miniapp
npm run dev:mp-weixin
# 用微信开发者工具打开 dist/dev/mp-weixin 目录
```

### 默认账号

| 用户名 | 密码 | 说明 |
|--------|------|------|
| admin | admin123 | 管理员（首次需调用 `/api/Auth/InitAdmin` 初始化） |

## Docker 部署

### 基础版（数据库在宿主机）

```bash
cd QvzhongrenApiC#
docker compose up -d --build
# Gateway: http://localhost:5000
```

### 完整版（含 PostgreSQL）

```bash
cd QvzhongrenApiC#
docker compose -f docker-compose.full.yml up -d --build
```

## 分布式部署

```bash
cd QvzhongrenApiC#

# 启动各服务
dotnet run --project Gateway/qvzhongren.Gateway --urls http://localhost:5000
dotnet run --project Permission/qvzhongren.Permission.Host --urls http://localhost:5001
dotnet run --project Message/qvzhongren.Message.Host --urls http://localhost:5002
dotnet run --project Agent/qvzhongren.Agent.Host --urls http://localhost:5003
dotnet run --project Platform/qvzhongren.Platform.Host --urls http://localhost:5004
dotnet run --project Shop/qvzhongren.Shop.Host --urls http://localhost:5005
```

各服务地址可在管理后台「系统管理 → 服务配置」页面修改，存储在数据库中。

## 项目截图

### 管理后台
- 权限管理：用户/角色/菜单/部门
- 系统管理：日志/服务配置
- 商城管理：商品/分类/订单

### 小程序
- 首页：分类导航 + 商品列表
- 分类：左侧分类 + 右侧商品
- 购物车：选购结算
- 我的：订单管理/地址管理
- 沙盒支付

## License

MIT
