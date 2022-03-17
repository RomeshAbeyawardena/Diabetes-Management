<template>
  <div id="app">
    <date-navigation />
    <inventory :items="items" />
  </div>
</template>

<script>
import Inventory from "./components/Inventory.vue"
import DateNavigation from "./components/Date-navigation.vue";

export default {
  name: 'App',
  components: {
    DateNavigation,
    Inventory
  },
  computed: {
    items() { 
      return this.$store.getters.filteredItems;
    }
  },
  created(){
    return this.$store.commit("setFilters", {
      fromDate: new Date("2022-03-17T00:00:00"),
      toDate: new Date("2022-03-17T23:59:59")
    });
  }
}
</script>
