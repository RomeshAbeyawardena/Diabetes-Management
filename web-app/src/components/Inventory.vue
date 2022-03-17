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
        }
        
    },
    methods: {
        shouldShowHeader(item) {
            return this.items.indexOf(item) < 1;
        },
        setItem(item) {
            this.$store.commit("updateItem", item)
        },
        addItem() {
            let id = this.$store.getters.getLastId;
            let fromDate = this.$store.state.Inventory.filters.fromDate;
            console.log(fromDate);
            let dateNow = new Date();
            this.$store.commit("pushItem", { 
                id: id + 1, 
                description: "", 
                unitValue: 0,
                consumedDate: new Date(fromDate.getFullYear(), 
                    fromDate.getMonth(), 
                    fromDate.getDate(),
                    dateNow.getHours(),
                    dateNow.getMinutes(),
                    dateNow.getSeconds())
            });
        },
        loadItems() {
            this.$store.dispatch("getItems", "inventory");
        },
        saveItems() {
            this.$store.dispatch("commitItems", "inventory");
        },
        removeItem(id) {
            this.$store.commit("removeItem", id);
        }
    },
    props: {
        "readOnly": Boolean,
        "items": Array
    },
    created() {
        this.loadItems();
    }
}
</script>