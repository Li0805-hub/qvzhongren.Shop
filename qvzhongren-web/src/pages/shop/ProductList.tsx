import React, { useEffect, useState, useCallback } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, InputNumber, Select, Card, Image, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined, ReloadOutlined } from '@ant-design/icons';
import { getProductPage, createProduct, updateProduct, deleteProduct, getCategoryList } from '../../api/shop';
import type { ProductDto, ProductCreateDto, ProductQueryDto, CategoryDto } from '../../api/shop';

const ProductList: React.FC = () => {
  const [data, setData] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ProductDto | null>(null);
  const [form] = Form.useForm();
  const [categoryList, setCategoryList] = useState<CategoryDto[]>([]);
  const [categoryMap, setCategoryMap] = useState<Record<string, string>>({});
  const [query, setQuery] = useState<ProductQueryDto>({
    pageIndex: 1,
    pageSize: 10,
    categoryId: undefined,
    keyword: undefined,
    status: undefined,
  });

  useEffect(() => {
    getCategoryList().then((res) => {
      const list = res.data.data ?? [];
      setCategoryList(list);
      const map: Record<string, string> = {};
      list.forEach((c) => { map[c.categoryId] = c.categoryName; });
      setCategoryMap(map);
    });
  }, []);

  const fetchData = useCallback(async (q: ProductQueryDto) => {
    setLoading(true);
    try {
      const res = await getProductPage(q);
      const page = res.data.data;
      setData(page?.values ?? []);
      setTotal(page?.totalCount ?? 0);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchData(query); }, [query, fetchData]);

  const handleSearch = () => {
    setQuery((p) => ({ ...p, pageIndex: 1 }));
  };

  const handleReset = () => {
    setQuery({ pageIndex: 1, pageSize: 10, categoryId: undefined, keyword: undefined, status: undefined });
  };

  const handleAdd = () => {
    setEditing(null);
    form.resetFields();
    form.setFieldsValue({ status: '1', sortNo: 0, stock: 0, price: 0 });
    setModalOpen(true);
  };

  const handleEdit = (record: ProductDto) => {
    setEditing(record);
    form.setFieldsValue({
      productName: record.productName,
      categoryId: record.categoryId,
      price: record.price,
      originalPrice: record.originalPrice,
      stock: record.stock,
      mainImage: record.mainImage,
      images: record.images,
      description: record.description,
      status: record.status,
      sortNo: record.sortNo,
    });
    setModalOpen(true);
  };

  const handleDelete = (record: ProductDto) => {
    Modal.confirm({
      title: '确认删除该商品？',
      content: `将删除商品「${record.productName}」`,
      onOk: async () => {
        await deleteProduct(record.productId);
        message.success('删除成功');
        fetchData(query);
      },
    });
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const dto: ProductCreateDto = { ...values };
    if (editing) {
      dto.productId = editing.productId;
      await updateProduct(dto);
      message.success('更新成功');
    } else {
      dto.productId = crypto.randomUUID();
      await createProduct(dto);
      message.success('创建成功');
    }
    setModalOpen(false);
    fetchData(query);
  };

  const columns = [
    {
      title: '商品图片', dataIndex: 'mainImage', key: 'mainImage', width: 80,
      render: (url: string) => (
        <Image
          src={url}
          width={60}
          height={60}
          style={{ objectFit: 'cover', borderRadius: 4 }}
          fallback="data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHJlY3Qgd2lkdGg9IjYwIiBoZWlnaHQ9IjYwIiBmaWxsPSIjZjBmMGYwIi8+PHRleHQgeD0iNTAlIiB5PSI1MCUiIGRvbWluYW50LWJhc2VsaW5lPSJtaWRkbGUiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGZpbGw9IiNjY2MiIGZvbnQtc2l6ZT0iMTIiPuaXoOWbvjwvdGV4dD48L3N2Zz4="
        />
      ),
    },
    { title: '商品名称', dataIndex: 'productName', key: 'productName', ellipsis: true },
    {
      title: '分类', dataIndex: 'categoryId', key: 'categoryId', width: 120,
      render: (id: string) => categoryMap[id] || id || '-',
    },
    {
      title: '价格', dataIndex: 'price', key: 'price', width: 100,
      render: (v: number) => <span style={{ color: '#E17055', fontWeight: 500 }}>&yen;{v.toFixed(2)}</span>,
    },
    {
      title: '原价', dataIndex: 'originalPrice', key: 'originalPrice', width: 100,
      render: (v: number | undefined) => v ? <span style={{ color: '#999', textDecoration: 'line-through' }}>&yen;{v.toFixed(2)}</span> : '-',
    },
    { title: '库存', dataIndex: 'stock', key: 'stock', width: 80 },
    { title: '销量', dataIndex: 'sales', key: 'sales', width: 80 },
    {
      title: '状态', dataIndex: 'status', key: 'status', width: 80,
      render: (s: string) => <Tag color={s === '1' ? 'green' : 'red'}>{s === '1' ? '上架' : '下架'}</Tag>,
    },
    {
      title: '操作', key: 'action', width: 160,
      render: (_: unknown, record: ProductDto) => (
        <Space>
          <Button type="link" size="small" icon={<EditOutlined />} onClick={() => handleEdit(record)}>编辑</Button>
          <Button type="link" size="small" danger icon={<DeleteOutlined />} onClick={() => handleDelete(record)}>删除</Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <Card style={{ borderRadius: 16, border: 'none', marginBottom: 12 }} styles={{ body: { padding: '12px 20px' } }}>
        <Space wrap size={10}>
          <Select
            value={query.categoryId}
            onChange={(val) => setQuery((p) => ({ ...p, categoryId: val || undefined, pageIndex: 1 }))}
            placeholder="全部分类"
            allowClear
            style={{ width: 160 }}
            options={categoryList.map((c) => ({ value: c.categoryId, label: c.categoryName }))}
          />
          <Input
            placeholder="搜索商品名称"
            value={query.keyword}
            onChange={(e) => setQuery((p) => ({ ...p, keyword: e.target.value || undefined }))}
            onPressEnter={handleSearch}
            style={{ width: 200 }}
            prefix={<SearchOutlined style={{ color: '#bbb' }} />}
            allowClear
          />
          <Select
            value={query.status}
            onChange={(val) => setQuery((p) => ({ ...p, status: val || undefined, pageIndex: 1 }))}
            placeholder="全部状态"
            allowClear
            style={{ width: 120 }}
            options={[
              { value: '1', label: '上架' },
              { value: '0', label: '下架' },
            ]}
          />
          <Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>查询</Button>
          <Button icon={<ReloadOutlined />} onClick={handleReset}>重置</Button>
        </Space>
      </Card>

      <div style={{ marginBottom: 12 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>新增商品</Button>
      </div>

      <Table
        columns={columns}
        dataSource={data}
        rowKey="productId"
        loading={loading}
        size="small"
        pagination={{
          current: query.pageIndex,
          pageSize: query.pageSize,
          total,
          showSizeChanger: true,
          showQuickJumper: true,
          showTotal: (t) => `共 ${t} 条`,
          size: 'small',
          onChange: (page, size) => setQuery((p) => ({ ...p, pageIndex: page, pageSize: size })),
        }}
      />

      <Modal title={editing ? '编辑商品' : '新增商品'} open={modalOpen} onOk={handleSubmit} onCancel={() => setModalOpen(false)} destroyOnClose width={640}>
        <Form form={form} layout="vertical">
          <Form.Item name="productName" label="商品名称" rules={[{ required: true, message: '请输入商品名称' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="categoryId" label="分类" rules={[{ required: true, message: '请选择分类' }]}>
            <Select
              placeholder="请选择分类"
              options={categoryList.map((c) => ({ value: c.categoryId, label: c.categoryName }))}
            />
          </Form.Item>
          <Form.Item name="price" label="价格" rules={[{ required: true, message: '请输入价格' }]}>
            <InputNumber min={0} step={0.01} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="originalPrice" label="原价">
            <InputNumber min={0} step={0.01} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="stock" label="库存" rules={[{ required: true, message: '请输入库存' }]}>
            <InputNumber min={0} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="mainImage" label="主图URL">
            <Input placeholder="商品主图地址" />
          </Form.Item>
          <Form.Item name="images" label="图片列表">
            <Input.TextArea rows={3} placeholder='JSON格式，如 ["url1","url2"]' />
          </Form.Item>
          <Form.Item name="description" label="描述">
            <Input.TextArea rows={4} />
          </Form.Item>
          <Form.Item name="status" label="状态" rules={[{ required: true }]}>
            <Select options={[{ value: '1', label: '上架' }, { value: '0', label: '下架' }]} />
          </Form.Item>
          <Form.Item name="sortNo" label="排序">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default ProductList;
