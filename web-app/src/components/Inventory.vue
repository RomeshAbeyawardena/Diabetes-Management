<template>
<div>
  <ScrollPanel style="width:inherit; height:calc(100vh - 255px)">
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
      :state="item.state"
      :read-only="readOnly"
    />
  </ScrollPanel >
    <h3>Total: {{ totalUnits }} units</h3>
    <div class="grid controls">
      <div class="col">
        <Button icon="pi pi-plus-circle" 
                class="p-button-raised"
                label="Add new" 
                v-on:click="addItem" />
      </div>
      <div class="col align-right">
        <Button icon="pi pi-undo" 
                class="hide-label mr-2 p-button-raised p-button-danger" 
                v-tooltip.top="'Reset app'"
                label="Reset app" 
                v-on:click="resetApp" />
        <Button icon="pi pi-save" 
                class="p-button-raised p-button-success" 
                v-tooltip.top="'Save changes'"
                label="Save" 
                v-on:click="saveItems" />
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import Button from "primevue/button";
import InventoryItem from "./Inventory-item.vue";
import DateHelper from "../helpers/date-helper";
import { Inventory } from "../store/inventory";
import { State } from "../db/inventory";
import { mapGetters } from "vuex";
import ScrollPanel from 'primevue/scrollpanel';
import Tooltip from 'primevue/tooltip';

export default {
  name: "inventory",
  components: {
    Button,
    InventoryItem,
    ScrollPanel
  },
  computed: {
    ...mapGetters([
      Inventory.getters.totalUnits, 
      Inventory.getters.lastId])
  },
  directives: {
    'tooltip': Tooltip
  },
  methods: {
    shouldShowHeader(item) {
      return this.items.indexOf(item) < 1;
    },
    setItem(item) {
      this.$store.commit(Inventory.mutations.updateItem, item);
      this.$store.commit(Inventory.mutations.setIsDirty, true);
    },
    resetApp(event) {
       this.$confirm.require({
        target: event.currentTarget,
        message: "Are you sure you want to proceed? This will remove all data and reload the page once completed.",
        icon: "pi pi-exclamation-triangle",
        accept: () => {
          this.$store.dispatch(Inventory.actions.rebuild);
        },
        reject: () => {
          //callback to execute when user rejects the action
        },
      });
    },
    async addItem() {
      let fromDate = this.$store.state.Inventory.filters.fromDate;
      let newId = this.lastId + 1;
      let dateNow = new Date();
      this.$store.commit(Inventory.mutations.pushItem, {
        id: newId,
        description: "",
        unitValue: 0,
        consumedDate: DateHelper.getDate(fromDate, dateNow),
        state: State.added
      });
      this.$store.commit(Inventory.mutations.setIsDirty, true);
      this.$store.commit(Inventory.mutations.setCurrentId, newId);
    },
    async loadItems() {
      await this.$store.dispatch(Inventory.actions.getItems);
      this.$toast.add({
        severity: "success",
        summary: "Items loaded",
        detail: "Items successfully loaded",
        life: 3000,
      });
    },
    async saveItems() {
      await this.$store.dispatch(Inventory.actions.commitItems, this.$store.state.Inventory.items);
      this.$toast.add({
        severity: "success",
        summary: "Items saved",
        detail: "Items successfully saved",
        life: 3000,
      });
    },
    removeItem(event, id) {
      let context = this;
      this.$confirm.require({
        target: event.currentTarget,
        message: "Are you sure you want to delete this entry?",
        icon: "pi pi-exclamation-triangle",
        accept: () => {
          console.log("Accept");
          context.$store.commit(Inventory.mutations.removeItem, id);
        },
        reject: () => {
          console.log("Reject");
          //callback to execute when user rejects the action
        },
      });
    },
  },
  props: {
    readOnly: Boolean,
    items: Array,
  },
  async created() {
    await this.loadItems();
  },
};
</script>