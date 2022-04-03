import { isDate } from "@vue/shared";

export class Inventory {
    fromObject (state) {
        this.id = Number(state.id);
        this.inputDate = Date(state.inputDate);
        this.description = state.description;
        this.value = Number(state.value);
        this.state = state.state;
    }

    constructor(id, inputDate, description, value, state) {
        if(!isDate(inputDate)) {
            inputDate = new Date(inputDate);
        }
        
        this.id = Number(id);
        this.inputDate = inputDate;
        this.description = description;
        this.value = Number(value);
        this.state = state;
    }
}

export const State = {
    added: "Added",
    modified: "Modified",
    unchanged: "Unchanged",
    deleted: "Deleted" 
}