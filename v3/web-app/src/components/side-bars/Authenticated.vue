<script setup>
import Menu from "primevue/menu";
import "../../scss/menu-sidebar.scss";

import { toRef } from "vue";
import { useStore } from "../../stores/main";
import { useUserStore } from "../../stores/User";
import { storeToRefs } from "pinia";
import { useInventoryStore } from "../../stores/Inventory";
import { DialogType } from "../../models";
const store = useStore();
const userStore = useUserStore();
const inventoryStore = useInventoryStore();
const { displayName } = storeToRefs(userStore);
const menuItems = [
  { label: "Sync", icon: "pi pi-sign-in", command: () => sync() },
  { label: "Restore", icon: "pi pi-sign-in", command: () => restore() },
];

const items = toRef(menuItems);

function restore() {
  const dialog = store.getDialog(DialogType.VersionPicker);
  store.showDialog(dialog, null, true);
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