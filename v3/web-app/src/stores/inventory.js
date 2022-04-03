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
        previousTotalValue() {
            let sum = 0;
            for(let item of this.previousDateItems) {
                sum += item.value;
            }

            return sum;
        },
        currentTotalValue() {
            let sum = 0;
            for(let item of this.currentDateItems) {
                sum += item.value;
            }

            return sum;
        },
        previousDateItems() {
            let store = useStore();
            let dateRange = store.filters.dateRange.subtract(1, "day");
            
            if(this.items.length) {
                return this.items.filter(i => i.inputDate >= dateRange.fromDate 
                    && i.inputDate <= dateRange.toDate && i.state !== State.deleted);
            }

            return this.items;
        },
        currentDateItems() {
            let store = useStore();
            let dateRange = store.filters.dateRange;
            
            if(this.items.length) {
                return this.items.filter(i => i.inputDate >= dateRange.fromDate 
                    && i.inputDate <= dateRange.toDate && i.state !== State.deleted);
            }

            return this.items;
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