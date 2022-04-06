import { IInventory, Inventory } from '../models/Inventory';
import { defineStore } from 'pinia'
import { State } from '../models/Inventory';
import { useStore } from "../stores/main";

export interface IInventoryStoreState {
    items: IInventory[]
    lastStoredId: number,
    isDeleteMode: boolean
}

export interface IInventoryStoreGetters {

}

export const useInventoryStore = defineStore('inventory', {
    state: (): IInventoryStoreState => ({
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
                let store = useStore();

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
        async getLastId() : Promise<void> {
            this.lastStoredId = await this.inventoryDb.getLastIndex();
        },
        addNew(fromDate: Date) : void {
            this.items.push(
                new Inventory(this.lastId + 1, fromDate, "", Number(0), State.added, false));
        },
        async load() : Promise<void> {
            this.items = await this.inventoryDb.getItems();
        }, 
        async save() : Promise<void> {
            await this.inventoryDb.setItems(this.items);
        }
    } 
});