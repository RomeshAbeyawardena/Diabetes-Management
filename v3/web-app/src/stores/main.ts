import { Cookie } from '../models/Cookies';
import dayjs from 'dayjs';
import { defineStore } from 'pinia'
import { IDialogComponent, IComponent, IConsent, IFilters } from "../models";
import { DialogType } from '../models';
import { Subject } from 'rxjs';

const cancelDialogOption = "dialog.cancel";

export interface IMainStoreState {
    blockEvents: boolean,
    consent: IConsent;
    dialog: IDialogComponent,
    filters?: IFilters;
    sideBar: IComponent,
    showWelcome: boolean;
}

export const useStore = defineStore('main', {
    state: (): IMainStoreState => ({
        blockEvents: false,
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
            type: DialogType.Guest,
            visible: false
        },
        showWelcome: false
    }),
    getters: {

    },
    actions: {
        addDialog(dialog: IDialogComponent) {
          return this.dialogHelper.addDialog(dialog);
        },
        getConsent() {
          let cookie = this.cookieHelper.getCookie("CP_end_user.consent");
          
          if(cookie && cookie.value) {
            this.consent = JSON.parse(cookie.value);
          }
        },
        getDialog(dialogType: DialogType) {
          return this.dialogHelper.getDialog(dialogType);
        },
        resetDialog() {
          this.dialog.component = "";
          this.dialog.title = "";
          this.dialog.value = "";
          this.dialog.visible = false;
        },
        setConsent() {
          this.cookieHelper.setCookie(new Cookie("CP_end_user.consent", JSON.stringify(this.consent), dayjs().add(1, "year").toDate()));
        },
        setDialogValue(value: any) {
          this.dialog.value = value;
          this.dialog.valueSubject.next(value);
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
        showDialog(dialog: IDialogComponent, value: any, showControls: boolean) {
          this.dialog.component = dialog.component;
          this.dialog.title = dialog.title;
          this.dialog.value = value;
          this.dialog.visible = true;
  
          if(showControls !== undefined) {
            this.dialog.showControls= showControls;
          }
  
          return new Promise((resolve) => {
            let subscriber = this.dialog.valueSubject.asObservable().subscribe((a:any) => {
              if(a !== cancelDialogOption)
              {
                resolve(a);
              }
              else {
                resolve(undefined);
              }
              subscriber.unsubscribe();
              subscriber.remove();
              this.resetDialog();
            });
          }); 
        },
        showSidebar(dialog: IComponent) {
          this.sideBar.component = dialog.component;
          this.sideBar.title = dialog.type;
          this.sideBar.visible = true;
        },
        voidDialogValue() {
          this.setDialogValue(cancelDialogOption);
        },
        
      } 
});