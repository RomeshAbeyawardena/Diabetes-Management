import Db from "./index";
import lf from "lovefield";
let database = Db.init("inventory", "1.0"); 

database.addTable("items")
.addColumn("id", lf.Type.INTEGER)
.addColumn("description", lf.Type.STRING)
.addColumn("unitValue", lf.Type.INTEGER)
.addColumn("consumedDate", lf.Type.DATE_TIME)
.addPrimaryKey(['id']);

export default {
    database: database
}