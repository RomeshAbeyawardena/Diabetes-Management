import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config';
import App from './App.vue'
import InventoryDb from "./db/inventory";
import ConfirmationService from 'primevue/confirmationservice';
import ToastService from 'primevue/toastservice';

import "primevue/resources/themes/bootstrap4-dark-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";
import "./scss/index.scss";

function inventoryDbPlugin() {
    return { inventoryDb: InventoryDb };
}

let pinia = createPinia()
    .use(inventoryDbPlugin);

createApp(App)
    .use(pinia)
    .use(PrimeVue) 
    .use(ConfirmationService)
    .use(ToastService)
    .mount('#app');