import { IApiHelper } from "../plugins/ApiHelper";
import { IPluginBuilder } from "../plugins/Plugin";
import { IApiDefinitions } from "./Definition";
import { IInventoryApi, InventoryApi } from "./Inventory"; 
import { IUserApi, UserApi } from "./User";

export interface IApiPlugin {
    apiDefinitions: IApiDefinitions;
    inventoryApi: IInventoryApi
    userApi: IUserApi;
}

export class ApiPlugin implements IApiPlugin, IPluginBuilder<IApiPlugin> {
    apiDefinitions: IApiDefinitions;
    inventoryApi: IInventoryApi;
    userApi: IUserApi;
    constructor(apiHelper: IApiHelper, apiDefinitions: IApiDefinitions) {
        this.apiDefinitions = apiDefinitions;
        this.inventoryApi = new InventoryApi(apiHelper, apiDefinitions.inventory);
        this.userApi = new UserApi(apiHelper, apiDefinitions.inventory);
    }

    build(): IApiPlugin {
        return this;
    }

}