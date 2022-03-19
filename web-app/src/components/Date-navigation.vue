<template>
  <div class="date-navigation grid mb-4">
    <div class="col-2">
      <Button
        v-on:click="navigate()"
        icon="pi pi-angle-double-left"
        class="p-button-rounded p-button-secondary"
      />
    </div>
    <div class="col-8 align-centre flex align-items-center justify-content-center">
      <h4 class="mt-0 mb-0">{{ fromDate }}</h4>
    </div>
    <div class="col-2 align-right">
      <Button
        v-on:click="navigate('forward')"
        icon="pi pi-angle-double-right"
        class="p-button-rounded p-button-secondary"
      />
    </div>
  </div>
</template>
<script>
import Button from "primevue/button/Button";
import dayjs from "dayjs";
import { Inventory } from "../store/inventory";

export default {
  components: {
    Button,
  },
  computed: {
    fromDate() {
      return dayjs(this.$store.state.Inventory.filters.fromDate).format(
        "ddd, DD MMMM YYYY"
      );
    },
  },
  methods: {
    async saveItems() {
      await this.$store.dispatch(
        Inventory.actions.commitItems,
        this.$store.state.Inventory.items
      );
      this.$toast.add({
        severity: "success",
        summary: "Items automatically saved",
        detail: "Items successfully saved",
        life: 1000,
      });
    },
    async navigate(direction) {
      let fromDate = dayjs(this.$store.state.Inventory.filters.fromDate);
      let toDate = dayjs(this.$store.state.Inventory.filters.toDate);

      let filter =
        direction === "forward"
          ? {
              fromDate: fromDate.add(1, "day").toDate(),
              toDate: toDate.add(1, "day").toDate(),
            }
          : {
              fromDate: fromDate.subtract(1, "day").toDate(),
              toDate: toDate.subtract(1, "day").toDate(),
            };
      
      if(this.$store.state.Inventory.isDirty) {
        await this.saveItems();
      }

      this.$store.commit(Inventory.mutations.setFilters, filter);
      await this.$store.dispatch(Inventory.actions.getItems);
    },
  },
  name: "date-navigation",
};
</script>