import { isDate } from "@vue/shared";

export const InventoryHelper = {
    getTotalValue(items) {
        let sum = 0;
        for(let item of items) {
            sum += item.value;
        }

        return sum;
    }
}

export class Inventory {
    fromObject (state) {
        this.id = Number(state.id);
        this.inputDate = new Date(state.inputDate);
        this.description = state.description;
        this.value = Number(state.value);
        this.state = state.state;
        this.published = state.published;
        return this;
    }

    toObject () {
        return {
            id: this.id,
            inputDate: this.inputDate,
            description: this.description,
            value: this.value,
            state: this.state,
            published: this.published
        } 
    }

    constructor(id, inputDate, description, value, state, published) {
        if(!isDate(inputDate)) {
            inputDate = new Date(inputDate);
        }
        
        this.id = Number(id);
        this.inputDate = inputDate;
        this.description = description;
        this.value = Number(value);
        this.state = state;
        this.published = published;
    }
}

export const State = {
    added: "Added",
    modified: "Modified",
    unchanged: "Unchanged",
    deleted: "Deleted" 
}