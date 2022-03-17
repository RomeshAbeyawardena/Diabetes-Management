import lf from "lovefield";

export default {
    schemaBuilder: null,
    init(dbName, dbVersion){
        this.schemaBuilder = lf.schema.create(dbName, dbVersion);
        return this;
    },
    addTable(tableName) {
        return this.schemaBuilder.createTable(tableName);
    }
}