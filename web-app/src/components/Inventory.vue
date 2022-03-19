<template>
  <div>
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
      :read-only="readOnly"
    />

    <h3>Total: {{ totalUnits }} units</h3>
    <Button class="mr-2" v-on:click="addItem">Add new</Button>
    <Button v-on:click="saveItems">Save</Button>
  </div>
</template>
<script type="text/javascript">
import Button from "primevue/button";
import InventoryItem from "./Inventory-item.vue";
import DateHelper from "../helpers/date-helper";
import { Inventory } from "../store/inventory";
import { State } from "../db/inventory";

export default {
  name: "inventory",
  components: {
    Button,
    InventoryItem,
  },
  computed: {
    totalUnits() {
      return this.$store.getters.totalUnits;
    },
    lastId() {
      return this.$store.getters.lastId;
    },
  },
  methods: {
    shouldShowHeader(item) {
      return this.items.indexOf(item) < 1;
    },
    setItem(item) {
      this.$store.commit(Inventory.mutations.updateItem, item);
      this.$store.commit(Inventory.mutations.setIsDirty, true);
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
      await this.$store.dispatch(Inventory.actions.commitItems, this.items);
      this.$toast.add({
        severity: "success",
        summary: "Items saved",
        detail: "Items successfully saved",
        life: 3000,
      });
    },
    removeItem(event, id) {
      this.$confirm.require({
        target: event.currentTarget,
        message: "Are you sure you want to proceed?",
        icon: "pi pi-exclamation-triangle",
        accept: () => {
          this.$store.commit(Inventory.mutations.removeItem, id);
        },
        reject: () => {
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