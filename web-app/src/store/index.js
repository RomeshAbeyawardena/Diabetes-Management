import Vue from 'vue';
import Vuex from 'vuex';
import Inventory from "./inventory";
Vue.use(Vuex)

export const Store = {
  mutations: {
    setClientSize: "setClientSize"
  }
}

export default new Vuex.Store({
  state: {
    
  },
  mutations: {
    
  },
  actions: {
    
  },
  modules: {
    Inventory
  }
})
