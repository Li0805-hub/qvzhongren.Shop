<template>
  <view class="order-detail-page" v-if="order">
    <!-- Status Banner -->
    <view class="status-banner" :class="'status-bg-' + order.status">
      <text class="status-text">{{ statusText(order.status) }}</text>
    </view>

    <!-- Address -->
    <view class="address-card" v-if="order.receiverName">
      <text class="address-icon">&#x1F4CD;</text>
      <view class="address-info">
        <view class="address-top">
          <text class="address-name">{{ order.receiverName }}</text>
          <text class="address-phone">{{ order.receiverPhone }}</text>
        </view>
        <text class="address-detail">{{ order.receiverAddress }}</text>
      </view>
    </view>

    <!-- Items -->
    <view class="items-card">
      <view class="order-item" v-for="(item, idx) in (order.items || [])" :key="idx">
        <image class="item-img" :src="item.productImage || '/static/logo.png'" mode="aspectFill" />
        <view class="item-info">
          <text class="item-name">{{ item.productName }}</text>
          <view class="item-bottom">
            <text class="item-price">¥{{ item.price }}</text>
            <text class="item-qty">x{{ item.quantity }}</text>
          </view>
        </view>
      </view>
    </view>

    <!-- Price Breakdown -->
    <view class="price-card">
      <view class="price-row">
        <text>商品金额</text>
        <text>¥{{ order.totalAmount }}</text>
      </view>
      <view class="price-row">
        <text>运费</text>
        <text>¥0.00</text>
      </view>
      <view class="price-row total">
        <text>实付金额</text>
        <text class="total-val">¥{{ order.totalAmount }}</text>
      </view>
    </view>

    <!-- Order Info -->
    <view class="info-card">
      <view class="info-row">
        <text class="info-label">订单编号</text>
        <text class="info-val">{{ order.orderNo }}</text>
      </view>
      <view class="info-row">
        <text class="info-label">创建时间</text>
        <text class="info-val">{{ order.createDate }}</text>
      </view>
      <view class="info-row" v-if="order.payTime">
        <text class="info-label">付款时间</text>
        <text class="info-val">{{ order.payTime }}</text>
      </view>
      <view class="info-row" v-if="order.remark">
        <text class="info-label">备注</text>
        <text class="info-val">{{ order.remark }}</text>
      </view>
    </view>

    <view style="height: 120rpx;"></view>

    <!-- Bottom Actions -->
    <view class="bottom-bar" v-if="order.status === '0' || order.status === '2' || order.status === '3'">
      <button class="action-btn" v-if="order.status === '0'" @click="handleCancel">取消订单</button>
      <button class="action-btn pay-btn" v-if="order.status === '0'" @click="showPayModal = true">立即付款</button>
      <button class="action-btn primary" v-if="order.status === '2'" @click="handleConfirm">确认收货</button>
      <button class="action-btn primary" v-if="order.status === '3'" @click="goReview">去评价</button>
    </view>

    <!-- 沙盒支付弹窗 -->
    <view class="pay-mask" v-if="showPayModal" @click="showPayModal = false">
      <view class="pay-modal" @click.stop>
        <view class="pay-header">
          <text class="pay-title">确认支付</text>
          <text class="pay-close" @click="showPayModal = false">&#x2715;</text>
        </view>
        <view class="pay-amount">
          <text class="pay-label">支付金额</text>
          <text class="pay-price">¥{{ order.totalAmount }}</text>
        </view>
        <view class="pay-methods">
          <view class="pay-method" :class="{ active: payMethod === 'wechat' }" @click="payMethod = 'wechat'">
            <text class="method-icon">&#x1F4B3;</text>
            <text class="method-name">微信支付（沙盒）</text>
            <view class="method-radio" :class="{ checked: payMethod === 'wechat' }"></view>
          </view>
          <view class="pay-method" :class="{ active: payMethod === 'alipay' }" @click="payMethod = 'alipay'">
            <text class="method-icon">&#x1F4B0;</text>
            <text class="method-name">支付宝（沙盒）</text>
            <view class="method-radio" :class="{ checked: payMethod === 'alipay' }"></view>
          </view>
        </view>
        <button class="confirm-pay-btn" :loading="paying" @click="handlePay">
          确认支付 ¥{{ order.totalAmount }}
        </button>
        <text class="pay-tip">* 沙盒模式，无需真实付款</text>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { getOrderById, cancelOrder, payOrder } from '../../utils/api';

const order = ref<any>(null);
const orderId = ref('');
const showPayModal = ref(false);
const payMethod = ref('wechat');
const paying = ref(false);

const statusMap: Record<string, string> = {
  '0': '待付款', '1': '待发货', '2': '待收货', '3': '已完成', '-1': '已取消'
};
const statusText = (status: string) => statusMap[status] || '未知';

const loadOrder = async () => {
  try {
    const res = await getOrderById(orderId.value);
    order.value = res.data;
  } catch (e) {}
};

const handlePay = async () => {
  paying.value = true;
  try {
    // 模拟支付延迟
    await new Promise(resolve => setTimeout(resolve, 1500));
    await payOrder(orderId.value);
    showPayModal.value = false;
    uni.showToast({ title: '支付成功！', icon: 'success' });
    loadOrder();
  } catch (e) {} finally {
    paying.value = false;
  }
};

const handleCancel = () => {
  uni.showModal({
    title: '提示',
    content: '确定取消订单？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await cancelOrder(orderId.value);
          uni.showToast({ title: '已取消', icon: 'success' });
          loadOrder();
        } catch (e) {}
      }
    },
  });
};

const handleConfirm = () => {
  uni.showModal({
    title: '提示',
    content: '确认收货？',
    success: async (res) => {
      if (res.confirm) {
        uni.showToast({ title: '已确认', icon: 'success' });
        loadOrder();
      }
    },
  });
};

const goReview = () => {
  const firstItem = (order.value?.items || [])[0];
  if (firstItem) {
    uni.navigateTo({
      url: `/pages/review/create?orderId=${orderId.value}&productId=${firstItem.productId}`,
    });
  }
};

onLoad((query: any) => {
  orderId.value = query.id || '';
  if (orderId.value) {
    loadOrder();
  }
});
</script>

<style scoped>
.order-detail-page {
  min-height: 100vh;
  background: #F5F5F5;
}

.status-banner {
  padding: 40rpx 24rpx;
}
.status-bg-0 { background: #E17055; }
.status-bg-1 { background: #6C5CE7; }
.status-bg-2 { background: #00b894; }
.status-bg-3 { background: #636e72; }
.status-bg--1 { background: #b2bec3; }

.status-text {
  font-size: 36rpx;
  color: #fff;
  font-weight: bold;
}

.address-card {
  display: flex;
  background: #fff;
  padding: 24rpx;
  margin: 20rpx;
  border-radius: 16rpx;
}
.address-icon { margin-right: 16rpx; font-size: 32rpx; }
.address-info { flex: 1; }
.address-top { display: flex; gap: 20rpx; margin-bottom: 8rpx; }
.address-name { font-size: 30rpx; font-weight: bold; color: #333; }
.address-phone { font-size: 28rpx; color: #666; }
.address-detail { font-size: 26rpx; color: #666; }

.items-card {
  background: #fff;
  margin: 0 20rpx 20rpx;
  border-radius: 16rpx;
  padding: 20rpx;
}
.order-item { display: flex; padding: 12rpx 0; border-bottom: 1rpx solid #f0f0f0; }
.order-item:last-child { border-bottom: none; }
.item-img { width: 140rpx; height: 140rpx; border-radius: 12rpx; margin-right: 16rpx; flex-shrink: 0; }
.item-info { flex: 1; display: flex; flex-direction: column; justify-content: space-between; }
.item-name { font-size: 28rpx; color: #333; }
.item-bottom { display: flex; justify-content: space-between; }
.item-price { font-size: 28rpx; color: #E17055; font-weight: bold; }
.item-qty { font-size: 26rpx; color: #999; }

.price-card { background: #fff; margin: 0 20rpx 20rpx; border-radius: 16rpx; padding: 20rpx; }
.price-row { display: flex; justify-content: space-between; padding: 10rpx 0; font-size: 28rpx; color: #666; }
.price-row.total { border-top: 1rpx solid #f0f0f0; padding-top: 16rpx; margin-top: 8rpx; }
.total-val { color: #E17055; font-weight: bold; font-size: 32rpx; }

.info-card { background: #fff; margin: 0 20rpx; border-radius: 16rpx; padding: 20rpx; }
.info-row { display: flex; justify-content: space-between; padding: 10rpx 0; }
.info-label { font-size: 26rpx; color: #999; }
.info-val { font-size: 26rpx; color: #333; }

.bottom-bar {
  position: fixed;
  bottom: 0; left: 0; right: 0;
  display: flex;
  justify-content: flex-end;
  gap: 16rpx;
  padding: 16rpx 20rpx;
  background: #fff;
  box-shadow: 0 -2rpx 8rpx rgba(0, 0, 0, 0.05);
  z-index: 100;
}
.action-btn {
  height: 72rpx; line-height: 72rpx; padding: 0 32rpx;
  font-size: 28rpx; border-radius: 36rpx;
  background: #fff; color: #666; border: 1rpx solid #ddd;
}
.action-btn.primary { background: #6C5CE7; color: #fff; border-color: #6C5CE7; }
.action-btn.pay-btn { background: #E17055; color: #fff; border-color: #E17055; }

/* 支付弹窗 */
.pay-mask {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  z-index: 999;
  display: flex;
  align-items: flex-end;
}
.pay-modal {
  width: 100%;
  background: #fff;
  border-radius: 32rpx 32rpx 0 0;
  padding: 40rpx;
  animation: slideUp 0.3s ease;
}
@keyframes slideUp {
  from { transform: translateY(100%); }
  to { transform: translateY(0); }
}
.pay-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 40rpx;
}
.pay-title { font-size: 34rpx; font-weight: bold; color: #333; }
.pay-close { font-size: 36rpx; color: #999; padding: 10rpx; }

.pay-amount {
  text-align: center;
  margin-bottom: 48rpx;
}
.pay-label { font-size: 28rpx; color: #999; display: block; margin-bottom: 12rpx; }
.pay-price { font-size: 64rpx; color: #E17055; font-weight: bold; }

.pay-methods { margin-bottom: 40rpx; }
.pay-method {
  display: flex;
  align-items: center;
  padding: 24rpx;
  border: 2rpx solid #eee;
  border-radius: 16rpx;
  margin-bottom: 16rpx;
}
.pay-method.active { border-color: #6C5CE7; background: #f8f7ff; }
.method-icon { font-size: 40rpx; margin-right: 16rpx; }
.method-name { flex: 1; font-size: 28rpx; color: #333; }
.method-radio {
  width: 36rpx; height: 36rpx;
  border: 2rpx solid #ddd; border-radius: 50%;
}
.method-radio.checked {
  border-color: #6C5CE7;
  background: #6C5CE7;
  box-shadow: inset 0 0 0 4rpx #fff;
}

.confirm-pay-btn {
  width: 100%;
  height: 88rpx; line-height: 88rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 44rpx;
  font-size: 32rpx;
  border: none;
  font-weight: bold;
}
.pay-tip {
  display: block;
  text-align: center;
  font-size: 22rpx;
  color: #999;
  margin-top: 16rpx;
}
</style>
