import { IInventory, Inventory } from '../models/Inventory';
import { defineStore } from 'pinia'
import { State } from '../models/Inventory';
import { useStore } from "../stores/main";
import { encode, decode } from "@msgpack/msgpack";
import { Buffer } from 'buffer';
import { useUserStore } from './User';

export interface IInventoryStoreState {
    isReadonly: boolean,
    items: IInventory[]
    isDeleteMode: boolean,
    lastStoredId: number,
    readonlyItems: IInventory[]
}

export interface IInventoryStoreGetters {

}

const APPLICATION_TYPE_KEY = "diabetic.unit.manager";
const APPLICATION_SUBJECT_TYPE_SAVE = "saveState";
const APPLICATION_SUBJECT_TYPE_SHARE = "export";
const CACHE_KEY_NAME = "versions";

export const useInventoryStore = defineStore('inventory', {
    state: (): IInventoryStoreState => ({
        isReadonly: false,
        readonlyItems: new Array<IInventory>(),
        items: new Array<IInventory>(),
        lastStoredId: 0,
        isDeleteMode: false
    }),
    getters: {
        currentDateItems() : IInventory[] {
            let items = this.getItemsFromDateRange(null, 1, "day");
            items.sort((a: IInventory, b: IInventory) => Number(a.inputDate) - Number(b.inputDate));
            return items;
        },
        currentTotalValue(): number {
            return this.inventoryHelper.getTotalValue(this.currentDateItems);
        },
        getItemsFromDateRange() {
            return function(action: string, value: number, unit: string): IInventory[] {
                const store = useStore();
                const items = this.isReadonly ? this.readonlyItems : this.items;
                
                let dateRange = store.filters.dateRange;
                
                if(action)
                {
                    dateRange = dateRange[action](value, unit);
                }

                if(items.length) {
                    return items.filter((i: IInventory) => i.inputDate >= dateRange.fromDate 
                        && i.inputDate <= dateRange.toDate && i.state !== State.deleted);
                }

                return items;
            };
        },
        lastId(): number {
            if(this.items.length)
            {
                return this.items[this.items.length - 1].id;
            }
            return this.lastStoredId; 
        },
        previousTotalValue(): number {
            return this.inventoryHelper.getTotalValue(this.previousDateItems);
        },
        previousDateItems() : IInventory[] {
            return this.getItemsFromDateRange("subtract", 1, "day");
        }
    },
    actions: {
        addNew(fromDate: Date) : void {
            this.items.push(
                new Inventory(this.lastId + 1, fromDate, "", Number(0), State.added, false));
        },
        async getLastId() : Promise<void> {
            this.lastStoredId = await this.inventoryDb.getLastIndex();
        },
        async load() : Promise<void> {
            this.items = await this.inventoryDb.getItems();
        },
        async loadFromFile(output: string) : Promise<void> {
            const val = Buffer.from(output, 'base64').toString("utf-8");
            const response = await this.inventoryApi.get({
                inventoryHistoryId: val
            });

            const resp = JSON.parse(response);
            if(resp.data) {
                const items = Buffer.from(resp.data.Items, 'base64')
                const decoded = decode<Array<IInventory>>(items);  
                this.readonlyItems = decoded;
                this.isReadonly = true;
            }
        },
        async loadVersions(): Promise<IInventory[]> {
            const versions = this.cacheHelper.get(this.cache, CACHE_KEY_NAME)
            if(versions && versions.length){
                return versions;
            }
            
            const store = useUserStore()
            const response = await this.inventoryApi.list({
                key: APPLICATION_TYPE_KEY,
                type: APPLICATION_SUBJECT_TYPE_SAVE,
                userId: store.userToken,
            });
            
            const resp = JSON.parse(response);
            
            this.cacheHelper.set(this.cache, CACHE_KEY_NAME, resp.data);

            return resp.data;
        },
        async save() : Promise<void> {
            await this.inventoryDb.setItems(this.items);
        },
        async saveInventory(type: string) : Promise<IInventory> {
            const value = encode(this.items);
            const output = Buffer.from(value).toString('base64');
            const store = useUserStore()
            const response = await this.inventoryApi.post({
                key: APPLICATION_TYPE_KEY,
                type: type,
                userId: store.userToken,
                items: output
            });
            const resp = JSON.parse(response);
            
            if(resp.data)
            {
                return resp.data;
            }

            throw "Save failed: " + resp.statusMessage;
        },
        async saveToFile(): Promise<string>  {
            const inventory = await this.saveInventory(APPLICATION_SUBJECT_TYPE_SHARE);
            return Buffer.from(inventory.inventoryHistoryId).toString("base64");
        },
        async saveVersion() : Promise<IInventory> {
            const item = await this.saveInventory(APPLICATION_SUBJECT_TYPE_SAVE);
            this.cache.evict(CACHE_KEY_NAME);
            return item;
        }
    } 
});