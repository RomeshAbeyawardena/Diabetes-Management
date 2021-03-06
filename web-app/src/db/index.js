import { Connection } from "jsstore";
import workerInjector from "jsstore/dist/worker_injector";


const itemsDb = {
    name: "items",
    columns: {
        id: { primaryKey: true, autoIncrement: false },
        description: { dataType: "string" },
        unitValue: { dataType: "number" },
        consumedDate: { dataType: "date_time" },
        state: { dataType: "string" }
    }
};

export default {
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
    async destroy() {
        this.isInitialised = false;
        console.log("destroy", "dropping db");
        let result = await this.connection.dropDb();
        await this.connection.terminate();
        console.log("connection terminated");
        console.log("destroy", "dropped db", result);
    },
    isInitialised: false,
    connection: null,
    database: {
        name: "inventory",
        tables: [itemsDb]
    },
    items: itemsDb
}