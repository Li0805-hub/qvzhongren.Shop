<template>
  <view class="review-page">
    <view class="review-card">
      <!-- Star Rating -->
      <view class="rating-section">
        <text class="rating-label">商品评分</text>
        <view class="stars">
          <view
            class="star"
            v-for="i in 5"
            :key="i"
            @click="rating = i"
          >
            <text :class="{ active: i <= rating }">&#x2605;</text>
          </view>
        </view>
      </view>

      <!-- Content -->
      <view class="content-section">
        <textarea
          class="content-input"
          v-model="content"
          placeholder="分享您的使用体验，帮助更多人..."
          placeholder-class="placeholder"
          :maxlength="500"
        />
        <text class="char-count">{{ content.length }}/500</text>
      </view>

      <!-- Submit -->
      <button class="submit-btn" :loading="submitting" @click="handleSubmit">提交评价</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { createReview } from '../../utils/api';

const orderId = ref('');
const productId = ref('');
const rating = ref(5);
const content = ref('');
const submitting = ref(false);

const handleSubmit = async () => {
  if (!content.value.trim()) {
    uni.showToast({ title: '请输入评价内容', icon: 'none' });
    return;
  }
  submitting.value = true;
  try {
    await createReview({
      orderId: orderId.value,
      productId: productId.value,
      rating: rating.value,
      content: content.value,
    });
    uni.showToast({ title: '评价成功', icon: 'success' });
    setTimeout(() => uni.navigateBack(), 500);
  } catch (e) {} finally {
    submitting.value = false;
  }
};

onLoad((query: any) => {
  orderId.value = query.orderId || '';
  productId.value = query.productId || '';
});
</script>

<style scoped>
.review-page {
  min-height: 100vh;
  background: #F5F5F5;
  padding: 20rpx;
}

.review-card {
  background: #fff;
  border-radius: 16rpx;
  padding: 32rpx;
}

.rating-section {
  display: flex;
  align-items: center;
  margin-bottom: 32rpx;
}

.rating-label {
  font-size: 28rpx;
  color: #333;
  margin-right: 24rpx;
}

.stars {
  display: flex;
  gap: 12rpx;
}

.star text {
  font-size: 48rpx;
  color: #ddd;
}

.star text.active {
  color: #f5a623;
}

.content-section {
  position: relative;
  margin-bottom: 32rpx;
}

.content-input {
  width: 100%;
  height: 300rpx;
  background: #f5f5f5;
  border-radius: 12rpx;
  padding: 20rpx;
  font-size: 28rpx;
  box-sizing: border-box;
}

.placeholder {
  color: #ccc;
}

.char-count {
  position: absolute;
  bottom: 12rpx;
  right: 16rpx;
  font-size: 22rpx;
  color: #ccc;
}

.submit-btn {
  width: 100%;
  height: 88rpx;
  line-height: 88rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 16rpx;
  font-size: 32rpx;
  border: none;
}
</style>
