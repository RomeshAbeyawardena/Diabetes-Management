import { Cookie } from '../models/Cookies';
import dayjs from 'dayjs';
import { defineStore } from 'pinia'
import { IDialogComponent, IComponent, IConsent, IFilters } from "../models";
import { DialogType } from '../models';
import { Subject } from 'rxjs';

const cancelDialogOption = "dialog.cancel";

export interface IMainStoreState {
    consent: IConsent;
    dialog: IDialogComponent,
    filters?: IFilters;
    sideBar: IComponent,
    showWelcome: boolean;
}


export const useStore = defineStore('main', {
    state: (): IMainStoreState => ({
        consent: {
            enableMarketing: true,
            enableNecessary: true,
            hasConsented: false
        },
        dialog: {
            name: "",
            showControls: true,
            title: "",
            type: DialogType.None,
            value: new Object(),
            valueSubject: new Subject<any>(),
            visible: false
        },
        sideBar: {
            name: "Guest",
            title: "",
            type: DialogType.None,
            visible: false
        },
        showWelcome: false
    }),
    getters: {

    },
    actions: {
        addDialog(dialog) {
          return this.dialogHelper.addDialog(dialog);
        },
        setFilterDateRange(fromDate: Date, toDate: Date) {
          let dateRange = this.dateHelper.dateRange(fromDate, toDate, true);
          
          if(!this.filters){
            this.filters = {
              dateRange: dateRange
            };
          }
          else
            this.filters.dateRange = dateRange;
        },
        getDialog(dialogType) {
          return this.dialogHelper.getDialog(dialogType);
        },
        setConsent() {
          this.cookieHelper.setCookie(new Cookie("CP_end_user.consent", JSON.stringify(this.consent), dayjs().add(1, "year").toDate()));
        },
        getConsent() {
          let cookie = this.cookieHelper.getCookie("CP_end_user.consent");
          
          if(cookie && cookie.value) {
            this.consent = JSON.parse(cookie.value);
          }
        },
        resetDialog() {
          this.dialog.component = "";
          this.dialog.title = "";
          this.dialog.value = "";
          this.dialog.visible = false;
        },
        setDialogValue(value) {
          this.dialog.value = value;
          this.dialog.valueSubject.next(value);
        },
        voidDialogValue() {
          this.setDialogValue(cancelDialogOption);
        },
        showDialog(component, value, showControls) {
          this.dialog.component = component;
          this.dialog.title = this.getDialog(component).type;
          this.dialog.value = value;
          this.dialog.visible = true;
  
          if(showControls !== undefined) {
            this.dialog.showControls= showControls;
          }
  
          return new Promise((resolve) => {
            let subscriber = this.dialog.valueSubject.asObservable().subscribe(a => {
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
});