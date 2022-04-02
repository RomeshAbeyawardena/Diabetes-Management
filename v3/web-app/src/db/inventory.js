import inventoryDb from "./index";
import { Inventory, State } from "../models/Inventory";

export default {
    dbConnection: null,
    async getDbConnection() {
        if(this.dbConnection){
            return this.dbConnection;
        }
        
        if(!inventoryDb.isInitialised){
            await inventoryDb.init();
        }
        
        this.dbConnection = inventoryDb.connection;

        return inventoryDb.connection;
    },
    async rebuild() {
        await inventoryDb.destroy();
        this.dbConnection = null;
        window.location.reload();
    },
    async setItems(items) {
        let connection = await this.getDbConnection();
        
        for(let item of items) 
        {
            if(item.state !== State.modified) {
                continue;
            }

            if(item.state === State.added)
            {
                item.state = State.modified;
            }
        }

        await connection.insert({
            into: "items",
            upsert: true,
            values: items
        });
    },
    async searchItems(query) {
        let connection = await this.getDbConnection();
        
        return await connection.select({
            from: "items",
            where: {
                description: {
                    like: "%" + query + "%"
                }
            }
        });
    },
    async getPreviousUnitsCount(fromDate, toDate) {
        let connection = await this.getDbConnection();

        let items =  await connection.select({
            from: "items",
            aggregate: {
                sum: "unitValue"    
            },
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

        if(!items.length){
            return 0;
        }

        let maxId = items[0]["sum(unitValue)"];
        return maxId;
    },
    async getLastIndex() {
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
        return maxId;
    },
    async sync() {
        
    },
    async getItems(fromDate, toDate) {
        let connection = await this.getDbConnection();
        
        if (!fromDate && !toDate) {
            return connection.select({ from: "items" });
        }

        let items = await connection.select({
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

        return items.map(i => new Inventory())
    }
}