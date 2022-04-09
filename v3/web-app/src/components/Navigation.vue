<script setup>
import Button from "primevue/button";
import { computed } from "vue";
import { useStore } from "../stores/main";
import { storeToRefs } from "pinia";
import { DialogType } from "../models";
import { DateRange } from "../models/DateRange";

const store = useStore();
const { blockEvents } = storeToRefs(store);
const dateFormat = "ddd, DD MMMM YYYY";
const currentDate = computed(() =>
  store.filters.dateRange.display(dateFormat, true)
);

async function selectDate() {
  const result = await store.showDialog(
    store.getDialog(DialogType.DatePicker),
    store.filters.dateRange.fromDate,
    true
  );
  
  store.filters.dateRange = store.filters.dateRange.set(result);
}

function setDateFilter(action) {
  const dateRange = store.filters.dateRange[action](1, "day");
  store.filters.dateRange = dateRange;
}
</script>
<template>
  <div id="navigation" class="grid justify-content-center align-items-center">
    <div class="col-2">
      <Button
        icon="pi pi-angle-double-left"
        :disabled="blockEvents"
        @click="setDateFilter('subtract')"
        class="p-button-rounded p-button-outlined"
      />
    </div>
    <div class="col-8">
      <h4 @click="selectDate">{{ currentDate }}</h4>
    </div>
    <div class="col-2">
      <Button
        icon="pi pi-angle-double-right"
        :disabled="blockEvents"
        @click="setDateFilter('add')"
        class="p-button-rounded p-button-outlined"
      />
    </div>
  </div>
</template>