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
}