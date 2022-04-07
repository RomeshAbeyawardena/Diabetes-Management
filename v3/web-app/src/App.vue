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
import { DialogType } from './models';
import { useStore } from './stores/main';
import { useInventoryStore } from './stores/inventory';
import { onBeforeMount, onMounted } from 'vue';
import { ref } from 'vue';

const store = useStore();
const inventoryStore = useInventoryStore();
const { getLastId, load } = inventoryStore;

onBeforeMount(() => {
  store.setFilterDateRange(new Date(), new Date());
});

onMounted(async() => { 
  await getLastId();
  await load();
  store.getConsent();
  if(!store.consent.hasConsented)
  {
    const cookieDialog = store.getDialog(DialogType.CookiePolicy);
    store.showDialog(cookieDialog, undefined, false);
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