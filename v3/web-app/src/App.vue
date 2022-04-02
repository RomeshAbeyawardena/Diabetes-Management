<script setup>
//components
import ActionNavigation from './components/ActionNavigation.vue';
import Dialogs from './components/dialogs/Dialogs.vue';
import Grid from "./components/Grid.vue";
import Navigation from "./components/Navigation.vue";
import Sidebars from './components/side-bars/Sidebars.vue';
import Title from './components/Title.vue';

//references
import { Inventory } from "./models/Inventory";

const entry = new Inventory(Date(), "", 0);

</script>

<template>
  <div class="app">
    <Sidebars />
    <Dialogs />
    <Title />
    <Navigation />
    <Grid />
    <ActionNavigation />
  </div>
</template>