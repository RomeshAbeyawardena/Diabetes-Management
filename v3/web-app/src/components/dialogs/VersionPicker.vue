<script setup>
import MultiSelect from 'primevue/multiselect';
const selectedInventory = ref(null);
const inventories = ref([]);
</script>
<template>
    <MultiSelect v-model="selectedInventory" :options="inventories" optionLabel="version" placeholder="Select version" />
</template>