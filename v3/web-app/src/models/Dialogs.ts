export interface IDialog {
    name: string;
    title: string;
    type: DialogType;
    component: any;
}

export interface IDialogHelper {
    dialogs : IDialog[];
    getDialog(type: DialogType) : IDialog
    addDialog(dialog: IDialog) : IDialogHelper;
}

export enum DialogType {
    CookiePolicy = "cookie-policy",
    DatePicker = "date-picker",
    Login = "login",
    Register = "register",
    TextEntry = "text-entry",
    NumberPicker = "number-picker"
}

export class DialogDef implements IDialog {
    name: string;
    title: string;
    type: DialogType;
    component: any;
    constructor(type: DialogType, name: string, title: string, component: any) {
        this.name = name;
        this.title = title;
        this.type = type;
        this.component = component;
    }
}

export class DialogHelper implements IDialogHelper {
    dialogs: IDialog[];

    constructor() {
        this.dialogs = new Array<IDialog>()
    }

    getDialog(type: DialogType): IDialog {
        return this.dialogs.find(d => d.type === type);
    }

    addDialog(dialog: IDialog) : IDialogHelper {
        const index = this.dialogs.findIndex(d => d.type === dialog.type);
        if(index !== -1) {
            this.dialogs.splice(index, 1);
        }
        this.dialogs.push(dialog);
        return this
    }
}