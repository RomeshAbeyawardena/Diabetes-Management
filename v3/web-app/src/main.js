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
import { MessageClientPlugin } from './models/MessageClients/Setup';
import { HelperPluginBuilder } from './plugins/HelperPlugin';

import "primevue/resources/themes/bootstrap4-dark-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";
import "./scss/index.scss";
import { DbPluginBuilder } from './plugins/DbPlugin';

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

function messageClientPlugin() {
    return new MessageClientPlugin().build();
}

let pinia = createPinia()
    .use(helperPlugin)
    .use(dbPlugin)
    .use(messageClientPlugin);

createApp(App)
    .use(pinia)
    .use(PrimeVue) 
    .use(ConfirmationService)
    .use(ToastService)
    .mount('#app');