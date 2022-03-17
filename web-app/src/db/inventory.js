import inventoryDb from "./index";

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
    async setItems(items) {
        let connection = await this.getDbConnection();

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
    async getItems(fromDate, toDate) {
        let connection = await this.getDbConnection();
        
        if (!fromDate && !toDate) {
            return connection.select({ from: "items" });
        }

        return await connection.select({
            from: "items",
            where: {
                consumedDate: {
                    '-': {
                        low: fromDate,
                        high: toDate
                    }
                }
            }
        });
    }
}