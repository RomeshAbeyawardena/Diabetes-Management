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
        nextDay() {
            let fromDate = dayjs(this.$store.state.Inventory.filters.fromDate);
            let toDate = dayjs(this.$store.state.Inventory.filters.toDate);
            this.$store.commit("setFilters", { 
                fromDate: fromDate.add(1, 'day').toDate(),
                toDate: toDate.add(1, 'day').toDate()
            });
        },

        previousDay() {
            let fromDate = dayjs(this.$store.state.Inventory.filters.fromDate);
            let toDate = dayjs(this.$store.state.Inventory.filters.toDate);
            this.$store.commit("setFilters", { 
                fromDate: fromDate.subtract(1, 'day').toDate(),
                toDate: toDate.subtract(1, 'day').toDate()
            });
        }
    },
    name: "date-navigation"
}
</script>