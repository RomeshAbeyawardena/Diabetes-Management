import Vue from 'vue';
import dayjs from "dayjs";

let isBetween = require('dayjs/plugin/isBetween');
dayjs.extend(isBetween);

import inventoryDb from '../db/inventory';

export default {
    state: {
        filters: {
            fromDate: null,
            toDate: null
        },
        items: []
    },
    mutations: {
        setFilters(state, value) {
            Vue.set(state, "filters", value);
        },
        setItems(state, value) {
            if (value && value.length) {
                value.forEach(v => v.consumedDate = new Date(v.consumedDate));
                Vue.set(state, "items", value);
            }
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
        filteredItems(state) {
            
            if (!state.filters.fromDate && !state.filters.toDate) {
                return state.items;
            }

            let fromDate = dayjs(state.filters.fromDate);
            let toDate = dayjs(state.filters.toDate);

            return state.items.filter(i => dayjs(i.consumedDate)
                .isBetween(fromDate, toDate));
        },
        getLastId(state) {
            let length = state.items ? state.items.length : 0;

            if (!length)
                return 0;

            return state.items[length - 1].id;
        },
        totalUnits(state, getters) {
            let sum = 0;

            let items = getters.filteredItems;

            if (items && items) {
                items.forEach(i => sum += i.unitValue);
            }

            return sum;
        }
    },
    actions: {
        async getItems(context, key) {
            let items = localStorage.getItem(key)
            
            context.commit("setItems", JSON.parse(items));

            let state = context.state;

            if (!state.filters.fromDate && !state.filters.toDate) {
                return;
            }

            let fromDate = state.filters.fromDate;
            let toDate = state.filters.toDate;

            let results = await inventoryDb.getItems(fromDate, toDate);

            console.log(results);
        },
        async commitItems(context, key) {
            let items = context.state.items;
            localStorage.setItem(key, JSON.stringify(items));
            
            await inventoryDb.setItems(items);
        }
    }
}