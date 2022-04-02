import { defineStore } from 'pinia'

export const useStore = defineStore('main', {
    state:() => {
      return {
        showWelcome: false,
        sideBar: {
          component: "Guest",
          title: "",
          visible: false
        },
        dialog: {
          component: "", 
          title: "",
          value: Object(),
          visible: false
        }
      }
    }
  })