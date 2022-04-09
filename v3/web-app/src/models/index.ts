import { IDateRange } from './DateRange';
import { Subject } from 'rxjs';

export enum DialogType {
    //dialogs
    CookiePolicy = "cookie-policy",
    DatePicker = "date-picker",
    LocalExport = "local-export",
    Login = "login",
    Register = "register",
    TextEntry = "text-entry",
    None = "none",
    NumberPicker = "number-picker",
    //side bars
    Authenticated = "authenticated",
    Guest = "guest"
}

export interface IFilters {
    dateRange: IDateRange;
}

export interface IConsent {
    hasConsented: boolean;
    enableNecessary: boolean;
    enableMarketing: boolean;
}

export interface IComponent {
    name: string;
    title: string;
    type: DialogType;
    visible: boolean;
    component?: any;
}

export interface IDialogComponent extends IComponent {
    showControls: boolean;
    value: any;
    valueSubject: Subject<any>;
}