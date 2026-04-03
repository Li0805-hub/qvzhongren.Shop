<template>
  <view class="my-page">
    <!-- User Header -->
    <view class="user-header">
      <view class="avatar">
        <text class="avatar-text">{{ userName ? userName.charAt(0).toUpperCase() : '?' }}</text>
      </view>
      <view class="user-info" v-if="isLoggedIn">
        <text class="user-name">{{ userName }}</text>
        <text class="user-id">ID: {{ userId }}</text>
      </view>
      <view class="user-info" v-else @click="goLogin">
        <text class="user-name">点击登录</text>
      </view>
    </view>

    <!-- Order Shortcuts -->
    <view class="order-section">
      <view class="section-header">
        <text class="section-title">我的订单</text>
        <text class="view-all" @click="goOrders(-1)">全部订单 &#x276F;</text>
      </view>
      <view class="order-shortcuts">
        <view class="shortcut-item" @click="goOrders(0)">
          <view class="icon-wrap">
            <text class="shortcut-icon">&#x1F4B3;</text>
            <view class="badge" v-if="orderCounts.pending > 0">
              <text class="badge-text">{{ orderCounts.pending }}</text>
            </view>
          </view>
          <text class="shortcut-text">待付款</text>
        </view>
        <view class="shortcut-item" @click="goOrders(1)">
          <view class="icon-wrap">
            <text class="shortcut-icon">&#x1F4E6;</text>
            <view class="badge" v-if="orderCounts.paid > 0">
              <text class="badge-text">{{ orderCounts.paid }}</text>
            </view>
          </view>
          <text class="shortcut-text">待发货</text>
        </view>
        <view class="shortcut-item" @click="goOrders(2)">
          <view class="icon-wrap">
            <text class="shortcut-icon">&#x1F69A;</text>
            <view class="badge" v-if="orderCounts.shipped > 0">
              <text class="badge-text">{{ orderCounts.shipped }}</text>
            </view>
          </view>
          <text class="shortcut-text">待收货</text>
        </view>
        <view class="shortcut-item" @click="goOrders(3)">
          <view class="icon-wrap">
            <text class="shortcut-icon">&#x2705;</text>
          </view>
          <text class="shortcut-text">已完成</text>
        </view>
      </view>
    </view>

    <!-- Menu List -->
    <view class="menu-list">
      <view class="menu-item" @click="goAddress">
        <text class="menu-text">收货地址</text>
        <text class="menu-arrow">&#x276F;</text>
      </view>
      <view class="menu-item" @click="goOrders(-1)">
        <text class="menu-text">全部订单</text>
        <text class="menu-arrow">&#x276F;</text>
      </view>
    </view>

    <!-- Logout -->
    <view class="logout-section" v-if="isLoggedIn">
      <button class="logout-btn" @click="handleLogout">退出登录</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue';
import { onShow } from '@dcloudio/uni-app';
import { getOrderPage } from '../../utils/api';

const userId = ref('');
const userName = ref('');
const isLoggedIn = computed(() => !!userId.value);

const orderCounts = reactive({
  pending: 0,   // 待付款 status=0
  paid: 0,      // 待发货 status=1
  shipped: 0,   // 待收货 status=2
  completed: 0, // 已完成 status=3
});

const refreshUserInfo = () => {
  userId.value = uni.getStorageSync('userId') || '';
  userName.value = uni.getStorageSync('userName') || '';
};

const loadOrderCounts = async () => {
  if (!isLoggedIn.value) return;
  try {
    // 查各状态的订单数
    const statuses = ['0', '1', '2'];
    const keys: (keyof typeof orderCounts)[] = ['pending', 'paid', 'shipped'];
    for (let i = 0; i < statuses.length; i++) {
      try {
        const res = await getOrderPage({ status: statuses[i], pageIndex: 1, pageSize: 1 });
        orderCounts[keys[i]] = res.data?.totalCount || 0;
      } catch { orderCounts[keys[i]] = 0; }
    }
  } catch (e) {}
};

const goLogin = () => {
  uni.navigateTo({ url: '/pages/login/index' });
};

const goOrders = (tab: number) => {
  if (!isLoggedIn.value) {
    goLogin();
    return;
  }
  uni.navigateTo({ url: `/pages/order/list?tab=${tab}` });
};

const goAddress = () => {
  if (!isLoggedIn.value) {
    goLogin();
    return;
  }
  uni.navigateTo({ url: '/pages/address/list' });
};

const handleLogout = () => {
  uni.showModal({
    title: '提示',
    content: '确定退出登录？',
    success: (res) => {
      if (res.confirm) {
        uni.removeStorageSync('token');
        uni.removeStorageSync('userId');
        uni.removeStorageSync('userName');
        refreshUserInfo();
        orderCounts.pending = 0;
        orderCounts.paid = 0;
        orderCounts.shipped = 0;
        orderCounts.completed = 0;
        uni.showToast({ title: '已退出', icon: 'success' });
      }
    },
  });
};

onShow(() => {
  refreshUserInfo();
  loadOrderCounts();
});
</script>

<style scoped>
.my-page {
  min-height: 100vh;
  background: #F5F5F5;
}

.user-header {
  display: flex;
  align-items: center;
  padding: 60rpx 32rpx 40rpx;
  background: linear-gradient(135deg, #6C5CE7 0%, #a29bfe 100%);
}

.avatar {
  width: 120rpx;
  height: 120rpx;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 24rpx;
}

.avatar-text {
  font-size: 48rpx;
  color: #fff;
  font-weight: bold;
}

.user-info { flex: 1; }
.user-name { display: block; font-size: 34rpx; color: #fff; font-weight: bold; }
.user-id { display: block; font-size: 24rpx; color: rgba(255, 255, 255, 0.7); margin-top: 8rpx; }

.order-section {
  background: #fff;
  margin: 20rpx;
  border-radius: 16rpx;
  padding: 24rpx;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24rpx;
}

.section-title { font-size: 30rpx; font-weight: bold; color: #333; }
.view-all { font-size: 26rpx; color: #999; }

.order-shortcuts {
  display: flex;
  justify-content: space-around;
}

.shortcut-item {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.icon-wrap {
  position: relative;
  display: inline-block;
}

.shortcut-icon {
  font-size: 48rpx;
}

.badge {
  position: absolute;
  top: -10rpx;
  right: -16rpx;
  min-width: 32rpx;
  height: 32rpx;
  background: #E17055;
  border-radius: 16rpx;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 8rpx;
}

.badge-text {
  font-size: 20rpx;
  color: #fff;
  font-weight: bold;
}

.shortcut-text {
  font-size: 24rpx;
  color: #666;
  margin-top: 8rpx;
}

.menu-list {
  background: #fff;
  margin: 0 20rpx;
  border-radius: 16rpx;
}

.menu-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 28rpx 24rpx;
  border-bottom: 1rpx solid #f0f0f0;
}

.menu-item:last-child { border-bottom: none; }
.menu-text { font-size: 28rpx; color: #333; }
.menu-arrow { font-size: 24rpx; color: #ccc; }

.logout-section { padding: 40rpx 20rpx; }

.logout-btn {
  width: 100%;
  height: 80rpx;
  line-height: 80rpx;
  background: #fff;
  color: #E17055;
  border-radius: 16rpx;
  font-size: 30rpx;
  border: 1rpx solid #E17055;
}
</style>
