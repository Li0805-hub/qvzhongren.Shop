import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, InputNumber, Select, TreeSelect, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { getCategoryTree, getCategoryList, createCategory, updateCategory, deleteCategory } from '../../api/shop';
import type { CategoryDto, CategoryCreateDto } from '../../api/shop';

const CategoryList: React.FC = () => {
  const [data, setData] = useState<CategoryDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<CategoryDto | null>(null);
  const [form] = Form.useForm();

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await getCategoryTree();
      setData(res.data.data ?? []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const convertToTreeData = (list: CategoryDto[]): any[] => [
    { value: '0', title: '顶级分类' },
    ...list.map((c) => ({
      value: c.categoryId,
      title: c.categoryName,
      children: c.children ? convertToTreeData(c.children).slice(1) : undefined,
    })),
  ];

  const buildTreeSelectData = (list: CategoryDto[]): any[] =>
    list.map((c) => ({
      value: c.categoryId,
      title: c.categoryName,
      children: c.children ? buildTreeSelectData(c.children) : undefined,
    }));

  const handleAdd = () => {
    setEditing(null);
    form.resetFields();
    form.setFieldsValue({ parentId: '0', status: '1', sortNo: 0 });
    setModalOpen(true);
  };

  const handleEdit = (record: CategoryDto) => {
    setEditing(record);
    form.setFieldsValue({
      parentId: record.parentId || '0',
      categoryName: record.categoryName,
      icon: record.icon,
      sortNo: record.sortNo,
      status: record.status,
    });
    setModalOpen(true);
  };

  const hasChildren = (categoryId: string, list: CategoryDto[]): boolean => {
    for (const item of list) {
      if (item.categoryId === categoryId && item.children && item.children.length > 0) return true;
      if (item.children && hasChildren(categoryId, item.children)) return true;
    }
    return false;
  };

  const handleDelete = (record: CategoryDto) => {
    if (hasChildren(record.categoryId, data)) {
      message.error('该分类下存在子分类，请先删除子分类');
      return;
    }
    Modal.confirm({
      title: '确认删除该分类？',
      content: `将删除分类「${record.categoryName}」`,
      onOk: async () => {
        await deleteCategory(record.categoryId);
        message.success('删除成功');
        fetchData();
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: CategoryCreateDto = { ...values };
    if (editing) {
      dto.categoryId = editing.categoryId;
      await updateCategory(dto);
      message.success('更新成功');
    } else {
      dto.categoryId = crypto.randomUUID();
      await createCategory(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData();
  };

  const columns = [
    { title: '分类名称', dataIndex: 'categoryName', key: 'categoryName' },
    {
      title: '图标', dataIndex: 'icon', key: 'icon', width: 200,
      render: (icon: string) => icon ? <img src={icon} alt="icon" style={{ width: 24, height: 24 }} /> : '-',
    },
    { title: '排序', dataIndex: 'sortNo', key: 'sortNo', width: 80 },
    {
      title: '状态', dataIndex: 'status', key: 'status', width: 80,
      render: (s: string) => <Tag color={s === '1' ? 'green' : 'red'}>{s === '1' ? '启用' : '禁用'}</Tag>,
    },
    {
      title: '操作', key: 'action', width: 180,
      render: (_: unknown, record: CategoryDto) => (
        <Space>
          <Button type="link" size="small" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" size="small" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 12 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>新增分类</Button>
      </div>
      <Table
        columns={columns}
        dataSource={data}
        rowKey="categoryId"
        loading={loading}
        pagination={false}
        expandable={{ childrenColumnName: 'children' }}
        size="small"
      />

      <Modal title={editing ? '编辑分类' : '新增分类'} open={modalOpen} onOk={handleSubmit} onCancel={() => setModalOpen(false)} destroyOnClose>
        <Form form={form} layout="vertical">
          <Form.Item name="parentId" label="上级分类">
            <TreeSelect
              treeData={[{ value: '0', title: '顶级分类', children: buildTreeSelectData(data) }]}
              placeholder="请选择上级分类"
              treeDefaultExpandAll
            />
          </Form.Item>
          <Form.Item name="categoryName" label="分类名称" rules={[{ required: true, message: '请输入分类名称' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="icon" label="图标URL">
            <Input placeholder="图标图片地址" />
          </Form.Item>
          <Form.Item name="sortNo" label="排序">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="status" label="状态" rules={[{ required: true }]}>
            <Select options={[{ value: '1', label: '启用' }, { value: '0', label: '禁用' }]} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default CategoryList;
