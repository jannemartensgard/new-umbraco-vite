import { defineConfig } from 'vite'
import liveReload from 'vite-plugin-live-reload'

export default defineConfig({
    base: '/dist',
    build: {
        outDir: '../wwwroot/dist',
        emptyOutDir: true,
        manifest: true,
        rollupOptions: {
            input: {
                index: './src/js/index.ts',
            }
        }
    },
    plugins:[
        liveReload('../Views/**/*', { alwaysReload: true } )
    ],
    server: {
        host: true,
        hmr: {
            protocol: 'ws'
        },
        port: 3000,
        strictPort: false
    }
})