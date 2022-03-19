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
import { Store } from "./store"; 
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
  methods: {
    setClientSize(width, height) {
      this.$store.commit(Store.mutations.setClientSize, { 
        width: width,
        height: height
      });
    }
  },
  created() {
    this.setClientSize(window.innerWidth, window.innerHeight);
    let context = this;
    window.onresize = () => {
      context.setClientSize(window.innerWidth, window.innerHeight);
    };

    return this.$store.commit(Inventory.mutations.setFilters, {
      fromDate: dayjs().startOf("day").toDate(),
      toDate: dayjs().endOf("day").toDate()
    });
  }
}
</script>