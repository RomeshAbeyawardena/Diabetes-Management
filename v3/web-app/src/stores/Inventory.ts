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

export const useInventoryStore = defineStore('inventory', {
    state: (): IInventoryStoreState => ({
        isReadonly: false,
        readonlyItems: new Array<IInventory>(),
        items: new Array<IInventory>(),
        lastStoredId: 0,
        isDeleteMode: false
    }),
    getters: {
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
        previousDateItems() : IInventory[] {
            return this.getItemsFromDateRange("subtract", 1, "day");
        },
        currentDateItems() : IInventory[] {
            let items = this.getItemsFromDateRange(null, 1, "day");
            items.sort((a: IInventory, b: IInventory) => Number(a.inputDate) - Number(b.inputDate));
            return items;
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
        async save() : Promise<void> {
            await this.inventoryDb.setItems(this.items);
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
        async saveToFile(): Promise<string>  {
            const value = encode(this.items);
            const output = Buffer.from(value).toString('base64');
            const store = useUserStore()
            const response = await this.inventoryApi.post({
                key: "diabetic.unit.manager",
                type: "export",
                userId: store.userToken,
                items: output
            });
            const resp = JSON.parse(response);
            
            if(resp.data)
            {
                return Buffer.from(resp.data.InventoryHistoryId).toString("base64");
            }

            throw "Invalid input: " + resp.statusMessage;
        }
    } 
});