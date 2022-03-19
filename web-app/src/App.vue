<template>
  <div id="app">
    <confirm-popup />
    <toast position="center" />
    <date-navigation />
    <inventory-vue :items="filteredItems" />
  </div>
</template>

<script>
import ConfirmPopup from 'primevue/confirmpopup';
import { Inventory } from "./store/inventory";
import InventoryVue from "./components/Inventory.vue"
import DateNavigation from "./components/Date-navigation.vue";
import Toast from 'primevue/toast';
import { mapGetters } from 'vuex';

import dayjs from "dayjs";
export default {
  name: 'App',
  components: {
    ConfirmPopup,
    DateNavigation,
    InventoryVue,
    Toast
  },
  computed: {
    ...mapGetters([Inventory.getters.filteredItems])
  },
  created() {
    return this.$store.commit(Inventory.mutations.setFilters, {
      fromDate: dayjs().startOf("day").toDate(),
      toDate: dayjs().endOf("day").toDate()
    });
  }
}
</script>