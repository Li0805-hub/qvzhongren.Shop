<template>
  <view class="order-list-page">
    <!-- Tabs -->
    <view class="tabs">
      <view
        class="tab-item"
        :class="{ active: currentTab === idx }"
        v-for="(tab, idx) in tabs"
        :key="idx"
        @click="switchTab(idx)"
      >
        <text>{{ tab.label }}</text>
      </view>
    </view>

    <!-- Order List -->
    <view class="order-list">
      <view class="order-card" v-for="order in orderList" :key="order.orderId" @click="goDetail(order.orderId)">
        <view class="order-header">
          <text class="order-no">订单号：{{ order.orderNo }}</text>
          <text class="order-status" :class="'status-' + order.status">{{ statusText(order.status) }}</text>
        </view>
        <view class="order-items">
          <view class="order-item" v-for="(item, idx) in (order.items || []).slice(0, 3)" :key="idx">
            <image class="item-img" :src="item.productImage || '/static/logo.png'" mode="aspectFill" />
            <view class="item-info">
              <text class="item-name">{{ item.productName }}</text>
              <text class="item-qty">x{{ item.quantity }}</text>
            </view>
            <text class="item-price">¥{{ item.price }}</text>
          </view>
        </view>
        <view class="order-footer">
          <text class="order-total">共{{ (order.items || []).length }}件 合计：<text class="price-val">¥{{ order.totalAmount }}</text></text>
          <view class="order-actions">
            <button class="action-btn" v-if="order.status === '0'" @click.stop="handleCancel(order.orderId)">取消订单</button>
            <button class="action-btn pay" v-if="order.status === '0'" @click.stop="goPay(order.orderId)">去付款</button>
            <button class="action-btn primary" v-if="order.status === '2'" @click.stop="handleConfirm(order.orderId)">确认收货</button>
            <button class="action-btn primary" v-if="order.status === '3'" @click.stop="goReview(order)">评价</button>
          </view>
        </view>
      </view>
    </view>

    <!-- Empty -->
    <view class="empty-tip" v-if="!loading && orderList.length === 0">
      <text>暂无订单</text>
    </view>

    <view class="loading-tip" v-if="loading">
      <text>加载中...</text>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad, onShow } from '@dcloudio/uni-app';
import { getOrderPage, cancelOrder, updateOrderStatus } from '../../utils/api';

const tabs = [
  { label: '全部', status: -1 },
  { label: '待付款', status: 0 },
  { label: '待发货', status: 1 },
  { label: '待收货', status: 2 },
  { label: '已完成', status: 3 },
];

const currentTab = ref(0);
const orderList = ref<any[]>([]);
const loading = ref(false);
const pageIndex = ref(1);

const statusText = (status: string) => {
  const map: Record<string, string> = { '0': '待付款', '1': '待发货', '2': '待收货', '3': '已完成', '-1': '已取消' };
  return map[status] || '未知';
};

const switchTab = (idx: number) => {
  currentTab.value = idx;
  orderList.value = [];
  pageIndex.value = 1;
  loadOrders();
};

const loadOrders = async () => {
  loading.value = true;
  try {
    const params: any = { pageIndex: pageIndex.value, pageSize: 10 };
    if (tabs[currentTab.value].status >= 0) {
      params.status = tabs[currentTab.value].status;
    }
    const res = await getOrderPage(params);
    orderList.value = res.data?.values || [];
  } catch (e) {} finally {
    loading.value = false;
  }
};

const goDetail = (orderId: string) => {
  uni.navigateTo({ url: `/pages/order/detail?id=${orderId}` });
};

const goPay = (orderId: string) => {
  uni.navigateTo({ url: `/pages/order/detail?id=${orderId}` });
};

const handleCancel = async (id: string) => {
  uni.showModal({
    title: '提示',
    content: '确定取消订单？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await cancelOrder(id);
          uni.showToast({ title: '已取消', icon: 'success' });
          loadOrders();
        } catch (e) {}
      }
    },
  });
};

const handleConfirm = (orderId: string) => {
  uni.showModal({
    title: '提示',
    content: '确认收货？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await updateOrderStatus({ orderId, status: '3' });
          uni.showToast({ title: '已确认收货', icon: 'success' });
          loadOrders();
        } catch (e) {}
      }
    },
  });
};

const goReview = (order: any) => {
  const firstItem = (order.items || [])[0];
  if (firstItem) {
    uni.navigateTo({
      url: `/pages/review/create?orderId=${order.orderId}&productId=${firstItem.productId}`,
    });
  }
};

onLoad((query: any) => {
  if (query.tab !== undefined) {
    const status = parseInt(query.tab);
    // 根据状态值找到 tabs 数组中的索引
    const idx = tabs.findIndex(t => t.status === status);
    currentTab.value = idx >= 0 ? idx : 0;
  }
});

onShow(() => {
  loadOrders();
});
</script>

<style scoped>
.order-list-page {
  min-height: 100vh;
  background: #F5F5F5;
}

.tabs {
  display: flex;
  background: #fff;
  position: sticky;
  top: 0;
  z-index: 10;
}

.tab-item {
  flex: 1;
  text-align: center;
  padding: 24rpx 0;
  font-size: 28rpx;
  color: #666;
  position: relative;
}

.tab-item.active {
  color: #6C5CE7;
  font-weight: bold;
}

.tab-item.active::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 48rpx;
  height: 4rpx;
  background: #6C5CE7;
  border-radius: 2rpx;
}

.order-list {
  padding: 20rpx;
}

.order-card {
  background: #fff;
  border-radius: 16rpx;
  padding: 20rpx;
  margin-bottom: 20rpx;
}

.order-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 16rpx;
  border-bottom: 1rpx solid #f0f0f0;
}

.order-no {
  font-size: 24rpx;
  color: #999;
}

.order-status {
  font-size: 26rpx;
  font-weight: bold;
}

.status-0 { color: #E17055; }
.status-1 { color: #6C5CE7; }
.status-2 { color: #00b894; }
.status-3 { color: #999; }
.status-4 { color: #ccc; }

.order-items {
  padding: 12rpx 0;
}

.order-item {
  display: flex;
  align-items: center;
  padding: 8rpx 0;
}

.item-img {
  width: 100rpx;
  height: 100rpx;
  border-radius: 8rpx;
  margin-right: 16rpx;
  flex-shrink: 0;
}

.item-info {
  flex: 1;
}

.item-name {
  font-size: 26rpx;
  color: #333;
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.item-qty {
  font-size: 24rpx;
  color: #999;
  margin-top: 4rpx;
  display: block;
}

.item-price {
  font-size: 26rpx;
  color: #333;
  flex-shrink: 0;
}

.order-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 16rpx;
  border-top: 1rpx solid #f0f0f0;
}

.order-total {
  font-size: 26rpx;
  color: #666;
}

.price-val {
  color: #E17055;
  font-weight: bold;
}

.order-actions {
  display: flex;
  gap: 12rpx;
}

.action-btn {
  height: 56rpx;
  line-height: 56rpx;
  padding: 0 24rpx;
  font-size: 24rpx;
  border-radius: 28rpx;
  background: #fff;
  color: #666;
  border: 1rpx solid #ddd;
}

.action-btn.primary {
  background: #6C5CE7;
  color: #fff;
  border-color: #6C5CE7;
}

.action-btn.pay {
  background: #E17055;
  color: #fff;
  border-color: #E17055;
}

.empty-tip,
.loading-tip {
  text-align: center;
  padding: 80rpx 0;
  font-size: 28rpx;
  color: #999;
}
</style>
