import { IResponse } from "./response";

export interface IInventory {
    inventoryId: string;
    userId: string;
    subject: string;
    defaultIntent: string;
    hash: string;
    created: Date;
    modified: Date;
}

export interface IInventoryHistory {
    inventoryHistoryId: string;
    inventoryId: string;
    version: number;
    intent: string;
    value: string;
    created: Date;
    hash: string;
    inventory: IInventory
}

export interface IInventoryRequest {
    items: string;
    key: string;
    userId: string;
    type: string;
}

export interface IInventoryService {
    save(request: IInventoryRequest): Promise<IResponse<IInventoryHistory>>;
    get(request: IInventoryRequest) : Promise<IResponse<IInventoryHistory>>;
}