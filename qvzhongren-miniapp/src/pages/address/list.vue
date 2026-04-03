<template>
  <view class="address-list-page">
    <view class="address-list">
      <view class="address-card" v-for="item in addressList" :key="item.addressId">
        <view class="address-content" @click="handleSelect(item)">
          <view class="address-top">
            <text class="address-name">{{ item.receiverName }}</text>
            <text class="address-phone">{{ item.receiverPhone }}</text>
            <view class="default-tag" v-if="item.isDefault === '1'">
              <text>默认</text>
            </view>
          </view>
          <text class="address-detail">{{ item.province }}{{ item.city }}{{ item.district }} {{ item.detailAddress }}</text>
        </view>
        <view class="address-actions">
          <text class="action-link" @click="handleEdit(item.addressId)">编辑</text>
          <text class="action-link" @click="handleSetDefault(item.addressId)" v-if="item.isDefault !== '1'">设为默认</text>
          <text class="action-link danger" @click="handleDelete(item.addressId)">删除</text>
        </view>
      </view>
    </view>

    <view class="empty-tip" v-if="!loading && addressList.length === 0">
      <text>暂无收货地址</text>
    </view>

    <view class="bottom-bar">
      <button class="add-btn" @click="handleAdd">新增地址</button>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onLoad, onShow } from '@dcloudio/uni-app';
import { getAddressList, deleteAddress, setDefaultAddress } from '../../utils/api';

const addressList = ref<any[]>([]);
const loading = ref(false);
const selectMode = ref(false);

const loadAddresses = async () => {
  const userId = uni.getStorageSync('userId');
  if (!userId) return;
  loading.value = true;
  try {
    const res = await getAddressList(userId);
    addressList.value = res.data || [];
  } catch (e) {} finally {
    loading.value = false;
  }
};

const handleSelect = (item: any) => {
  if (selectMode.value) {
    uni.setStorageSync('selectedAddress', JSON.stringify(item));
    uni.navigateBack();
  }
};

const handleEdit = (addressId: string) => {
  uni.navigateTo({ url: `/pages/address/edit?id=${addressId}` });
};

const handleAdd = () => {
  uni.navigateTo({ url: '/pages/address/edit' });
};

const handleSetDefault = async (addressId: string) => {
  try {
    await setDefaultAddress(addressId);
    uni.showToast({ title: '设置成功', icon: 'success' });
    loadAddresses();
  } catch (e) {}
};

const handleDelete = (addressId: string) => {
  uni.showModal({
    title: '提示',
    content: '确定删除该地址？',
    success: async (res) => {
      if (res.confirm) {
        try {
          await deleteAddress(addressId);
          uni.showToast({ title: '已删除', icon: 'success' });
          loadAddresses();
        } catch (e) {}
      }
    },
  });
};

onLoad((query: any) => {
  selectMode.value = query.select === '1';
});

onShow(() => {
  loadAddresses();
});
</script>

<style scoped>
.address-list-page {
  min-height: 100vh;
  background: #F5F5F5;
  padding-bottom: 120rpx;
}

.address-list {
  padding: 20rpx;
}

.address-card {
  background: #fff;
  border-radius: 16rpx;
  padding: 24rpx;
  margin-bottom: 16rpx;
}

.address-content {
  padding-bottom: 16rpx;
  border-bottom: 1rpx solid #f0f0f0;
}

.address-top {
  display: flex;
  align-items: center;
  gap: 16rpx;
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

.default-tag {
  background: #6C5CE7;
  padding: 2rpx 12rpx;
  border-radius: 4rpx;
}

.default-tag text {
  font-size: 20rpx;
  color: #fff;
}

.address-detail {
  font-size: 26rpx;
  color: #666;
  line-height: 1.4;
}

.address-actions {
  display: flex;
  gap: 32rpx;
  padding-top: 16rpx;
}

.action-link {
  font-size: 26rpx;
  color: #6C5CE7;
}

.action-link.danger {
  color: #E17055;
}

.empty-tip {
  text-align: center;
  padding: 80rpx 0;
  font-size: 28rpx;
  color: #999;
}

.bottom-bar {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 20rpx;
  background: #fff;
  box-shadow: 0 -2rpx 8rpx rgba(0, 0, 0, 0.05);
}

.add-btn {
  width: 100%;
  height: 80rpx;
  line-height: 80rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 40rpx;
  font-size: 30rpx;
  border: none;
}
</style>
