import { defineConfig } from "vite";
import uni from "@dcloudio/vite-plugin-uni";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [uni()],
  server: {
    port: 5174,
    host: true,
    proxy: {
      '/api': {
        target: 'http://localhost:8096',
        changeOrigin: true,
      },
      '/uploads': {
        target: 'http://localhost:8096',
        changeOrigin: true,
      },
    },
  },
});
