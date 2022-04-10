
import { IApiHelper } from "./ApiHelper";
import { ICookieHelper } from "../models/Cookies";
import { IDateHelper } from "../models/DateRange";
import { IDialogHelper } from "../models/Dialogs";
import { IInventoryHelper } from "../models/Inventory";
import { IPluginBuilder } from "./Plugin";

export interface IHelperPlugin {
    apiHelper: IApiHelper;
    cookieHelper: ICookieHelper;
    dateHelper: IDateHelper;
    dialogHelper: IDialogHelper;
    inventoryHelper: IInventoryHelper;
}

export class HelperPluginBuilder implements IHelperPlugin, IPluginBuilder<IHelperPlugin> {
    apiHelper: IApiHelper;
    cookieHelper: ICookieHelper;
    dateHelper: IDateHelper;
    dialogHelper: IDialogHelper;
    inventoryHelper: IInventoryHelper;

    constructor(apiHelper: IApiHelper,
        cookieHelper: ICookieHelper,
        dateHelper: IDateHelper,
        dialogHelper: IDialogHelper,
        inventoryHelper: IInventoryHelper) {
        this.apiHelper = apiHelper;
        this.cookieHelper = cookieHelper;
        this.dateHelper = dateHelper;
        this.dialogHelper = dialogHelper;
        this.inventoryHelper = inventoryHelper;
    }
    
    build(): IHelperPlugin {
        return this;
    }
}