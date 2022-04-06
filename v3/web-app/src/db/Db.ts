import { IDataBase, Connection } from "jsstore";
import workerInjector from "jsstore/dist/worker_injector";

export interface IDb {
    connection: Connection;
    database: IDataBase 
    isInitialised: boolean;
    destroy() : Promise<void>;
    getDbConnection(): Promise<Connection>;
    init() : Promise<boolean>;
    isDbCreated() : Promise<boolean>;
}

export abstract class DbBase implements IDb {
    isInitialised: boolean;
    connection: Connection;
    database: IDataBase;

    constructor(database: IDataBase) {
        this.database = database;
    }

    async getDbConnection() {
        if(this.connection){
            return this.connection;
        }
        
        if(!this.isInitialised){
            await this.init();
        }
        
        return this.connection;
    }

    async init(): Promise<boolean> {
        this.connection = new Connection();
        
        this.connection.addPlugin(workerInjector);
        
        let result = await this.isDbCreated();
        if(result)
        {
            this.isInitialised = true;
        }

        return result;
    }

    async isDbCreated(): Promise<boolean> {
        const isDbCreated = await this.connection
            .initDb(this.database);

        if(isDbCreated === true){
            console.log("db created");
        }
        else {
            console.log("db opened");
        }

        return isDbCreated;
    }

    async destroy(): Promise<void> {
        this.isInitialised = false;
        await this.connection.dropDb();
        await this.connection.terminate();
    }
}