<template>
  <view class="detail-page" v-if="product">
    <!-- Image Swiper -->
    <swiper class="swiper" indicator-dots autoplay circular>
      <swiper-item v-for="(img, idx) in images" :key="idx">
        <image class="swiper-img" :src="img" mode="aspectFill" />
      </swiper-item>
      <swiper-item v-if="images.length === 0">
        <image class="swiper-img" src="/static/logo.png" mode="aspectFill" />
      </swiper-item>
    </swiper>

    <!-- Product Info -->
    <view class="info-card">
      <view class="price-row">
        <text class="price">¥{{ product.price }}</text>
        <text class="original-price" v-if="product.originalPrice">¥{{ product.originalPrice }}</text>
        <text class="sales" v-if="product.salesCount">已售{{ product.salesCount }}件</text>
      </view>
      <text class="product-name">{{ product.name }}</text>
    </view>

    <!-- Description -->
    <view class="desc-card" v-if="product.description">
      <text class="card-title">商品详情</text>
      <text class="desc-text">{{ product.description }}</text>
    </view>

    <!-- Reviews -->
    <view class="review-card">
      <text class="card-title">商品评价 ({{ reviews.length }})</text>
      <view v-if="reviews.length === 0" class="empty-tip">
        <text>暂无评价</text>
      </view>
      <view class="review-item" v-for="item in reviews" :key="item.id">
        <view class="review-header">
          <text class="review-user">{{ item.userName || '匿名用户' }}</text>
          <text class="review-stars">{{ '★'.repeat(item.rating || 5) }}</text>
        </view>
        <text class="review-content">{{ item.content }}</text>
      </view>
    </view>

    <!-- Bottom spacer -->
    <view style="height: 120rpx;"></view>

    <!-- Bottom Bar -->
    <view class="bottom-bar">
      <button class="btn-cart" @click="handleAddCart">加入购物车</button>
      <button class="btn-buy" @click="handleBuyNow">立即购买</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { getProductById, getReviewsByProduct, addToCart } from '../../utils/api';

const product = ref<any>(null);
const reviews = ref<any[]>([]);
const productId = ref('');

const images = computed(() => {
  if (!product.value) return [];
  try {
    if (product.value.images) {
      if (typeof product.value.images === 'string') {
        return JSON.parse(product.value.images);
      }
      return product.value.images;
    }
    if (product.value.mainImage) return [product.value.mainImage];
  } catch (e) {}
  return [];
});

const loadProduct = async () => {
  try {
    const res = await getProductById(productId.value);
    product.value = res.data;
  } catch (e) {}
};

const loadReviews = async () => {
  try {
    const res = await getReviewsByProduct(productId.value);
    reviews.value = res.data || [];
  } catch (e) {}
};

const handleAddCart = async () => {
  const userId = uni.getStorageSync('userId');
  if (!userId) {
    uni.navigateTo({ url: '/pages/login/index' });
    return;
  }
  try {
    await addToCart({ productId: productId.value, quantity: 1 });
    uni.showToast({ title: '已加入购物车', icon: 'success' });
  } catch (e) {}
};

const handleBuyNow = () => {
  const userId = uni.getStorageSync('userId');
  if (!userId) {
    uni.navigateTo({ url: '/pages/login/index' });
    return;
  }
  uni.navigateTo({
    url: `/pages/order/create?productId=${productId.value}&quantity=1`,
  });
};

onLoad((query: any) => {
  productId.value = query.id || '';
  if (productId.value) {
    loadProduct();
    loadReviews();
  }
});
</script>

<style scoped>
.detail-page {
  min-height: 100vh;
  background: #F5F5F5;
}

.swiper {
  width: 100%;
  height: 600rpx;
}

.swiper-img {
  width: 100%;
  height: 100%;
}

.info-card {
  background: #fff;
  padding: 24rpx;
  margin-bottom: 16rpx;
}

.price-row {
  display: flex;
  align-items: baseline;
  gap: 16rpx;
}

.price {
  font-size: 44rpx;
  color: #E17055;
  font-weight: bold;
}

.original-price {
  font-size: 26rpx;
  color: #999;
  text-decoration: line-through;
}

.sales {
  font-size: 24rpx;
  color: #999;
  margin-left: auto;
}

.product-name {
  display: block;
  font-size: 32rpx;
  color: #333;
  margin-top: 16rpx;
  line-height: 1.5;
}

.desc-card,
.review-card {
  background: #fff;
  padding: 24rpx;
  margin-bottom: 16rpx;
}

.card-title {
  display: block;
  font-size: 30rpx;
  font-weight: bold;
  color: #333;
  margin-bottom: 16rpx;
}

.desc-text {
  font-size: 28rpx;
  color: #666;
  line-height: 1.6;
}

.empty-tip {
  text-align: center;
  padding: 40rpx;
  color: #999;
  font-size: 26rpx;
}

.review-item {
  padding: 16rpx 0;
  border-bottom: 1rpx solid #f0f0f0;
}

.review-item:last-child {
  border-bottom: none;
}

.review-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8rpx;
}

.review-user {
  font-size: 26rpx;
  color: #333;
}

.review-stars {
  color: #f5a623;
  font-size: 24rpx;
}

.review-content {
  font-size: 26rpx;
  color: #666;
  line-height: 1.5;
}

.bottom-bar {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  display: flex;
  padding: 16rpx 20rpx;
  background: #fff;
  box-shadow: 0 -2rpx 8rpx rgba(0, 0, 0, 0.05);
  z-index: 100;
}

.btn-cart,
.btn-buy {
  flex: 1;
  height: 80rpx;
  line-height: 80rpx;
  text-align: center;
  border-radius: 40rpx;
  font-size: 28rpx;
  margin: 0 8rpx;
  border: none;
}

.btn-cart {
  background: #fff;
  color: #6C5CE7;
  border: 2rpx solid #6C5CE7;
}

.btn-buy {
  background: #6C5CE7;
  color: #fff;
}
</style>
