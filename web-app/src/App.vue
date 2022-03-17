<template>
  <div id="app">
    <date-navigation />
    <ScrollPanel style="height:100%">
      <inventory :items="items" />
    </ScrollPanel>
    
  </div>
</template>

<script>
import Inventory from "./components/Inventory.vue"
import DateNavigation from "./components/Date-navigation.vue";
import ScrollPanel from 'primevue/scrollpanel';
export default {
  name: 'App',
  components: {
    DateNavigation,
    Inventory,
    ScrollPanel
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
