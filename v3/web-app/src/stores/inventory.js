import { defineStore } from 'pinia'
import { Inventory, State } from '../models/Inventory';
import { useStore } from "../stores";

export const useInventoryStore = defineStore('inventory', {
    state:() => {
        return {
            items: [],
            lastStoredId: 0,
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
            let fromDate = store.filters.dateRange.fromDate;
            let toDate = store.filters.dateRange.toDate;
            if(this.items.length) {
                return this.items.filter(i => i.inputDate >= fromDate && i.inputDate <= toDate);
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
        async save() {
            await this.inventoryDb.setItems(this.items);
        }
    }
});