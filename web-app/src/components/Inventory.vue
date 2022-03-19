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
      :state="item.state"
      :read-only="readOnly"
    />

    <h3>Total: {{ totalUnits }} units</h3>
    <div class="grid">
      <div class="col">
        <Button icon="pi pi-plus-circle" 
                label="Add new" 
                v-on:click="addItem" />
      </div>
      <div class="col align-right">
        <Button icon="pi pi-undo" 
                class="mr-2 p-button-danger" 
                :label="toggleResponsiveLabel('Reset app')" 
                v-on:click="resetApp" />
        <Button icon="pi pi-save" 
                class="p-button-success" 
                :label="toggleResponsiveLabel('Save changes', 'Save')" 
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
export default {
  name: "inventory",
  components: {
    Button,
    InventoryItem,
  },
  computed: {
    clientSize() {
      return this.$store.state.clientSize;
    },
    ...mapGetters([
      Inventory.getters.totalUnits, 
      Inventory.getters.lastId])
  },
  methods: {
    toggleResponsiveLabel(label, shortName) {
      if(this.clientSize.width < 400) {
        return shortName ?? null;
      }
      
      return label;
    },
    shouldShowHeader(item) {
      return this.items.indexOf(item) < 1;
    },
    setItem(item) {
      this.$store.commit(Inventory.mutations.updateItem, item);
      this.$store.commit(Inventory.mutations.setIsDirty, true);
    },
    resetApp() {

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