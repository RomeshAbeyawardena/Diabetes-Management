import { IInventory, Inventory } from '../models/Inventory';
import { defineStore } from 'pinia'
import { State } from '../models/Inventory';
import { useStore } from "../stores/main";
import { encode, decode } from "@msgpack/msgpack";
import { Buffer } from 'buffer';

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

                if(this.items.length) {
                    return this.items.filter((i: IInventory) => i.inputDate >= dateRange.fromDate 
                        && i.inputDate <= dateRange.toDate && i.state !== State.deleted);
                }

                return this.items;
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
        loadFromFile(output: string) : void {
            const val = Buffer.from(output, 'base64');
            const decoded = decode<Array<IInventory>>(val);  
            this.readonlyItems = decoded;
            this.isReadonly = true;
        },
        async saveToFile(): Promise<string>  {
            const value = encode(this.items);
            const output = Buffer.from(value).toString('base64');
            
            const response = await this.inventoryApi.post({
                key: "diabetic.unit.manager",
                type: "export",
                userId: "c55d2555-dc9a-4583-ada2-d68f9c21b184",
                items: output
            });
            const payload = JSON.parse(response);
            console.log(payload);
            return payload.InventoryHistoryId;
        }
    } 
});