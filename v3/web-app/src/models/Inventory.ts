export enum State {
    added = "added",
    modified = "Modified",
    unchanged = "Unchanged",
    deleted = "Deleted" 
}

export interface IInventoryHelper {
    getTotalValue(items: IInventory[]) : Number;
}

export interface IInventory {
    id: number;
    inputDate: Date;
    description: String;
    value: number;
    state: State;
    published: Boolean;

    fromObject(state: Object): Inventory;
    toObject(): any;
} 

export class InventoryHelper implements IInventoryHelper {
    getTotalValue(items: IInventory[]): number {
        let sum: number = 0;
        for(let item of items) {
            sum = sum + item.value;
        }

        return sum;
    }
}

export class Inventory implements IInventory {
    id: number;
    inputDate: Date;
    description: String;
    value: number;
    state: State;
    published: Boolean;

    constructor(id: number, inputDate: Date, description, value: number, 
        state : State, published : Boolean) {
        this.id = id,
        this.inputDate = inputDate,
        this.description = description,
        this.value = value,
        this.state = state,
        this.published = published
    }

    fromObject(state: any): Inventory {
        this.id = Number(state.id);
        this.inputDate = new Date(state.inputDate);
        this.description = state.description;
        this.value = Number(state.value);
        this.state = state.state;
        this.published = state.published;
        return this;
    }

    toObject(): any {
        return {
            id: this.id,
            inputDate: this.inputDate,
            description: this.description,
            value: this.value,
            state: this.state,
            published: this.published
        } 
    }
}