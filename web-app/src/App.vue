<template>
  <div id="app">
    <toast position="center" />
    <date-navigation />
    <inventory-vue :items="items" />
  </div>
</template>

<script>
import { Inventory } from "./store/inventory";
import InventoryVue from "./components/Inventory.vue"
import DateNavigation from "./components/Date-navigation.vue";
import Toast from 'primevue/toast';

import dayjs from "dayjs";
export default {
  name: 'App',
  components: {
    DateNavigation,
    InventoryVue,
    Toast
  },
  computed: {
    items() { 
      return this.$store.state.Inventory.items;
    }
  },
  created(){
    return this.$store.commit(Inventory.mutations.setFilters, {
      fromDate: dayjs().startOf("day").toDate(),
      toDate: dayjs().endOf("day").toDate()
    });
  }
}
</script>
