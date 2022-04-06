import { DATA_TYPE, IAlterQuery, IDataBase, ITable, TColumns } from "jsstore";
import { State, IInventory } from "../models/Inventory";
import { DbBase } from "./Db";


export interface IInventoryDb {
    getItems(fromDate: Date, toDate: Date) : Promise<IInventory[]>;
    getLastIndex() : Promise<number>;
    rebuild() : Promise<void>;
    searchItems(query: string) : Promise<IInventory[]>;
    setItems(items: IInventory[]) : Promise<void>;
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
    async getItems(fromDate: Date, toDate: Date): Promise<IInventory[]> {
        let connection = await this.getDbConnection();
        
        if (!fromDate && !toDate) {
            return connection.select({ from: "items" });
        }

        return await connection.select<IInventory>({
            from: "items",
            where: {
                state: {
                    in: [State.unchanged, State.modified]
                },
                consumedDate: {
                    '-': {
                        low: fromDate,
                        high: toDate
                    }
                }
            }
        });
    }
    async getLastIndex(): Promise<number> {
        let connection = await this.getDbConnection();

        let items =  await connection.select({
            from: "items",
            aggregate: {
                max: "id"    
            },
        });
        
        if(!items.length){
            return 0;
        }

        let maxId = items[0]["max(id)"];
        return maxId;;
    }
    async rebuild(): Promise<void> {
        await this.destroy();
        this.connection = null;
        window.location.reload();
    }
    async searchItems(query: string): Promise<IInventory[]> {
        let connection = await this.getDbConnection();
        
        return await connection.select({
            from: "items",
            where: {
                description: {
                    like: "%" + query + "%"
                }
            }
        });
    }
    async setItems(items: IInventory[]): Promise<void> {
        let connection = await this.getDbConnection();
        
        let itemsToPush = [];

        for(let item of items) 
        {
            if(item.state === State.added) {
                item.state = State.modified;
            }

            //don't push items marked as deleted that haven't been published
            if(item.state === State.deleted && !item.published) {
                continue;
            }

            if(!item.published) {
                item.published = true;
            }

            itemsToPush.push(item);
        }

        await connection.insert({
            into: "items",
            upsert: true,
            values: itemsToPush
        });
    }
    sync(): Promise<void> {
        throw new Error("Method not implemented.");
    }
}