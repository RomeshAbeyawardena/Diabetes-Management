import Vue from 'vue'
import App from './App.vue'
import store from './store'
import PrimeVue from 'primevue/config';
import ConfirmationService from 'primevue/confirmationservice';
import ToastService from 'primevue/toastservice';
import Tooltip from 'primevue/tooltip';
require("/node_modules/primeflex/primeflex.css");
require("primevue/resources/themes/bootstrap4-dark-blue/theme.css");       //theme
require("primevue/resources/primevue.min.css");                 //core css
require("primeicons/primeicons.css");  //icons
require("./scss/main.scss"); 

Vue.config.productionTip = false;
Vue.use(PrimeVue, {ripple: true});
Vue.use(ConfirmationService);
Vue.use(ToastService);

Vue.directive('tooltip', Tooltip);
new Vue({
  store,
  render: h => h(App)
}).$mount('#app')
