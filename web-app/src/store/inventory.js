import Vue from 'vue';

import inventoryDb from '../db/inventory';

export const Inventory = {
    mutations: {
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
        commitItems: "commitItems"
    }
}

export default {
    state: {
        currentId: 0,
        lastId: 0,
        filters: {
            fromDate: null,
            toDate: null
        },
        items: []
    },
    mutations: {
        setCurrentId(state, value){
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
            state.items.splice(index, 1);
        }
    },
    getters: {
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
                items.forEach(i => sum += i.unitValue);
            }

            return sum;
        }
    },
    actions: {
        async getLastId(context) {
            let result = await inventoryDb.getLastIndex();
            context.commit("setLastId", result);
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
            console.log(results);
            context.commit("setItems", results);
            await context.dispatch("getLastId");
        },
        async commitItems(context, items) {
            await inventoryDb.setItems(items);
            await context.dispatch("getLastId");
        }
    }
}