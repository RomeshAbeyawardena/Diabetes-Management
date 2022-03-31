import Vue from 'vue';
import Vuex from 'vuex';
import Inventory from "./inventory";
import User from "./user";
Vue.use(Vuex)

export const Store = {
  mutations: {
    setSideBarDisplay: "setSideBarDisplay",
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
    setSideBarDisplay(state, display){
      state.sideBar.display = display;
    }
  },
  actions: {
    
  },
  modules: {
    Inventory,
    User
  }
})
