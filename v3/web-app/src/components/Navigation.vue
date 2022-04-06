<script setup>
    import Button from 'primevue/button';
    import { computed } from 'vue';
    import { useStore } from '../stores/main';
    const store = useStore();
    const dateFormat = "ddd, DD MMMM YYYY";
    const currentDate = computed(() => store.filters.dateRange.display(dateFormat, true));
    function setDateFilter(action) {
        var dateRange = store.filters.dateRange[action](1, "day");
        //console.log(dateRange);
        store.filters.dateRange = dateRange;
    }
</script>
<template>
    <div id="navigation" class="grid justify-content-center align-items-center">
        <div class="col-2">
            <Button icon="pi pi-angle-double-left" v-on:click="setDateFilter('subtract')"
                    class="p-button-rounded p-button-outlined" />
        </div>
        <div class="col-8">
            <h4>{{currentDate}}</h4>
        </div>
        <div class="col-2">
            <Button icon="pi pi-angle-double-right" v-on:click="setDateFilter('add')"
                    class="p-button-rounded p-button-outlined" /> 
        </div>
    </div>
</template>