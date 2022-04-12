<script setup>
import Menu from "primevue/menu";
import "../../scss/menu-sidebar.scss";

import { toRef } from "vue";
import { useUserStore } from "../../stores/User";
import { storeToRefs } from "pinia";
import { useInventoryStore } from "../../stores/Inventory";

const store = useUserStore();
const inventoryStore = useInventoryStore();
const { displayName } = storeToRefs(store);
const menuItems = [
  { label: "Sync", icon: "pi pi-sign-in", command: () => sync() },
  { label: "Restore", icon: "pi pi-sign-in", command: () => restore() },
];

const items = toRef(menuItems);

async function restore() {
  await inventoryStore.saveVersion();
}

async function sync() {
  await inventoryStore.saveVersion();
}
</script>
<template>
  <div class="sidebar-menu">
    <h3>Options</h3>
    <h4>Welcome {{ displayName }}</h4>
    <Menu :model="menuItems" />
  </div>
</template>