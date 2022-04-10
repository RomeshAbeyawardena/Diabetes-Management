import { IApiHelper } from "../plugins/ApiHelper";
import { IPluginBuilder } from "../plugins/Plugin";
import { IApiDefinitions } from "./Definition";
import { IInventoryApi, InventoryApi } from "./Inventory"; 

export interface IApiPlugin {
    apiDefinitions: IApiDefinitions;
    inventoryApi: IInventoryApi
}

export class ApiPlugin implements IApiPlugin, IPluginBuilder<IApiPlugin> {
    apiDefinitions: IApiDefinitions;
    inventoryApi: IInventoryApi;

    constructor(apiHelper: IApiHelper, apiDefinitions: IApiDefinitions) {
        this.apiDefinitions = apiDefinitions;
        this.inventoryApi = new InventoryApi(apiHelper, apiDefinitions.inventory);
    }

    build(): IApiPlugin {
        return this;
    }

}