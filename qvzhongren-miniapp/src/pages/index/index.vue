<template>
  <view class="home-page">
    <!-- Search Bar -->
    <view class="search-bar" @click="goSearch">
      <text class="search-icon">&#x1F50D;</text>
      <text class="search-placeholder">搜索商品</text>
    </view>

    <!-- Category Scroll -->
    <scroll-view class="category-scroll" scroll-x>
      <view
        class="category-item"
        v-for="item in categoryList"
        :key="item.categoryId"
        @click="goCategory(item.categoryId)"
      >
        <view class="category-icon-wrap">
          <text class="category-icon-text">{{ item.categoryName?.charAt(0) || '分' }}</text>
        </view>
        <text class="category-name">{{ item.categoryName }}</text>
      </view>
    </scroll-view>

    <!-- Product Grid -->
    <view class="section-title">
      <text>热门推荐</text>
    </view>
    <view class="product-grid">
      <view
        class="product-card"
        v-for="item in productList"
        :key="item.productId"
        @click="goDetail(item.productId)"
      >
        <image
          class="product-img"
          :src="item.mainImage || '/static/logo.png'"
          mode="aspectFill"
        />
        <view class="product-info">
          <text class="product-name">{{ item.productName }}</text>
          <view class="product-price-row">
            <text class="product-price">¥{{ item.price }}</text>
            <text class="product-sales" v-if="item.sales">已售{{ item.sales }}</text>
          </view>
        </view>
      </view>
    </view>

    <!-- Loading -->
    <view class="loading-tip" v-if="loading">
      <text>加载中...</text>
    </view>
    <view class="loading-tip" v-if="!loading && finished">
      <text>没有更多了</text>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { onPullDownRefresh, onReachBottom } from '@dcloudio/uni-app';
import { getCategoryList, getProductPage } from '../../utils/api';

const categoryList = ref<any[]>([]);
const productList = ref<any[]>([]);
const loading = ref(false);
const finished = ref(false);
const pageIndex = ref(1);
const pageSize = 10;

const loadCategories = async () => {
  try {
    const res = await getCategoryList();
    categoryList.value = res.data || [];
  } catch (e) {}
};

const loadProducts = async (refresh = false) => {
  if (loading.value) return;
  if (!refresh && finished.value) return;
  if (refresh) {
    pageIndex.value = 1;
    finished.value = false;
  }
  loading.value = true;
  try {
    const res = await getProductPage({
      pageIndex: pageIndex.value,
      pageSize,
    });
    // 后端返回 { totalCount, pageIndex, pageSize, values: [...] }
    const page = res.data;
    const list = page?.values || [];
    if (refresh) {
      productList.value = list;
    } else {
      productList.value = [...productList.value, ...list];
    }
    if (list.length < pageSize) {
      finished.value = true;
    } else {
      pageIndex.value++;
    }
  } catch (e) {} finally {
    loading.value = false;
  }
};

const goSearch = () => {
  uni.navigateTo({ url: '/pages/search/index' });
};

const goCategory = (id: string) => {
  uni.switchTab({ url: '/pages/category/index' });
};

const goDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/product/detail?id=${id}` });
};

onMounted(() => {
  loadCategories();
  loadProducts(true);
});
</script>

<style scoped>
.home-page {
  min-height: 100vh;
  background: #F5F5F5;
  padding-bottom: 20rpx;
}

.search-bar {
  display: flex;
  align-items: center;
  margin: 20rpx;
  padding: 0 24rpx;
  height: 72rpx;
  background: #fff;
  border-radius: 36rpx;
  box-shadow: 0 2rpx 8rpx rgba(0, 0, 0, 0.05);
}

.search-icon {
  font-size: 28rpx;
  margin-right: 12rpx;
}

.search-placeholder {
  font-size: 28rpx;
  color: #999;
}

.category-scroll {
  white-space: nowrap;
  padding: 20rpx;
  background: #fff;
  margin: 0 20rpx;
  border-radius: 16rpx;
}

.category-item {
  display: inline-flex;
  flex-direction: column;
  align-items: center;
  width: 120rpx;
  margin-right: 16rpx;
}

.category-icon-wrap {
  width: 80rpx;
  height: 80rpx;
  border-radius: 50%;
  background: linear-gradient(135deg, #6C5CE7, #a29bfe);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 8rpx;
}

.category-icon-text {
  color: #fff;
  font-size: 28rpx;
  font-weight: bold;
}

.category-name {
  font-size: 24rpx;
  color: #333;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  max-width: 120rpx;
}

.section-title {
  padding: 24rpx 20rpx 12rpx;
  font-size: 32rpx;
  font-weight: bold;
  color: #333;
}

.product-grid {
  display: flex;
  flex-wrap: wrap;
  padding: 0 12rpx;
}

.product-card {
  width: calc(50% - 24rpx);
  margin: 0 12rpx 20rpx;
  background: #fff;
  border-radius: 16rpx;
  overflow: hidden;
  box-shadow: 0 2rpx 8rpx rgba(0, 0, 0, 0.05);
}

.product-img {
  width: 100%;
  height: 340rpx;
}

.product-info {
  padding: 16rpx;
}

.product-name {
  font-size: 28rpx;
  color: #333;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-height: 1.4;
}

.product-price-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 12rpx;
}

.product-price {
  font-size: 32rpx;
  color: #E17055;
  font-weight: bold;
}

.product-sales {
  font-size: 22rpx;
  color: #999;
}

.loading-tip {
  text-align: center;
  padding: 20rpx;
  font-size: 24rpx;
  color: #999;
}
</style>
