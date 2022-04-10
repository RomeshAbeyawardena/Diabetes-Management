<script setup>
    import { toRef } from 'vue';
    import Menu from 'primevue/menu';
import { useUserStore } from '../../stores/User';
import { storeToRefs } from 'pinia';
    
    const store = useUserStore();
    const { displayName } = storeToRefs(store);
    const menuItems = [
        { label: "Sync", icon: "pi pi-sign-in", command: () => register()  },
        { label: "Restore", icon: "pi pi-sign-in", command: () => login()  }
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
    <div>
        <h3>You are authenticated</h3>
        <h4>Welcome {{ displayName }} [Logout]</h4>
        <Menu :model="menuItems" />
    </div>
</template>