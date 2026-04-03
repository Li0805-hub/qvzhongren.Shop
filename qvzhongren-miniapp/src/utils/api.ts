import request from './request';

// Categories
export const getCategoryList = () => request<any[]>('/Category/GetList');
export const getCategoryTree = () => request<any[]>('/Category/GetTree');

// Products
export const getProductPage = (data: any) => request<any>('/Product/GetPage', data);
export const getProductById = (id: string) => request<any>('/Product/GetById', id);

// Cart
export const getCartList = (userId: string) => request<any[]>('/Cart/GetList', userId);
export const addToCart = (data: { productId: string; quantity: number }) => request<boolean>('/Cart/Add', data);
export const updateCartQuantity = (data: { cartId: string; quantity: number }) => request<boolean>('/Cart/UpdateQuantity', data);
export const removeCart = (cartId: string) => request<boolean>('/Cart/Remove', cartId);
export const clearCart = (userId: string) => request<boolean>('/Cart/Clear', userId);

// Orders
export const createOrder = (data: any) => request<any>('/Order/Create', data);
export const getOrderPage = (data: any) => request<any>('/Order/GetPage', data);
export const getOrderById = (id: string) => request<any>('/Order/GetById', id);
export const cancelOrder = (id: string) => request<boolean>('/Order/Cancel', id);
export const payOrder = (id: string) => request<boolean>('/Order/Pay', id);
export const updateOrderStatus = (data: { orderId: string; status: string }) => request<boolean>('/Order/UpdateStatus', data);

// Address
export const getAddressList = (userId: string) => request<any[]>('/Address/GetList', userId);
export const createAddress = (data: any) => request<boolean>('/Address/Create', data);
export const updateAddress = (data: any) => request<boolean>('/Address/Update', data);
export const deleteAddress = (id: string) => request<boolean>('/Address/Delete', id);
export const setDefaultAddress = (id: string) => request<boolean>('/Address/SetDefault', id);

// Reviews
export const createReview = (data: any) => request<boolean>('/Review/Create', data);
export const getReviewsByProduct = (productId: string) => request<any[]>('/Review/GetByProduct', productId);

// Auth
export const login = (data: { userName: string; password: string }) => request<any>('/Auth/Login', data);
