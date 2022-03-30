import Vue from 'vue';
import Vuex from 'vuex';
import Inventory from "./inventory";
Vue.use(Vuex)

export const Store = {
  mutations: {
    toggleSideBar: "toggleSidebar",
    setDialogOptions: "setDialogOptions"
  }
}

export default new Vuex.Store({
  state: {
    sideBar: {
      display: false
    },
    dialog: {
      header: null,
      display: false,
      value: null,
      modal: true,
      subject: null,
      showTime: false,
      type: null
    }
  },
  mutations: {
    setDialogOptions(state, dialogOptions) {
      Vue.set(state, "dialog", dialogOptions);
    },
    toggleSideBar(state, display){
      state.sideBar.display = display;
    }
  },
  actions: {
    
  },
  modules: {
    Inventory
  }
})
