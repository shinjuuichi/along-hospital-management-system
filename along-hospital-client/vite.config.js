import react from '@vitejs/plugin-react'
import * as fs from 'fs'
import * as path from 'path'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
	server: {
		port: 3000,
		https: {
			key: fs.readFileSync(path.resolve(__dirname, 'localhost+2-key.pem')),
			cert: fs.readFileSync(path.resolve(__dirname, 'localhost+2.pem')),
		},
	},
	plugins: [react()],
	resolve: {
		alias: [{ find: '@', replacement: path.resolve(__dirname, 'src') }],
	},
})
