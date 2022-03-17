import Vue from 'vue';
import dayjs from "dayjs";
let isBetween = require('dayjs/plugin/isBetween');
dayjs.extend(isBetween);

import inventory from '../db/inventory';

const dbConnection = inventory.database.schemaBuilder.connect();

export default {
    state: {
        filters: {
            fromDate: null,
            toDate: null
        },
        items: []
    },
    mutations: {
        setFilters(state, value){
            Vue.set(state, "filters", value);
        },
        setItems(state, value) {
            if(value && value.length) {
                value.forEach(v => v.consumedDate = new Date(v.consumedDate));
                Vue.set(state, "items", value);
            }
        },
        pushItem(state, value){
            state.items.push(value);
        },
        updateItem(state, payload) {
            let index = state.items.findIndex(i => i.id === payload.id);
            state.items.splice(index, 1, payload);
        },
        removeItem(state, id){
            let index = state.items.findIndex(i => i.id === id);
            state.items.splice(index, 1);
        }
    },
    getters: {
        filteredItems(state) {
            
            if(!state.filters.fromDate && !state.filters.toDate)
            {
                return state.items;
            }

            let fromDate = dayjs(state.filters.fromDate);
            let toDate = dayjs(state.filters.toDate);

            return state.items.filter(i => dayjs(i.consumedDate).isBetween(fromDate, toDate));
        },
        getLastId(state) {
            let length = state.items ? state.items.length : 0;

            if(!length)
                return 0;

            return state.items[length - 1].id;
        },
        totalUnits(state, getters) {
            let sum = 0;
            
            let items = getters.filteredItems;

            if(items && items)
            {
                items.forEach(i => sum += i.unitValue);
            }
            
            return sum;
        }
    },
    actions: {
        getItems(context, key) {
            let items = localStorage.getItem(key)
            context.commit("setItems", JSON.parse(items));
        },
        commitItems(context, key) {
            let items = context.state.items;
            console.log(inventory.database);
            dbConnection.then(db => {
                
                let itemTable = db.getSchema().table("items");
                let rows = [];
                for(let item of items) {
                    let row = itemTable.createRow(item);
                    rows.push(row);
                }

                return db.insertOrReplace()
                    .into(itemTable).values(rows).exec();
            });
            localStorage.setItem(key, JSON.stringify(items));
        }
    }
}