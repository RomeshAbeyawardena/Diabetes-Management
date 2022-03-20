import Vue from 'vue';

import inventoryDb from '../db/inventory';
import { State } from '../db/inventory';

export const Inventory = {
    mutations: {
        setIsDirty: "setIsDirty",
        setCurrentId: "setCurrentId",
        setLastId: "setLastId",
        setFilters: "setFilters",
        setItems: "setItems",
        pushItem: "pushItem",
        updateItem: "updateItem",
        removeItem: "removeItem"
    },
    actions: {
        getLastId: "getLastId",
        getItems: "getItems",
        commitItems: "commitItems",
        rebuild: "rebuild"
    },
    getters: {
        filteredItems: "filteredItems",
        lastId: "lastId",
        totalUnits: "totalUnits",
    }
}

export default {
    state: {
        isDirty: false,
        currentId: 0,
        lastId: 0,
        filters: {
            fromDate: null,
            toDate: null
        },
        items: []
    },
    mutations: {
        setIsDirty(state, isDirty) {
            state.isDirty = isDirty;
        },
        setCurrentId(state, value) {
            Vue.set(state, "currentId", value);
        },
        setLastId(state, value) {
            Vue.set(state, "lastId", value);
            //reset current id
            Vue.set(state, "currentId", value);
        },
        setFilters(state, value) {
            Vue.set(state, "filters", value);
        },
        setItems(state, value) {
            if (value && value.length) {
                value.forEach(v => v.consumedDate = new Date(v.consumedDate));
            }

            Vue.set(state, "items", value);
        },
        pushItem(state, value) {
            state.items.push(value);
        },
        updateItem(state, payload) {
            let index = state.items.findIndex(i => i.id === payload.id);
            state.items.splice(index, 1, payload);
        },
        removeItem(state, id) {
            let index = state.items.findIndex(i => i.id === id);
            let item = state.items[index];
            if(item.state === State.added) {   
                state.items.splice(index, 1);

                if(state.currentId > state.lastId) {
                    Vue.set(state, "currentId", state.currentId - 1);
                }
            }
            else {
                item.state = State.deleted;
                state.items.splice(index, 1, item);
            }
        }
    },
    getters: {
        filteredItems(state) {
            return state.items.filter(i => i.state !== State.deleted);
        },
        lastId(state) {
            if(state.currentId > state.lastId) {
                return state.currentId;
            }

            return state.lastId;
        },
        totalUnits(state) {
            let sum = 0;

            let items = state.items;

            if (items && items) {
                items.forEach(i => sum += i.state !== State.deleted ? i.unitValue : 0);
            }

            return sum;
        }
    },
    actions: {
        async getLastId(context) {
            let result = await inventoryDb.getLastIndex();
            context.commit(Inventory.mutations.setLastId, result);
            return result;
        },
        async getItems(context) {
            let state = context.state;

            if (!state.filters.fromDate && !state.filters.toDate) {
                return;
            }

            let fromDate = state.filters.fromDate;
            let toDate = state.filters.toDate;

            let results = await inventoryDb.getItems(fromDate, toDate);
            
            context.commit(Inventory.mutations.setItems, results);
            await context.commit(Inventory.mutations.setIsDirty, false);
            await context.dispatch(Inventory.actions.getLastId);
        },
        async commitItems(context, items) {
            await inventoryDb.setItems(items);
            await context.dispatch(Inventory.actions.getLastId);
            await context.commit(Inventory.mutations.setIsDirty, false);
        },
        async rebuild(context) {
            await inventoryDb.rebuild(context);
        }
    }
}