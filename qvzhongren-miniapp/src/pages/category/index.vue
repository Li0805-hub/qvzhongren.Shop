<template>
  <view class="category-page">
    <!-- Left sidebar -->
    <scroll-view class="sidebar" scroll-y>
      <view
        class="sidebar-item"
        :class="{ active: currentId === item.categoryId }"
        v-for="item in categoryList"
        :key="item.categoryId"
        @click="selectCategory(item)"
      >
        <text>{{ item.categoryName }}</text>
      </view>
    </scroll-view>

    <!-- Right content -->
    <scroll-view class="content" scroll-y @scrolltolower="loadMore">
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
            <text class="product-price">¥{{ item.price }}</text>
          </view>
        </view>
      </view>
      <view class="loading-tip" v-if="loading">
        <text>加载中...</text>
      </view>
      <view class="loading-tip" v-if="!loading && finished">
        <text>没有更多了</text>
      </view>
    </scroll-view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getCategoryList, getProductPage } from '../../utils/api';

const categoryList = ref<any[]>([]);
const currentId = ref('');
const productList = ref<any[]>([]);
const loading = ref(false);
const finished = ref(false);
const pageIndex = ref(1);
const pageSize = 10;

const loadCategories = async () => {
  try {
    const res = await getCategoryList();
    categoryList.value = res.data || [];
    if (categoryList.value.length > 0) {
      selectCategory(categoryList.value[0]);
    }
  } catch (e) {}
};

const selectCategory = (item: any) => {
  currentId.value = item.categoryId;
  productList.value = [];
  pageIndex.value = 1;
  finished.value = false;
  loadProducts();
};

const loadProducts = async () => {
  if (loading.value || finished.value) return;
  loading.value = true;
  try {
    const res = await getProductPage({
      categoryId: currentId.value,
      pageIndex: pageIndex.value,
      pageSize,
    });
    const list = res.data?.values || [];
    productList.value = [...productList.value, ...list];
    if (list.length < pageSize) {
      finished.value = true;
    } else {
      pageIndex.value++;
    }
  } catch (e) {} finally {
    loading.value = false;
  }
};

const loadMore = () => {
  loadProducts();
};

const goDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/product/detail?id=${id}` });
};

onMounted(() => {
  loadCategories();
});
</script>

<style scoped>
.category-page {
  display: flex;
  height: 100vh;
  background: #F5F5F5;
}

.sidebar {
  width: 200rpx;
  background: #fff;
  height: 100%;
}

.sidebar-item {
  padding: 28rpx 20rpx;
  font-size: 26rpx;
  color: #666;
  text-align: center;
  border-left: 4rpx solid transparent;
}

.sidebar-item.active {
  color: #6C5CE7;
  background: #F5F5F5;
  border-left-color: #6C5CE7;
  font-weight: bold;
}

.content {
  flex: 1;
  height: 100%;
  padding: 12rpx;
}

.product-grid {
  display: flex;
  flex-wrap: wrap;
}

.product-card {
  width: calc(50% - 16rpx);
  margin: 8rpx;
  background: #fff;
  border-radius: 16rpx;
  overflow: hidden;
}

.product-img {
  width: 100%;
  height: 240rpx;
}

.product-info {
  padding: 12rpx;
}

.product-name {
  font-size: 26rpx;
  color: #333;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.product-price {
  font-size: 28rpx;
  color: #E17055;
  font-weight: bold;
  margin-top: 8rpx;
  display: block;
}

.loading-tip {
  text-align: center;
  padding: 20rpx;
  font-size: 24rpx;
  color: #999;
}
</style>
