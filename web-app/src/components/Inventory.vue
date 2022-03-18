<template>
    <div>
        <ScrollPanel style="height:180px">
            <inventory-item 
                v-for="item in items" 
                v-bind:key="item.id" 
                v-on:item:updated="setItem"
                v-on:item:deleted="removeItem"
                :show-header="shouldShowHeader(item)"
                :inventory-id="item.id"
                :consumed-date="item.consumedDate"
                :description="item.description"
                :unit-value="item.unitValue"
                :read-only="readOnly" />
        </ScrollPanel>
        <h3>Total: {{ totalUnits }} units</h3>
        <Button class="mr-2" v-on:click="addItem">Add new</Button>
        <Button v-on:click="saveItems">Save</Button>
    </div>
</template>
<script type="text/javascript">
import Button from 'primevue/button';
import InventoryItem from "./Inventory-item.vue"
import ScrollPanel from 'primevue/scrollpanel';
import DateHelper from "../helpers/date-helper";
import { Inventory } from "../store/inventory";

export default {
    name: "inventory",
    components: {
        Button,
        InventoryItem,
        ScrollPanel
    },
    computed: {
        totalUnits() {
            return this.$store.getters.totalUnits;
        },
        lastId() {
            return this.$store.getters.lastId;
        }
    },
    methods: {
        shouldShowHeader(item) {
            return this.items.indexOf(item) < 1;
        },
        setItem(item) {
            this.$store.commit(Inventory.mutations.updateItem, item)
        },
        async addItem() {
            
            let fromDate = this.$store.state.Inventory.filters.fromDate;
            let newId = this.lastId + 1;
            let dateNow = new Date();
            this.$store.commit(Inventory.mutations.pushItem, { 
                id: newId,
                description: "", 
                unitValue: 0,
                consumedDate: DateHelper.getDate(fromDate, dateNow)
            });

            this.$store.commit(Inventory.mutations.setCurrentId, newId);
        },
        async loadItems() {
            await this.$store.dispatch(Inventory.actions.getItems);
        },
        async saveItems() {
            this.$store.dispatch(Inventory.actions.commitItems);
        },
        removeItem(id) {
            this.$store.commit(Inventory.mutations.removeItem, id);
        }
    },
    props: {
        "readOnly": Boolean,
        "items": Array
    },
    async created() {
        await this.loadItems();
    }
}
</script>