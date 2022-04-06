import { IComponent, DialogType } from "../models";

export interface IDialogHelper {
    dialogs : IComponent[];
    getDialog(type: DialogType) : IComponent
    addDialog(dialog: IComponent) : IDialogHelper;
}

export class DialogDef implements IComponent {
    name: string;
    title: string;
    type: DialogType;
    component: any;
    visible: boolean;

    constructor(type: DialogType, name: string, title: string, component: any, visible: boolean) {
        this.name = name;
        this.title = title;
        this.type = type;
        this.component = component;
        this.visible = visible;
    }
}

export class DialogHelper implements IDialogHelper {
    dialogs: IComponent[];

    constructor() {
        this.dialogs = new Array<IComponent>()
    }

    getDialog(type: DialogType): IComponent {
        return this.dialogs.find(d => d.type === type);
    }

    addDialog(dialog: IComponent) : IDialogHelper {
        const index = this.dialogs.findIndex(d => d.type === dialog.type);
        if(index !== -1) {
            this.dialogs.splice(index, 1);
        }
        this.dialogs.push(dialog);
        return this
    }
}