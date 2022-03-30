<template>
    <Menu style="width: 100%" :model="items" />
</template>
<script>
import Menu from 'primevue/menu';
import { Inventory } from "../store/inventory";

export default {
    data() {
        let context = this;
        return {
            items: [{
                label:"Sync with server",
                icon: "pi pi-refresh",
                command() {
                    context.sync();
                }
            }]
        }
    },
    components: {
        Menu
    },
    methods: {
        sync() {
            this.$store.dispatch(Inventory.actions.sync);
        }
    }
}
</script>