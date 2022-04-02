<script setup>
    import { toRef } from 'vue';
    import Menu from 'primevue/menu';
    
    let menuItems = [
        { label: "Sign Up", icon: "pi pi-sign-in", command: () => register()  },
        { label: "Sign In", icon: "pi pi-sign-in", command: () => login()  }
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
        <h3>You are a guest</h3>
        <Menu :model="menuItems" />
    </div>
</template>