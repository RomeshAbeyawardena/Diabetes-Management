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
import { DbPluginBuilder } from './plugins/DbPlugin';
import { ApiPlugin } from './api/plugin';
import { ApiHelper } from './plugins/ApiHelper';
import { StringHelper } from './models/StringHelper';

import "primevue/resources/themes/bootstrap4-dark-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";
import "./scss/index.scss";

const appElement = document.getElementById("app");

const apiDefinition = atob(appElement.dataset.apiDefinition);
appElement.dataset.apiDefinition = null;

const apiHelper = new ApiHelper();

function helperPlugin() {
    return new HelperPluginBuilder(
        apiHelper,
        new CookieHelper(), 
        new DateHelper(),
        new DialogHelper(),
        new InventoryHelper(),
        new StringHelper()
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

function apiPlugin() {
    const apiDefinitions = JSON.parse(apiDefinition);
    const apiPlugin = new ApiPlugin(apiHelper, apiDefinitions)
        .build();
    return apiPlugin;
}

let pinia = createPinia()
    .use(helperPlugin)
    .use(dbPlugin)
    .use(messageClientPlugin)
    .use(apiPlugin);

createApp(App)
    .use(pinia)
    .use(PrimeVue) 
    .use(ConfirmationService)
    .use(ToastService)
    .mount('#app');