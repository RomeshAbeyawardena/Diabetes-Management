<script setup>
    import Menu from 'primevue/menu';
    
    import { useStore } from '../../stores/main';
    import { toRef } from 'vue';
import { DialogType } from '../../models';
    const store = useStore();
    const menuItems = [
        { label: "Sign Up", icon: "pi pi-sign-in", command: () => register()  },
        { label: "Sign In", icon: "pi pi-sign-in", command: () => login()  }
    ];
    
    const items = toRef(menuItems);
    
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
    <div>
        <h3>You are a guest</h3>
        <Menu :model="menuItems" />
    </div>
</template>