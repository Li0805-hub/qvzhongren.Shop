<template>
  <view class="address-edit-page">
    <view class="form-card">
      <view class="form-item">
        <text class="form-label">收货人</text>
        <input class="form-input" v-model="form.receiverName" placeholder="请输入收货人姓名" placeholder-class="placeholder" />
      </view>
      <view class="form-item">
        <text class="form-label">联系电话</text>
        <input class="form-input" v-model="form.receiverPhone" type="number" placeholder="请输入联系电话" placeholder-class="placeholder" />
      </view>
      <view class="form-item">
        <text class="form-label">省份</text>
        <input class="form-input" v-model="form.province" placeholder="请输入省份" placeholder-class="placeholder" />
      </view>
      <view class="form-item">
        <text class="form-label">城市</text>
        <input class="form-input" v-model="form.city" placeholder="请输入城市" placeholder-class="placeholder" />
      </view>
      <view class="form-item">
        <text class="form-label">区/县</text>
        <input class="form-input" v-model="form.district" placeholder="请输入区/县" placeholder-class="placeholder" />
      </view>
      <view class="form-item">
        <text class="form-label">详细地址</text>
        <input class="form-input" v-model="form.detailAddress" placeholder="请输入详细地址" placeholder-class="placeholder" />
      </view>
      <view class="form-item switch-item">
        <text class="form-label">设为默认地址</text>
        <switch :checked="form.isDefault === '1'" @change="form.isDefault = $event.detail.value ? '1' : '0'" color="#6C5CE7" />
      </view>
    </view>

    <button class="save-btn" :loading="saving" @click="handleSave">保存</button>
  </view>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { onLoad } from '@dcloudio/uni-app';
import { createAddress, updateAddress, getAddressList } from '../../utils/api';

const addressId = ref('');
const saving = ref(false);
const form = reactive({
  receiverName: '',
  receiverPhone: '',
  province: '',
  city: '',
  district: '',
  detailAddress: '',
  isDefault: '0',
});

const loadAddress = async () => {
  const userId = uni.getStorageSync('userId');
  if (!userId) return;
  try {
    const res = await getAddressList(userId);
    const item = (res.data || []).find((a: any) => a.addressId === addressId.value);
    if (item) {
      form.receiverName = item.receiverName || '';
      form.receiverPhone = item.receiverPhone || '';
      form.province = item.province || '';
      form.city = item.city || '';
      form.district = item.district || '';
      form.detailAddress = item.detailAddress || '';
      form.isDefault = item.isDefault || '0';
    }
  } catch (e) {}
};

const handleSave = async () => {
  if (!form.receiverName) {
    uni.showToast({ title: '请输入收货人', icon: 'none' });
    return;
  }
  if (!form.receiverPhone) {
    uni.showToast({ title: '请输入联系电话', icon: 'none' });
    return;
  }
  if (!form.detailAddress) {
    uni.showToast({ title: '请输入详细地址', icon: 'none' });
    return;
  }

  saving.value = true;
  try {
    const userId = uni.getStorageSync('userId');
    const data: any = {
      ...form,
      userId,
      addressId: addressId.value || undefined,
    };
    if (addressId.value) {
      await updateAddress(data);
    } else {
      await createAddress(data);
    }
    uni.showToast({ title: '保存成功', icon: 'success' });
    setTimeout(() => uni.navigateBack(), 500);
  } catch (e) {} finally {
    saving.value = false;
  }
};

onLoad((query: any) => {
  addressId.value = query.id || '';
  if (addressId.value) {
    uni.setNavigationBarTitle({ title: '编辑地址' });
    loadAddress();
  } else {
    uni.setNavigationBarTitle({ title: '新增地址' });
  }
});
</script>

<style scoped>
.address-edit-page {
  min-height: 100vh;
  background: #F5F5F5;
  padding: 20rpx;
}

.form-card {
  background: #fff;
  border-radius: 16rpx;
  padding: 0 24rpx;
}

.form-item {
  display: flex;
  align-items: center;
  padding: 24rpx 0;
  border-bottom: 1rpx solid #f0f0f0;
}

.form-item:last-child {
  border-bottom: none;
}

.switch-item {
  justify-content: space-between;
}

.form-label {
  width: 160rpx;
  font-size: 28rpx;
  color: #333;
  flex-shrink: 0;
}

.form-input {
  flex: 1;
  font-size: 28rpx;
  color: #333;
}

.placeholder {
  color: #ccc;
}

.save-btn {
  width: 100%;
  height: 88rpx;
  line-height: 88rpx;
  background: #6C5CE7;
  color: #fff;
  border-radius: 16rpx;
  font-size: 32rpx;
  margin-top: 40rpx;
  border: none;
}
</style>
