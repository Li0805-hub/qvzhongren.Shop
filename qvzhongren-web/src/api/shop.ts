import request from './request';
import type { ResultDto, ListPageResultDto } from '../types';

// ========== 分类 ==========
export interface CategoryDto {
  categoryId: string;
  parentId?: string;
  categoryName: string;
  icon?: string;
  sortNo?: number;
  status: string;
  createDate?: string;
  children?: CategoryDto[];
}

export interface CategoryCreateDto {
  categoryId?: string;
  parentId?: string;
  categoryName: string;
  icon?: string;
  sortNo?: number;
  status?: string;
}

export function getCategoryList() {
  return request.post<ResultDto<CategoryDto[]>>('/Category/GetList');
}
export function getCategoryTree() {
  return request.post<ResultDto<CategoryDto[]>>('/Category/GetTree');
}
export function createCategory(data: CategoryCreateDto) {
  return request.post<ResultDto<boolean>>('/Category/Create', data);
}
export function updateCategory(data: CategoryCreateDto) {
  return request.post<ResultDto<boolean>>('/Category/Update', data);
}
export function deleteCategory(id: string) {
  return request.post<ResultDto<boolean>>('/Category/Delete', id, { headers: { 'Content-Type': 'application/json' } });
}

// ========== 商品 ==========
export interface ProductDto {
  productId: string;
  categoryId: string;
  productName: string;
  description?: string;
  price: number;
  originalPrice?: number;
  stock: number;
  sales: number;
  mainImage?: string;
  images?: string;
  status: string;
  sortNo?: number;
  createDate?: string;
}

export interface ProductCreateDto {
  productId?: string;
  categoryId: string;
  productName: string;
  description?: string;
  price: number;
  originalPrice?: number;
  stock: number;
  mainImage?: string;
  images?: string;
  status?: string;
  sortNo?: number;
}

export interface ProductQueryDto {
  categoryId?: string;
  keyword?: string;
  status?: string;
  pageIndex: number;
  pageSize: number;
}

export function getProductPage(query: ProductQueryDto) {
  return request.post<ResultDto<ListPageResultDto<ProductDto>>>('/Product/GetPage', query);
}
export function getProductById(id: string) {
  return request.post<ResultDto<ProductDto>>('/Product/GetById', id, { headers: { 'Content-Type': 'application/json' } });
}
export function createProduct(data: ProductCreateDto) {
  return request.post<ResultDto<boolean>>('/Product/Create', data);
}
export function updateProduct(data: ProductCreateDto) {
  return request.post<ResultDto<boolean>>('/Product/Update', data);
}
export function deleteProduct(id: string) {
  return request.post<ResultDto<boolean>>('/Product/Delete', id, { headers: { 'Content-Type': 'application/json' } });
}

// ========== 订单 ==========
export interface OrderItemDto {
  itemId: string;
  orderId: string;
  productId: string;
  productName: string;
  productImage?: string;
  price: number;
  quantity: number;
  subtotal: number;
}

export interface OrderDto {
  orderId: string;
  orderNo: string;
  userId: string;
  totalAmount: number;
  status: string;
  receiverName: string;
  receiverPhone: string;
  receiverAddress: string;
  remark?: string;
  payTime?: string;
  shipTime?: string;
  completeTime?: string;
  createDate?: string;
  items?: OrderItemDto[];
}

export interface OrderQueryDto {
  status?: string;
  userId?: string;
  pageIndex: number;
  pageSize: number;
}

export function getOrderPage(query: OrderQueryDto) {
  return request.post<ResultDto<ListPageResultDto<OrderDto>>>('/Order/GetPage', query);
}
export function getOrderById(id: string) {
  return request.post<ResultDto<OrderDto>>('/Order/GetById', id, { headers: { 'Content-Type': 'application/json' } });
}
export function cancelOrder(id: string) {
  return request.post<ResultDto<boolean>>('/Order/Cancel', id, { headers: { 'Content-Type': 'application/json' } });
}
export function updateOrderStatus(data: { orderId: string; status: string }) {
  return request.post<ResultDto<boolean>>('/Order/UpdateStatus', data);
}
