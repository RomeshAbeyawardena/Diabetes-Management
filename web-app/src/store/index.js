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
    clientSize: {
      width: 0,
      height: 0
    }
  },
  mutations: {
    setClientSize(state, clientSize){
      state.clientSize = clientSize;
    }
  },
  actions: {
    
  },
  modules: {
    Inventory
  }
})
