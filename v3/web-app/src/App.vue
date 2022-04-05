<script setup>
//components
import ActionNavigation from './components/ActionNavigation.vue';
import ConfirmPopup from 'primevue/confirmpopup';
import Dialogs from './components/dialogs/Dialogs.vue';
import Grid from "./components/Grid.vue";
import Navigation from "./components/Navigation.vue";
import Sidebars from './components/side-bars/Sidebars.vue';
import StatusBar from './components/StatusBar.vue';
import Title from './components/Title.vue';
//references
import { DialogTypes } from './models/Dialogs';
import { useStore } from './stores';
import { useInventoryStore } from './stores/inventory';
import { onMounted } from 'vue';
import { ref } from 'vue';
const store = useStore();
const inventoryStore = useInventoryStore();
onMounted(async() => { 
  await inventoryStore.getLastId();
  await inventoryStore.load();
  store.getConsent();
  if(!store.consent.hasConsented)
  {
    store.showDialog(DialogTypes.CookiePolicy, "Cookie policy", undefined, false);
  }
});

const date = ref(new Date());
</script>

<template>
  <div class="app">
    <ConfirmPopup />
    <Sidebars />
    <Dialogs />
    <Title />
    <Navigation />
    <Grid />
    <StatusBar />
    <ActionNavigation />
  </div>
</template>