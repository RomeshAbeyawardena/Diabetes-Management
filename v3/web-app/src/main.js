import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config';
import App from './App.vue'
import InventoryDb from "./db/inventory";

import "primevue/resources/themes/bootstrap4-dark-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";
import "./scss/index.scss";

InventoryDb.getDbConnection();

function inventoryDbPlugin() {
    return { inventoryDb: InventoryDb };
}

let pinia = createPinia()
    .use(inventoryDbPlugin);

createApp(App)
    .use(pinia)
    .use(PrimeVue) 
    .mount('#app');