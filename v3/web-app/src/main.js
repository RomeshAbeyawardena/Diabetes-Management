import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config';
import App from './App.vue'
import { InventoryDatabase, InventoryDb  } from "./db/Inventory";
import ConfirmationService from 'primevue/confirmationservice';
import ToastService from 'primevue/toastservice';
import { CookieHelper } from './models/Cookies';
import { InventoryHelper } from './models/Inventory';
import { DialogHelper } from './models/Dialogs';
import { DateHelper } from './models/DateRange';
import { HelperPluginBuilder } from './plugins/HelperPlugin';

import "primevue/resources/themes/bootstrap4-dark-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";
import "./scss/index.scss";
import { DbPluginBuilder } from './plugins/DbPlugin';

const debugServiceWorker = false;

if(debugServiceWorker || import.meta.env.PROD)
{
    console.log("Production: using service worker -- remove this before next deployment")
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('./sw.js').then(function(reg) {
            console.log('Successfully registered service worker', reg);
        }).catch(function(err) {
            console.warn('Error whilst registering service worker', err);
        });
    }
}
else {
    console.warn("Non-production: service worker has been disabled, to debug the service worker, set debugServiceWorker = true")
}

function helperPlugin() {
    return new HelperPluginBuilder(
        new CookieHelper(), 
        new DateHelper(),
        new DialogHelper(),
        new InventoryHelper()
    ).build();
}

function dbPlugin() {
    return new DbPluginBuilder(
        new InventoryDb(new InventoryDatabase())
    ).build();
}

let pinia = createPinia()
    .use(helperPlugin)
    .use(dbPlugin);

createApp(App)
    .use(pinia)
    .use(PrimeVue) 
    .use(ConfirmationService)
    .use(ToastService)
    .mount('#app');