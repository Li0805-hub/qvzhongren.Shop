<template>
  <view class="search-page">
    <!-- Search Input -->
    <view class="search-header">
      <view class="search-input-wrap">
        <input
          class="search-input"
          v-model="keyword"
          :focus="true"
          placeholder="搜索商品"
          placeholder-class="placeholder"
          confirm-type="search"
          @confirm="doSearch"
        />
      </view>
      <text class="search-btn" @click="doSearch">搜索</text>
    </view>

    <!-- Before search: history & hot -->
    <view v-if="!searched">
      <!-- Search History -->
      <view class="section" v-if="history.length > 0">
        <view class="section-header">
          <text class="section-title">搜索历史</text>
          <text class="clear-btn" @click="clearHistory">清空</text>
        </view>
        <view class="tag-list">
          <view class="tag" v-for="(item, idx) in history" :key="idx" @click="searchTag(item)">
            <text>{{ item }}</text>
          </view>
        </view>
      </view>

      <!-- Hot Search -->
      <view class="section">
        <view class="section-header">
          <text class="section-title">热门搜索</text>
        </view>
        <view class="tag-list">
          <view class="tag hot" v-for="(item, idx) in hotTags" :key="idx" @click="searchTag(item)">
            <text>{{ item }}</text>
          </view>
        </view>
      </view>
    </view>

    <!-- Search Results -->
    <view v-if="searched">
      <view class="product-grid" v-if="productList.length > 0">
        <view
          class="product-card"
          v-for="item in productList"
          :key="item.id"
          @click="goDetail(item.id)"
        >
          <image class="product-img" :src="item.mainImage || '/static/logo.png'" mode="aspectFill" />
          <view class="product-info">
            <text class="product-name">{{ item.name }}</text>
            <text class="product-price">¥{{ item.price }}</text>
          </view>
        </view>
      </view>
      <view class="empty-tip" v-if="!loading && productList.length === 0">
        <text>未找到相关商品</text>
      </view>
      <view class="loading-tip" v-if="loading">
        <text>搜索中...</text>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getProductPage } from '../../utils/api';

const keyword = ref('');
const searched = ref(false);
const loading = ref(false);
const productList = ref<any[]>([]);
const history = ref<string[]>([]);

const hotTags = ['手机', '电脑', '耳机', '零食', '服装', '鞋子', '美妆', '家居'];

const loadHistory = () => {
  const stored = uni.getStorageSync('searchHistory');
  if (stored) {
    history.value = JSON.parse(stored);
  }
};

const saveHistory = (kw: string) => {
  const list = history.value.filter((i) => i !== kw);
  list.unshift(kw);
  if (list.length > 10) list.length = 10;
  history.value = list;
  uni.setStorageSync('searchHistory', JSON.stringify(list));
};

const clearHistory = () => {
  history.value = [];
  uni.removeStorageSync('searchHistory');
};

const doSearch = async () => {
  const kw = keyword.value.trim();
  if (!kw) return;
  saveHistory(kw);
  searched.value = true;
  loading.value = true;
  productList.value = [];
  try {
    const res = await getProductPage({ keyword: kw, pageIndex: 1, pageSize: 20 });
    productList.value = res.data?.items || res.data?.list || [];
  } catch (e) {} finally {
    loading.value = false;
  }
};

const searchTag = (tag: string) => {
  keyword.value = tag;
  doSearch();
};

const goDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/product/detail?id=${id}` });
};

onMounted(() => {
  loadHistory();
});
</script>

<style scoped>
.search-page {
  min-height: 100vh;
  background: #F5F5F5;
}

.search-header {
  display: flex;
  align-items: center;
  padding: 20rpx;
  background: #fff;
}

.search-input-wrap {
  flex: 1;
  height: 68rpx;
  background: #f0f0f0;
  border-radius: 34rpx;
  padding: 0 24rpx;
  display: flex;
  align-items: center;
}

.search-input {
  width: 100%;
  font-size: 28rpx;
}

.placeholder {
  color: #ccc;
}

.search-btn {
  margin-left: 16rpx;
  font-size: 28rpx;
  color: #6C5CE7;
  font-weight: bold;
  flex-shrink: 0;
}

.section {
  padding: 24rpx 20rpx;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16rpx;
}

.section-title {
  font-size: 30rpx;
  font-weight: bold;
  color: #333;
}

.clear-btn {
  font-size: 26rpx;
  color: #999;
}

.tag-list {
  display: flex;
  flex-wrap: wrap;
  gap: 16rpx;
}

.tag {
  padding: 12rpx 24rpx;
  background: #fff;
  border-radius: 24rpx;
  font-size: 26rpx;
  color: #666;
}

.tag.hot {
  background: #f0ecff;
  color: #6C5CE7;
}

.product-grid {
  display: flex;
  flex-wrap: wrap;
  padding: 12rpx;
}

.product-card {
  width: calc(50% - 24rpx);
  margin: 12rpx;
  background: #fff;
  border-radius: 16rpx;
  overflow: hidden;
}

.product-img {
  width: 100%;
  height: 300rpx;
}

.product-info {
  padding: 16rpx;
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
  display: block;
  font-size: 30rpx;
  color: #E17055;
  font-weight: bold;
  margin-top: 8rpx;
}

.empty-tip,
.loading-tip {
  text-align: center;
  padding: 80rpx 0;
  font-size: 28rpx;
  color: #999;
}
</style>
