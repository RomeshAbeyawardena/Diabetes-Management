<script setup>
//components
import ActionNavigation from './components/ActionNavigation.vue';
import Dialogs from './components/dialogs/Dialogs.vue';
import Grid from "./components/Grid.vue";
import Navigation from "./components/Navigation.vue";
import Sidebars from './components/side-bars/Sidebars.vue';
import Title from './components/Title.vue';
import ConfirmPopup from 'primevue/confirmpopup';
//references
import { useInventoryStore } from './stores/inventory';
import { onMounted } from 'vue';

const store = useInventoryStore();
onMounted(async() => { 
  await store.getLastId();
  await store.load() 
});
</script>

<template>
  <div class="app">
    <ConfirmPopup />
    <Sidebars />
    <Dialogs />
    <Title />
    <Navigation />
    <Grid />
    <ActionNavigation />
  </div>
</template>