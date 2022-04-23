import { createPinia } from 'pinia'
import { createApp } from 'vue'
import App from './App.vue'
import { useAppServices } from './services/main';

const pinia = createPinia();

const appServices = function() {
    return useAppServices({
        baseURL: "http://localhost:7071/api",
        headers: {
            "x-api-acc-token": ""
        }
    });
}

createApp(App)
    .use(pinia.use(appServices))
    .mount('#app');
