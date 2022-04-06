import { IDateRange } from './DateRange';
import { Subject } from 'rxjs';

export enum DialogType {
    None = "none",
    CookiePolicy = "cookie-policy",
    DatePicker = "date-picker",
    Login = "login",
    Register = "register",
    TextEntry = "text-entry",
    NumberPicker = "number-picker"
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
}

export interface IDialogComponent extends IComponent {
    showControls: boolean;
    value: any;
    valueSubject: Subject<any>;
}