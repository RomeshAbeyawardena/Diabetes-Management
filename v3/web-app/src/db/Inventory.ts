import { DATA_TYPE, IAlterQuery, IDataBase, ITable, TColumns } from "jsstore";
import { State } from "../models/Inventory";
import { DbBase } from "./Db";

export interface IItem {
    id: number;
    description: string;
    value: number;
    inputDate: Date;
    state: State,
    published: boolean
}

export interface IInventoryDb {
    getItems() : Promise<IItem[]>;
    getLastIndex() : Promise<number>;
    rebuild() : Promise<void>;
    searchItems(query: string) : Promise<IItem[]>;
    setItems() : Promise<void>;
    sync() : Promise<void>;
}

export class Items implements ITable {
    name: string;
    columns: TColumns;
    alter?: IAlterQuery;

    constructor() {
        this.name = "items";
        this.columns = {
            "id" : {
                primaryKey: true,
                unique: true,
                notNull: true,
                dataType: DATA_TYPE.Number,
                enableSearch: true,
                keyPath: []
            },
            "description" : {
                primaryKey: true,
                unique: true,
                notNull: true,
                dataType: DATA_TYPE.String,
                enableSearch: true,
                keyPath: []
            },
            "value" : {
                primaryKey: true,
                unique: true,
                notNull: true,
                dataType: DATA_TYPE.Number,
                enableSearch: true,
                keyPath: []
            },
            "inputDate" : {
                primaryKey: true,
                unique: true,
                notNull: true,
                dataType: DATA_TYPE.DateTime,
                enableSearch: true,
                keyPath: []
            },
            "state" : {
                primaryKey: true,
                unique: true,
                notNull: true,
                dataType: DATA_TYPE.String,
                enableSearch: true,
                keyPath: []
            },
            "published" : {
                primaryKey: true,
                unique: true,
                notNull: true,
                dataType: DATA_TYPE.Boolean,
                enableSearch: true,
                keyPath: []
            },
        }
    }
}

export class InventoryDatabase implements IDataBase {
    name: string;
    tables: ITable[];
    version?: number;

    constructor() {
        this.name = "inventory";
        this.version = 1;
        this.tables = new Array<ITable>();
        this.tables.push(new Items());
    }
}

export class InventoryDb extends DbBase implements IInventoryDb {
    constructor() {
        super(new InventoryDatabase());
    }
    getItems(): Promise<IItem[]> {
        throw new Error("Method not implemented.");
    }
    getLastIndex(): Promise<number> {
        throw new Error("Method not implemented.");
    }
    rebuild(): Promise<void> {
        throw new Error("Method not implemented.");
    }
    searchItems(query: string): Promise<IItem[]> {
        throw new Error("Method not implemented.");
    }
    setItems(): Promise<void> {
        throw new Error("Method not implemented.");
    }
    sync(): Promise<void> {
        throw new Error("Method not implemented.");
    }
}