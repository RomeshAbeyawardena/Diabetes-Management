import { IInventoryDb } from "../db/Inventory";
import { IPluginBuilder } from "./Plugin";

export interface IDbPlugin {
    inventoryDb: IInventoryDb
}

export class DbPluginBuilder implements IDbPlugin, IPluginBuilder<IDbPlugin> {
    inventoryDb: IInventoryDb;
    
    constructor(inventoryDb: IInventoryDb) {
        this.inventoryDb = inventoryDb;
    }

    build(): IDbPlugin {
        return this;
    }
}