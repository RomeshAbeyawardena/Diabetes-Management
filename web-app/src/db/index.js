import { Connection } from "jsstore";
import workerInjector from "jsstore/dist/worker_injector";


const itemsDb = {
    name: "items",
    columns: {
        id: { primaryKey: true, autoIncrement: false },
        description: { dataType: "string" },
        unitValue: { dataType: "number" },
        consumedDate: { dataType: "date_time" }
    }
};

export default {
    isInitialised: false,
    async init() {
        
        this.connection = new Connection();
        
        this.connection.addPlugin(workerInjector);
        
        let result = await this.isDbCreated();
        
        this.isInitialised = true;
        return result;
    },
    async isDbCreated() {        
        const isDbCreated = await this.connection.initDb(this.database);

        if(isDbCreated === true){
            console.log("db created");
            // here you can prefill database with some data
        }
        else {
            console.log("db opened");
        }

        return isDbCreated;
    },
    connection: null,
    database: {
        name: "inventory",
        tables: [itemsDb]
    },
    items: itemsDb
}