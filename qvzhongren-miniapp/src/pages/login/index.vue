<template>
  <view class="login-page">
    <view class="login-card">
      <view class="logo-section">
        <text class="logo-text">曲终人商城</text>
        <text class="logo-sub">欢迎登录</text>
      </view>
      <view class="form-section">
        <view class="input-group">
          <text class="input-label">用户名</text>
          <input
            class="input-field"
            v-model="form.userName"
            placeholder="请输入用户名"
            placeholder-class="placeholder"
          />
        </view>
        <view class="input-group">
          <text class="input-label">密码</text>
          <input
            class="input-field"
            v-model="form.password"
            type="password"
            placeholder="请输入密码"
            placeholder-class="placeholder"
          />
        </view>
        <button class="login-btn" :loading="loading" @click="handleLogin">登录</button>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { login } from '../../utils/api';

const form = reactive({
  userName: '',
  password: '',
});
const loading = ref(false);

const handleLogin = async () => {
  if (!form.userName || !form.password) {
    uni.showToast({ title: '请填写完整信息', icon: 'none' });
    return;
  }
  loading.value = true;
  try {
    const res = await login({ userName: form.userName, password: form.password });
    const data = res.data;
    uni.setStorageSync('token', data.token);
    uni.setStorageSync('userId', data.userId);
    uni.setStorageSync('userName', data.userName);
    uni.showToast({ title: '登录成功', icon: 'success' });
    setTimeout(() => {
      uni.switchTab({ url: '/pages/index/index' });
    }, 500);
  } catch (e) {
    // handled in request
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #6C5CE7 0%, #a29bfe 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40rpx;
}

.login-card {
  width: 100%;
  background: #fff;
  border-radius: 24rpx;
  padding: 60rpx 40rpx;
  box-shadow: 0 8rpx 32rpx rgba(0, 0, 0, 0.1);
}

.logo-section {
  text-align: center;
  margin-bottom: 60rpx;
}

.logo-text {
  display: block;
  font-size: 44rpx;
  font-weight: bold;
  color: #6C5CE7;
}

.logo-sub {
  display: block;
  font-size: 28rpx;
  color: #999;
  margin-top: 12rpx;
}

.input-group {
  margin-bottom: 30rpx;
}

.input-label {
  display: block;
  font-size: 28rpx;
  color: #333;
  margin-bottom: 12rpx;
}

.input-field {
  width: 100%;
  height: 88rpx;
  border: 2rpx solid #eee;
  border-radius: 16rpx;
  padding: 0 24rpx;
  font-size: 28rpx;
  box-sizing: border-box;
}

.placeholder {
  color: #ccc;
}

.login-btn {
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
