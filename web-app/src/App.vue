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
import { Store } from "./store"; 

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
  created() {
    this.clientWidth = window.innerWidth;
    let store = this.$store;
    window.onresize = () => {
      store.commit(Store.mutations.setClientSize, { 
        width: window.innerWidth,
        height: window.innerHeight
      });
    };

    return this.$store.commit(Inventory.mutations.setFilters, {
      fromDate: dayjs().startOf("day").toDate(),
      toDate: dayjs().endOf("day").toDate()
    });
  }
}
</script>