<template>
    <div class="title-navigation grid">
        <div class="col-2">
            <Button icon="pi pi-chart-pie" v-on:click="toggleSideBar" class="p-button-rounded p-button-danger" />
        </div>
        <div class="col align-centre flex align-items-center justify-content-center">
            <h3>Diabetic unit's manager</h3>
        </div>
        <div class="col-2 align-right">
            <Button icon="pi pi-chart-line" class="p-button-rounded p-button-help" />
        </div>
    </div>
</template>
<script>
import Button from "primevue/button/Button"
import { Store } from "../store"
export default {
    computed: {
        displaySideBar() {
            return this.$store.state.sideBar.display;
        }
    },
    components: {
        Button
    },
    methods: {
        toggleSideBar() {
            this.$store.commit(Store.mutations.setSideBarDisplay, !this.displaySideBar);
        }
    }
}
</script>