<script setup>
    import { useInventoryStore } from '../stores/inventory';
    import { computed } from "vue";

    const store = useInventoryStore();
    const currentTotalValue = computed(() => store.currentTotalValue);
    const previousTotalValue = computed(() => store.previousTotalValue);
</script>
<template>
    <div id="status-bar">
        <div class="grid">
            <div class="col-3">
                <div>
                    <h3>
                        <i class="pi pi-clock historic-value"></i>
                        {{previousTotalValue}}
                    </h3>
                </div>
            </div>
            <div class="col-6">

            </div>
            <div class="col-3" style="text-align:right">
                <div>
                    <h3>
                        {{currentTotalValue}}
                        <i class="pi pi-bolt current-value"></i></h3>
                </div>
            </div>
        </div>
    </div>
</template>