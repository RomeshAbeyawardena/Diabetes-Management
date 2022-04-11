<script setup>
import Menu from "primevue/menu";
import "../../scss/menu-sidebar.scss";

import { toRef } from "vue";
import { useUserStore } from "../../stores/User";
import { storeToRefs } from "pinia";

const store = useUserStore();
const { displayName } = storeToRefs(store);
const menuItems = [
  { label: "Sync", icon: "pi pi-sign-in", command: () => register() },
  { label: "Restore", icon: "pi pi-sign-in", command: () => login() },
];

const items = toRef(menuItems);

function login() {
  console.log("login");
}

function register() {
  console.log("register");
}
</script>
<template>
  <div class="sidebar-menu">
    <h3>Options</h3>
    <h4>Welcome {{ displayName }}</h4>
    <Menu :model="menuItems" />
  </div>
</template>