import { defineStore } from 'pinia'
import { Inventory } from '../models/Inventory';
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
        }
    },
    actions: {
        async getLastId() {
            this.lastStoredId = await this.inventoryDb.getLastIndex();
        },
        addNew() {
            this.items.push(new Inventory(this.lastId + 1, Date(), "", Number(0)));
        } 
    }
});