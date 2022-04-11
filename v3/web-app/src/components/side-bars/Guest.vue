<script setup>
import Menu from "primevue/menu";
import "../../scss/menu-sidebar.scss";
import { useStore } from "../../stores/main";
import { ref } from "vue";
import { DialogType } from "../../models";
import { useInventoryStore } from "../../stores/Inventory";
import { storeToRefs } from "pinia";
const store = useStore();
const inventoryStore = useInventoryStore();
const { sideBar } = storeToRefs(store);
const { readonlyItems, isReadonly } = storeToRefs(inventoryStore);

const menuItems = ref([
  { label: "Sign Up", icon: "pi pi-sign-in", command: () => register() },
  { label: "Sign In", icon: "pi pi-sign-in", command: () => login() },
]);

if(isReadonly.value) {
    menuItems.value.push({
         label: "Leave read-only mode", icon: "pi pi-sign-in", command: () => leaveReadonlyMode() 
    });
}

function leaveReadonlyMode() {
    isReadonly.value = false;
    readonlyItems.value = undefined;
    store.sideBar.visible = false;
}

function login() {
  const loginDialog = store.getDialog(DialogType.Login);
  store.showDialog(loginDialog, null, false);
  store.sideBar.visible = false;
}

function register() {
  const registerDialog = store.getDialog(DialogType.Register);
  store.showDialog(registerDialog, null, false);
  store.sideBar.visible = false;
}
</script>
<template>
  <div class="sidebar-menu">
    <h3>Options</h3>
    <Menu :model="menuItems" />
  </div>
</template>