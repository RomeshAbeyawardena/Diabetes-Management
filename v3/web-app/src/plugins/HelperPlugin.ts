import { ICookieHelper } from "../models/Cookies";
import { IDateHelper } from "../models/DateRange";
import { IDialogHelper } from "../models/Dialogs";
import { IInventoryHelper } from "../models/Inventory";
import { IPluginBuilder } from "./Plugin";

export interface IHelperPlugin {
    cookieHelper: ICookieHelper;
    dateHelper: IDateHelper;
    dialogHelper: IDialogHelper;
    inventoryHelper: IInventoryHelper;
}

export class HelperPluginBuilder implements IHelperPlugin, IPluginBuilder<IHelperPlugin> {
    cookieHelper: ICookieHelper;
    dateHelper: IDateHelper;
    dialogHelper: IDialogHelper;
    inventoryHelper: IInventoryHelper;

    constructor(cookieHelper: ICookieHelper,
        dateHelper: IDateHelper,
        dialogHelper: IDialogHelper,
        inventoryHelper: IInventoryHelper) {
        
        this.cookieHelper = cookieHelper;
        this.dateHelper = dateHelper;
        this.dialogHelper = dialogHelper;
        this.inventoryHelper = inventoryHelper;
    }
    
    build(): IHelperPlugin {
        return this;
    }
}