<template>
  <view class="cart-page">
    <!-- Empty State -->
    <view class="empty-cart" v-if="!loading && cartList.length === 0">
      <text class="empty-icon">&#x1F6D2;</text>
      <text class="empty-text">购物车空空如也</text>
      <button class="go-shop-btn" @click="goHome">去逛逛</button>
    </view>

    <!-- Cart List -->
    <view class="cart-list" v-if="cartList.length > 0">
      <view
        class="cart-item"
        v-for="item in cartList"
        :key="item.cartId"
      >
        <view class="item-checkbox" @click="toggleCheck(item)">
          <view class="checkbox" :class="{ checked: item.checked }">
            <text v-if="item.checked" class="check-mark">&#x2713;</text>
          </view>
        </view>
        <image
          class="item-img"
          :src="item.productImage || '/static/logo.png'"
          mode="aspectFill"
          @click="goDetail(item.productId)"
        />
        <view class="item-info">
          <text class="item-name">{{ item.productName }}</text>
          <view class="item-bottom">
            <text class="item-price">¥{{ item.price }}</text>
            <view class="stepper">
              <view class="stepper-btn" @click="changeQty(item, -1)">
                <text>-</text>
              </view>
              <text class="stepper-val">{{ item.quantity }}</text>
              <view class="stepper-btn" @click="changeQty(item, 1)">
                <text>+</text>
              </view>
            </view>
          </view>
        </view>
        <view class="item-delete" @click="handleRemove(item.cartId)">
          <text class="delete-text">删除</text>
        </view>
      </view>
    </view>

    <!-- Bottom Bar -->
    <view class="bottom-bar" v-if="cartList.length > 0">
      <view class="bar-left">
        <view class="item-checkbox" @click="toggleAll">
          <view class="checkbox" :class="{ checked: allChecked }">
            <text v-if="allChecked" class="check-mark">&#x2713;</text>
          </view>
        </view>
        <text class="select-all-text">全选</text>
      </view>
      <view class="bar-right">
        <text class="total-label">合计：</text>
        <text class="total-price">¥{{ totalPrice.toFixed(2) }}</text>
        <button class="checkout-btn" :disabled="checkedCount === 0" @click="handleCheckout">
          结算({{ checkedCount }})
        </button>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { onShow } from '@dcloudio/uni-app';
import { getCartList, updateCartQuantity, removeCart } from '../../utils/api';

const cartList = ref<any[]>([]);
const loading = ref(false);

const allChecked = computed(() => {
  return cartList.value.length > 0 && cartList.value.every((i) => i.checked);
});

const checkedItems = computed(() => cartList.value.filter((i) => i.checked));
const checkedCount = computed(() => checkedItems.value.length);
const totalPrice = computed(() => {
  return checkedItems.value.reduce((sum, i) => sum + (i.price || 0) * i.quantity, 0);
});

const loadCart = async () => {
  const userId = uni.getStorageSync('userId');
  if (!userId) return;
  loading.value = true;
  try {
    const res = await getCartList(userId);
    cartList.value = (res.data || []).map((item: any) => ({ ...item, checked: true }));
  } catch (e) {} finally {
    loading.value = false;
  }
};

const toggleCheck = (item: any) => {
  item.checked = !item.checked;
};

const toggleAll = () => {
  const val = !allChecked.value;
  cartList.value.forEach((i) => (i.checked = val));
};

const changeQty = async (item: any, delta: number) => {
  const newQty = item.quantity + delta;
  if (newQty < 1) return;
  try {
    await updateCartQuantity({ cartId: item.cartId, quantity: newQty });
    item.quantity = newQty;
  } catch (e) {}
};

const handleRemove = async (cartId: string) => {
  uni.showModal({
    title: '提示',
    content: '确定删除该商品？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await removeCart(cartId);
          cartList.value = cartList.value.filter((i) => i.cartId !== cartId);
        } catch (e) {}
      }
    },
  });
};

const goHome = () => {
  uni.switchTab({ url: '/pages/index/index' });
};

const goDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/product/detail?id=${id}` });
};

const handleCheckout = () => {
  const items = checkedItems.value.map((i) => ({
    cartId: i.cartId,
    productId: i.productId,
    productName: i.productName,
    productImage: i.productImage,
    price: i.price,
    quantity: i.quantity,
  }));
  uni.setStorageSync('checkoutItems', JSON.stringify(items));
  uni.navigateTo({ url: '/pages/order/create?from=cart' });
};

onShow(() => {
  loadCart();
});
</script>

<style scoped>
.cart-page {
  min-height: 100vh;
  background: #F5F5F5;
  padding-bottom: 220rpx;
}

.empty-cart {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 200rpx;
}

.empty-icon {
  font-size: 120rpx;
}

.empty-text {
  font-size: 28rpx;
  color: #999;
  margin: 20rpx 0;
}

.go-shop-btn {
  width: 240rpx;
  height: 72rpx;
  line-height: 72rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 36rpx;
  font-size: 28rpx;
  border: none;
}

.cart-list {
  padding: 20rpx;
}

.cart-item {
  display: flex;
  align-items: center;
  background: #fff;
  border-radius: 16rpx;
  padding: 20rpx;
  margin-bottom: 16rpx;
}

.item-checkbox {
  margin-right: 16rpx;
}

.checkbox {
  width: 40rpx;
  height: 40rpx;
  border: 2rpx solid #ddd;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.checkbox.checked {
  background: #6C5CE7;
  border-color: #6C5CE7;
}

.check-mark {
  color: #fff;
  font-size: 24rpx;
}

.item-img {
  width: 160rpx;
  height: 160rpx;
  border-radius: 12rpx;
  margin-right: 16rpx;
  flex-shrink: 0;
}

.item-info {
  flex: 1;
  min-width: 0;
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
  align-items: center;
  justify-content: space-between;
  margin-top: 16rpx;
}

.item-price {
  font-size: 30rpx;
  color: #E17055;
  font-weight: bold;
}

.stepper {
  display: flex;
  align-items: center;
}

.stepper-btn {
  width: 48rpx;
  height: 48rpx;
  background: #f0f0f0;
  border-radius: 8rpx;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 28rpx;
  color: #333;
}

.stepper-val {
  width: 64rpx;
  text-align: center;
  font-size: 28rpx;
}

.item-delete {
  margin-left: 12rpx;
  padding: 8rpx 16rpx;
}

.delete-text {
  font-size: 24rpx;
  color: #E17055;
}

.bottom-bar {
  position: fixed;
  bottom: 100rpx;
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
  align-items: center;
}

.select-all-text {
  font-size: 26rpx;
  color: #333;
}

.bar-right {
  display: flex;
  align-items: center;
}

.total-label {
  font-size: 26rpx;
  color: #333;
}

.total-price {
  font-size: 32rpx;
  color: #E17055;
  font-weight: bold;
  margin-right: 16rpx;
}

.checkout-btn {
  height: 72rpx;
  line-height: 72rpx;
  padding: 0 32rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 36rpx;
  font-size: 28rpx;
  border: none;
}

.checkout-btn[disabled] {
  background: #ccc;
}
</style>
