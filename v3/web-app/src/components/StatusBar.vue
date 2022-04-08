<script setup>
    import { storeToRefs } from 'pinia';
    import { useInventoryStore } from '../stores/Inventory';
    import { ref, reactive, watch } from "vue";
    import gsap from "gsap";

    const store = useInventoryStore(); 
    const { currentTotalValue, previousTotalValue } = storeToRefs(store);
    const tweenedPreviousTotalValue = reactive({
        number: 0
    });
    const tweenedTotalValue = reactive({
        number: 0
    });

    watch(currentTotalValue, v => {
        gsap.to(tweenedTotalValue, { duration: 0.5, number: v || 0 });
    });

    watch(previousTotalValue, v => {
        gsap.to(tweenedPreviousTotalValue, { duration: 0.5, number: v || 0 });
    });

</script>
<template>
    <div id="status-bar">
        <div class="grid">
            <div class="col-3">
                <div>
                    <h2>
                        <i class="pi pi-clock historic-value"></i>
                        {{tweenedPreviousTotalValue.number.toFixed(0)}}
                    </h2>
                </div>
            </div>
            <div class="col-6">

            </div>
            <div class="col-3" style="text-align:right">
                <div>
                    <h2>
                        {{tweenedTotalValue.number.toFixed(0)}}
                        <i class="pi pi-bolt current-value"></i></h2>
                </div>
            </div>
        </div>
    </div>
</template>