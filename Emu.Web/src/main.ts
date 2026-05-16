import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from '@/app/App.vue'
import router from '@/app/router'
import '@/app/styles/main.css'
import '@/app/styles/liquid.css'
import '@/app/styles/animations.css'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
