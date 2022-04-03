import { defineStore } from 'pinia';
import { DateRange } from '../models/DateRange';

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
        },
        filters: {
          dateRange: new DateRange(Date(), Date(), true) 
        }
      }
    }
  })