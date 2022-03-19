import Vue from 'vue';
import Vuex from 'vuex';
import Inventory from "./inventory";
Vue.use(Vuex)

export const Store = {
  mutations: {
    setDialogOptions: "setDialogOptions"
  }
}

export default new Vuex.Store({
  state: {
    dialog: {
      header: null,
      display: false,
      value: null,
      modal: true,
      subject: null
    }
  },
  mutations: {
    setDialogOptions(state, dialogOptions) {
      Vue.set(state, "dialog", dialogOptions);
    }
  },
  actions: {
    
  },
  modules: {
    Inventory
  }
})
