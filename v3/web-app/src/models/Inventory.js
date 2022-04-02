export class Inventory {
    fromObject (state) {
        this.id = Number(state.id);
        this.inputDate = Date(state.inputDate);
        this.description = state.description;
        this.value = Number(state.value);
    }

    constructor(id, inputDate, description, value) {
        this.id = Number(id);
        this.inputDate = Date(inputDate);
        this.description = description;
        this.value = Number(value);
    }
}
