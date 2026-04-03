// H5 模式用相对路径走 vite proxy，小程序模式用绝对地址
// #ifdef H5
const BASE_URL = '/api';
// #endif
// #ifndef H5
const BASE_URL = 'http://localhost:8096/api';
// #endif

interface ApiResult<T> {
  isSuccess: boolean;
  code: number;
  message: string;
  data: T;
}

export const request = <T>(url: string, data?: any, method: 'POST' | 'GET' = 'POST'): Promise<ApiResult<T>> => {
  // 对纯字符串参数，需要手动 JSON 包裹（后端 [FromBody] string 需要 JSON 格式的字符串）
  let body: any;
  if (data === undefined || data === null) {
    body = {};
  } else if (typeof data === 'string') {
    // 纯字符串 -> 用 JSON.stringify 包裹成 "value"，并作为原始字符串发送
    body = JSON.stringify(data);
  } else {
    body = data;
  }

  return new Promise((resolve, reject) => {
    // 如果 body 是被 stringify 过的字符串（如 '"abc"'），H5 模式下用 fetch 发送
    // #ifdef H5
    if (typeof body === 'string') {
      fetch(`${BASE_URL}${url}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${uni.getStorageSync('token') || ''}`,
        },
        body: body,
      })
        .then(async (res) => {
          if (res.status === 401) {
            reject(new Error('未登录'));
            return;
          }
          const result = await res.json() as ApiResult<T>;
          if (result && result.isSuccess !== undefined && !result.isSuccess) {
            uni.showToast({ title: result.message || '请求失败', icon: 'none' });
            reject(new Error(result.message));
            return;
          }
          resolve(result);
        })
        .catch((err) => {
          console.error('Request failed:', url, err);
          uni.showToast({ title: '网络错误', icon: 'none' });
          reject(err);
        });
      return;
    }
    // #endif

    uni.request({
      url: `${BASE_URL}${url}`,
      method,
      data: body,
      header: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${uni.getStorageSync('token') || ''}`,
      },
      success: (res: any) => {
        if (res.statusCode === 401) {
          reject(new Error('未登录'));
          return;
        }
        const result = res.data as ApiResult<T>;
        if (result && result.isSuccess !== undefined) {
          if (!result.isSuccess) {
            uni.showToast({ title: result.message || '请求失败', icon: 'none' });
            reject(new Error(result.message));
            return;
          }
          resolve(result);
        } else {
          resolve({ isSuccess: true, code: 200, message: '', data: res.data } as ApiResult<T>);
        }
      },
      fail: (err) => {
        console.error('Request failed:', url, err);
        uni.showToast({ title: '网络错误', icon: 'none' });
        reject(err);
      },
    });
  });
};

export default request;
