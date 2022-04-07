import Dexie from "dexie";
import { State, IInventory } from "../models/Inventory";

export interface IInventoryDatabase {
    items: Dexie.Table<IInventory, number>;
}

export interface IInventoryDb {
    inventoryDatabase: IInventoryDatabase;
    getItems(fromDate: Date, toDate: Date) : Promise<IInventory[]>;
    getLastIndex() : Promise<number>;
    rebuild() : Promise<void>;
    searchItems(query: string) : Promise<IInventory[]>;
    setItems(items: IInventory[]) : Promise<void>;
    sync() : Promise<void>;
}

export class InventoryDatabase extends Dexie implements IInventoryDatabase {
    items: Dexie.Table<IInventory, number>;

    constructor() {
        super("inventory");
        this.version(1).stores({
            items: 'id, description, value, inputDate, state, published'
        });
    }
}

export class InventoryDb implements IInventoryDb {
    inventoryDatabase: IInventoryDatabase;
    constructor(inventoryDatabase: IInventoryDatabase) {
        this.inventoryDatabase = inventoryDatabase;
    }
    async getItems(fromDate: Date, toDate: Date): Promise<IInventory[]> {
        const query = this.inventoryDatabase.items
            .where("state")
            .notEqual(State.deleted);
            
        
        if (!fromDate && !toDate) {
            return await query.toArray();
        }

        return await query.and(item => item.inputDate >= fromDate && item.inputDate <= toDate).toArray();
    }
    async getLastIndex(): Promise<number> {
        const lastItem = await this.inventoryDatabase.items.orderBy("id").last();
        return lastItem?.id ?? 0;
    }
    async rebuild(): Promise<void> {
        // await this.destroy();
        // this.connection = null;
        // window.location.reload();
    }
    async searchItems(query: string): Promise<IInventory[]> {
        const collection = this.inventoryDatabase.items;
        return await collection.where("description").startsWith(query).toArray();
    }
    async setItems(items: IInventory[]): Promise<void> {
        const collection = this.inventoryDatabase.items;
        
        let itemsToPush = new Array<IInventory>();

        for(let item of items) 
        {
            if(item.state === State.added) {
                item.state = State.modified;
            }

            //don't push items marked as deleted that haven't been published
            if(item.state === State.deleted && !item.published) {
                continue;
            }

            if(!item.published) {
                item.published = true;
            }
            
            itemsToPush.push({ ...item } );
        }
        
        await collection.bulkPut(itemsToPush, { allKeys: false });
    }
    sync(): Promise<void> {
        throw new Error("Method not implemented.");
    }
}