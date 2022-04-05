import { defineStore } from 'pinia';
import { DateRange } from '../models/DateRange';
import { Subject } from 'rxjs';
import Promise from "promise";

const cancelDialogOption = "dialog.cancel";

export const useStore = defineStore('main', {
    state:() => {
      return {
        showWelcome: false,
        consent: {
          hasConsented: false,
          enableNecessary: true,
          enableMarketing: true
        },
        sideBar: {
          component: "Guest",
          title: "",
          visible: false
        },
        dialog: {
          showControls: true,
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
      setDialogValue(value) {
        this.dialog.value = value;
        this.dialog.itemSubject.next(value);
      },
      voidDialogValue() {
        this.setDialogValue(cancelDialogOption);
      },
      showDialog(component, title, value, showControls) {
        this.dialog.component = component;
        this.dialog.title = title;
        this.dialog.value = value;
        this.dialog.visible = true;

        if(showControls !== undefined) {
          this.dialog.showControls= showControls;
        }

        return new Promise((resolve) => {
          let subscriber = this.dialog.itemSubject.asObservable().subscribe(a => {
            if(a !== cancelDialogOption)
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