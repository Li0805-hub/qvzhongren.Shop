<template>
  <view class="create-order-page">
    <!-- Address Section -->
    <view class="address-card" @click="goAddressList">
      <view v-if="address" class="address-info">
        <view class="address-top">
          <text class="address-name">{{ address.receiverName }}</text>
          <text class="address-phone">{{ address.receiverPhone }}</text>
        </view>
        <text class="address-detail">{{ address.province }}{{ address.city }}{{ address.district }}{{ address.detailAddress }}</text>
      </view>
      <view v-else class="no-address">
        <text>请选择收货地址</text>
      </view>
      <text class="arrow">&#x276F;</text>
    </view>

    <!-- Order Items -->
    <view class="items-card">
      <view class="order-item" v-for="(item, idx) in orderItems" :key="idx">
        <image
          class="item-img"
          :src="item.productImage || '/static/logo.png'"
          mode="aspectFill"
        />
        <view class="item-info">
          <text class="item-name">{{ item.productName }}</text>
          <view class="item-bottom">
            <text class="item-price">¥{{ item.price }}</text>
            <text class="item-qty">x{{ item.quantity }}</text>
          </view>
        </view>
      </view>
    </view>

    <!-- Remark -->
    <view class="remark-card">
      <text class="remark-label">备注</text>
      <input
        class="remark-input"
        v-model="remark"
        placeholder="选填，请先和商家协商一致"
        placeholder-class="placeholder"
      />
    </view>

    <!-- Summary -->
    <view class="summary-card">
      <view class="summary-row">
        <text>商品金额</text>
        <text class="summary-val">¥{{ totalAmount.toFixed(2) }}</text>
      </view>
      <view class="summary-row">
        <text>运费</text>
        <text class="summary-val">¥0.00</text>
      </view>
    </view>

    <!-- Bottom spacer -->
    <view style="height: 120rpx;"></view>

    <!-- Bottom Bar -->
    <view class="bottom-bar">
      <view class="bar-left">
        <text class="total-label">合计：</text>
        <text class="total-price">¥{{ totalAmount.toFixed(2) }}</text>
      </view>
      <button class="submit-btn" @click="handleSubmit" :loading="submitting">提交订单</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { onLoad, onShow } from '@dcloudio/uni-app';
import { createOrder, getProductById, getAddressList } from '../../utils/api';

const orderItems = ref<any[]>([]);
const address = ref<any>(null);
const remark = ref('');
const submitting = ref(false);
const fromType = ref('');
const productId = ref('');
const quantity = ref(1);

const totalAmount = computed(() => {
  return orderItems.value.reduce((sum, item) => sum + (item.price || 0) * (item.quantity || 1), 0);
});

const loadDefaultAddress = async () => {
  const userId = uni.getStorageSync('userId');
  if (!userId) return;
  try {
    const res = await getAddressList(userId);
    const list = res.data || [];
    address.value = list.find((a: any) => a.isDefault === '1') || list[0] || null;
  } catch (e) {}
};

const loadDirectBuy = async () => {
  try {
    const res = await getProductById(productId.value);
    const p = res.data;
    orderItems.value = [{
      productId: p.id,
      productName: p.name,
      productImage: p.mainImage,
      price: p.price,
      quantity: quantity.value,
    }];
  } catch (e) {}
};

const goAddressList = () => {
  uni.navigateTo({ url: '/pages/address/list?select=1' });
};

const handleSubmit = async () => {
  if (!address.value) {
    uni.showToast({ title: '请选择收货地址', icon: 'none' });
    return;
  }
  if (orderItems.value.length === 0) {
    uni.showToast({ title: '没有商品信息', icon: 'none' });
    return;
  }
  submitting.value = true;
  try {
    const data = {
      addressId: address.value.addressId,
      remark: remark.value,
      items: orderItems.value.map((i) => ({
        productId: i.productId,
        quantity: i.quantity,
        price: i.price,
      })),
    };
    const res = await createOrder(data);
    uni.showToast({ title: '下单成功', icon: 'success' });
    // Clear cart items from storage
    uni.removeStorageSync('checkoutItems');
    setTimeout(() => {
      uni.redirectTo({ url: `/pages/order/detail?id=${res.data?.id || res.data}` });
    }, 500);
  } catch (e) {} finally {
    submitting.value = false;
  }
};

onLoad((query: any) => {
  fromType.value = query.from || '';
  productId.value = query.productId || '';
  quantity.value = parseInt(query.quantity || '1');

  if (fromType.value === 'cart') {
    const stored = uni.getStorageSync('checkoutItems');
    if (stored) {
      orderItems.value = JSON.parse(stored);
    }
  } else if (productId.value) {
    loadDirectBuy();
  }
  loadDefaultAddress();
});

// Receive selected address from address list page
onShow(() => {
  const selected = uni.getStorageSync('selectedAddress');
  if (selected) {
    address.value = JSON.parse(selected);
    uni.removeStorageSync('selectedAddress');
  }
});
</script>

<style scoped>
.create-order-page {
  min-height: 100vh;
  background: #F5F5F5;
}

.address-card {
  display: flex;
  align-items: center;
  background: #fff;
  padding: 24rpx;
  margin: 20rpx;
  border-radius: 16rpx;
}

.address-info {
  flex: 1;
}

.address-top {
  display: flex;
  gap: 20rpx;
  margin-bottom: 8rpx;
}

.address-name {
  font-size: 30rpx;
  font-weight: bold;
  color: #333;
}

.address-phone {
  font-size: 28rpx;
  color: #666;
}

.address-detail {
  font-size: 26rpx;
  color: #666;
  line-height: 1.4;
}

.no-address {
  flex: 1;
  font-size: 28rpx;
  color: #999;
}

.arrow {
  color: #ccc;
  font-size: 28rpx;
  margin-left: 12rpx;
}

.items-card {
  background: #fff;
  margin: 0 20rpx 20rpx;
  border-radius: 16rpx;
  padding: 20rpx;
}

.order-item {
  display: flex;
  padding: 12rpx 0;
  border-bottom: 1rpx solid #f0f0f0;
}

.order-item:last-child {
  border-bottom: none;
}

.item-img {
  width: 140rpx;
  height: 140rpx;
  border-radius: 12rpx;
  margin-right: 16rpx;
  flex-shrink: 0;
}

.item-info {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.item-name {
  font-size: 28rpx;
  color: #333;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.item-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.item-price {
  font-size: 28rpx;
  color: #E17055;
  font-weight: bold;
}

.item-qty {
  font-size: 26rpx;
  color: #999;
}

.remark-card {
  display: flex;
  align-items: center;
  background: #fff;
  padding: 24rpx;
  margin: 0 20rpx 20rpx;
  border-radius: 16rpx;
}

.remark-label {
  font-size: 28rpx;
  color: #333;
  margin-right: 16rpx;
  flex-shrink: 0;
}

.remark-input {
  flex: 1;
  font-size: 26rpx;
}

.placeholder {
  color: #ccc;
}

.summary-card {
  background: #fff;
  padding: 24rpx;
  margin: 0 20rpx;
  border-radius: 16rpx;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  padding: 8rpx 0;
  font-size: 28rpx;
  color: #666;
}

.summary-val {
  color: #333;
}

.bottom-bar {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16rpx 20rpx;
  background: #fff;
  box-shadow: 0 -2rpx 8rpx rgba(0, 0, 0, 0.05);
  z-index: 100;
}

.bar-left {
  display: flex;
  align-items: baseline;
}

.total-label {
  font-size: 28rpx;
  color: #333;
}

.total-price {
  font-size: 36rpx;
  color: #E17055;
  font-weight: bold;
}

.submit-btn {
  height: 80rpx;
  line-height: 80rpx;
  padding: 0 48rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 40rpx;
  font-size: 30rpx;
  border: none;
}
</style>
