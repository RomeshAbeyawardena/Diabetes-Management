<script setup>
import "../../scss/version-picker.scss"
import Dropdown from "primevue/dropdown";
import { onBeforeMount } from "vue-demi";
import { useInventoryStore } from "../../stores/Inventory";
import { computed, ref } from "vue";
import { storeToRefs } from "pinia";
import dayjs from "dayjs";
const dateNow = new Date();
const longDateFormat = "DD MMM HH:mm:ss";
const shortDateFormat = "DD MMM YY HH:mm:ss";
const store = useInventoryStore();
const { currentVersion } = storeToRefs(store);
const selectedInventory = ref({ version: 0, created: new Date() });
const inventoryVersions = ref([]);
const loading = ref(false);
const placeholder = computed(() => {
    if(loading.value){
        return "Loading versions...";
    }

    return "Select version";
});
function formatDate(date)  {
    const d = new Date(date);
    const n = dayjs(dateNow);
    const _date = dayjs(d);
    const format = _date.year() == n.year() 
      ? shortDateFormat
      : longDateFormat;

    return _date.format(format);
}


onBeforeMount(async () => {
  const versions = await store.loadVersions();
  inventoryVersions.value = versions;
});
</script>
<template>
  <div class="version-picker">
    <div class="field grid">
      <div class="col-2">
        <label style="margin-right: 0.5rem">Version:</label>
      </div>
      <div class="col-6">
        <Dropdown
          :filter="true"
          :showClear="true"
          v-model="selectedInventory"
          :options="inventoryVersions"
          placeholder="Select version"
        >
          <template #value="slotProps">
            <div>Version {{ slotProps.value.version }} - {{ slotProps.value.created }}</div>
          </template>
          <template #option="slotProps">
            <div>Version {{ slotProps.option.version }} - {{ slotProps.option.created }}</div>
          </template>
        </Dropdown>
      </div>
    </div>
  </div>
</template>