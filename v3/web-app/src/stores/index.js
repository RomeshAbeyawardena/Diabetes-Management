import { defineStore } from 'pinia';
import { DateRange } from '../models/DateRange';
import { Subject } from 'rxjs';
import Promise from "promise";

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
          itemSubject: new Subject(),
          component: "", 
          title: "",
          value: "",
          visible: false
        },
        filters: {
          dateRange: new DateRange(Date(), Date(), true) 
        }
      }
    },
    actions: {
      resetDialog() {
        this.dialog.component = "";
        this.dialog.title = "";
        this.dialog.value = "";
        this.dialog.visible = false;
      },
      showDialog(component, title, value) {
        this.dialog.component = component;
        this.dialog.title = title;
        this.dialog.value = value;
        this.dialog.visible = true;
        return new Promise((resolve) => {
          let subscriber = this.dialog.itemSubject.asObservable().subscribe(a => {
            if(a !== "dialog.cancel")
            {
              resolve(a);
            }
            subscriber.unsubscribe();
            subscriber.remove();
            this.resetDialog();
          });
        }); 
      }
    }
  })