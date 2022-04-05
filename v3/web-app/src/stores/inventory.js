import { defineStore } from 'pinia'
import { Inventory, InventoryHelper, State } from '../models/Inventory';
import { useStore } from "../stores";

export const useInventoryStore = defineStore('inventory', {
    state:() => {
        return {
            items: [],
            lastStoredId: 0,
            isDeleteMode: false
        }
    },
    getters: {
        lastId() { 
            if(this.items.length)
            {
                return this.items[this.items.length - 1].id;
            }
            return this.lastStoredId; 
        },
        previousTotalValue() {
            return new InventoryHelper().getTotalValue(this.previousDateItems);
        },
        currentTotalValue() {
            return new InventoryHelper().getTotalValue(this.currentDateItems);
        },
        getItemsFromDateRange() {
            return (action, value, unit) => {
                let store = useStore();

                let dateRange = store.filters.dateRange;
                
                if(action)
                {
                    dateRange = dateRange[action](value, unit);
                }

                if(this.items.length) {
                    return this.items.filter(i => i.inputDate >= dateRange.fromDate 
                        && i.inputDate <= dateRange.toDate && i.state !== State.deleted);
                }

                return this.items;
            }
        },
        previousDateItems() {
            return this.getItemsFromDateRange("subtract", 1, "day");
        },
        currentDateItems() {
            let items = this.getItemsFromDateRange(null, 1, "day");
            items.sort((a,b) => Number(a.inputDate) - Number(b.inputDate));
            return items;
        }
    },
    actions: {
        async getLastId() {
            this.lastStoredId = await this.inventoryDb.getLastIndex();
        },
        addNew(fromDate) {
            this.items.push(
                new Inventory(this.lastId + 1, fromDate, "", Number(0), State.added, false));
        },
        async load() {
            let items = await this.inventoryDb.getItems();
            this.items = items.map(i => new Inventory().fromObject(i));
        },
        async save() {
            await this.inventoryDb.setItems(this.items.map(i => i.toObject()));
        }
    }
});