import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  base: '/Abilities/',  // Replace 'repository-name' with your actual repository name
});
