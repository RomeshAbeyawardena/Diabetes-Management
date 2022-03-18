<template>
    <div class="date-navigation grid mb-4">
        <div class="col-2">
        <Button v-on:click="previousDay"
                icon="pi pi-angle-double-left" 
                class="p-button-rounded p-button-success" />
        </div>
        <div class="col-8 align-centre flex align-items-center justify-content-center">
            <h4 class="mt-0 mb-0">{{ fromDate }}</h4>
        </div>
        <div class="col-2 align-right">
            <Button v-on:click="nextDay"
                    icon="pi pi-angle-double-right" 
                    class="p-button-rounded p-button-success" />
        </div>
    </div>
</template>
<script>
import Button from "primevue/button/Button"
import dayjs from "dayjs";
import { Inventory } from "../store/inventory";

export default {
    components: {
        Button
    },
    computed: {
        fromDate() {
            return dayjs(this.$store.state.Inventory.filters.fromDate)
                .format("ddd, DD MMMM YYYY");
        }
    },
    methods: {
        async nextDay() {
            let fromDate = dayjs(this.$store.state.Inventory.filters.fromDate);
            let toDate = dayjs(this.$store.state.Inventory.filters.toDate);
            this.$store.commit(Inventory.mutations.setFilters, { 
                fromDate: fromDate.add(1, 'day').toDate(),
                toDate: toDate.add(1, 'day').toDate()
            });

            await this.$store.dispatch(Inventory.actions.getItems);
        },

        async previousDay() {
            let fromDate = dayjs(this.$store.state.Inventory.filters.fromDate);
            let toDate = dayjs(this.$store.state.Inventory.filters.toDate);
            this.$store.commit(Inventory.mutations.setFilters, { 
                fromDate: fromDate.subtract(1, 'day').toDate(),
                toDate: toDate.subtract(1, 'day').toDate()
            });

            await this.$store.dispatch(Inventory.actions.getItems, "inventory");
        }
    },
    name: "date-navigation"
}
</script>