
import { IApiHelper } from "./ApiHelper";
import { ICookieHelper } from "../models/Cookies";
import { IDateHelper } from "../models/DateRange";
import { IDialogHelper } from "../models/Dialogs";
import { IInventoryHelper } from "../models/Inventory";
import { IPluginBuilder } from "./Plugin";
import { IStringHelper } from "../models/StringHelper";

export interface IHelperPlugin {
    apiHelper: IApiHelper;
    cookieHelper: ICookieHelper;
    dateHelper: IDateHelper;
    dialogHelper: IDialogHelper;
    inventoryHelper: IInventoryHelper;
    stringHelper: IStringHelper;
}

export class HelperPluginBuilder implements IHelperPlugin, IPluginBuilder<IHelperPlugin> {
    apiHelper: IApiHelper;
    cookieHelper: ICookieHelper;
    dateHelper: IDateHelper;
    dialogHelper: IDialogHelper;
    inventoryHelper: IInventoryHelper;
    stringHelper: IStringHelper;
    constructor(apiHelper: IApiHelper,
        cookieHelper: ICookieHelper,
        dateHelper: IDateHelper,
        dialogHelper: IDialogHelper,
        inventoryHelper: IInventoryHelper,
        stringHelper: IStringHelper) {
        this.apiHelper = apiHelper;
        this.cookieHelper = cookieHelper;
        this.dateHelper = dateHelper;
        this.dialogHelper = dialogHelper;
        this.inventoryHelper = inventoryHelper;
        this.stringHelper = stringHelper;
    }
    
    build(): IHelperPlugin {
        return this;
    }
}