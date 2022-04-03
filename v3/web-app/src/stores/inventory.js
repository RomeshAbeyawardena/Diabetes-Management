import { defineStore } from 'pinia'
import { Inventory, State } from '../models/Inventory';
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
        currentDateItems() {
            let store = useStore();
            let dateRange = store.filters.dateRange;
            
            if(this.items.length) {
                return this.items.filter(i => i.inputDate >= dateRange.fromDate 
                    && i.inputDate <= dateRange.toDate);
            }
        }
    },
    actions: {
        async getLastId() {
            this.lastStoredId = await this.inventoryDb.getLastIndex();
        },
        addNew(fromDate) {
            this.items.push(
                new Inventory(this.lastId + 1, fromDate, "", Number(0), State.added));
        },
        async load() {
            this.items = await this.inventoryDb.getItems();
        },
        async save() {
            await this.inventoryDb.setItems(this.items);
        }
    }
});