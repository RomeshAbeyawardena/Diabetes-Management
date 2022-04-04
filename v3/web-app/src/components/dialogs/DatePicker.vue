<script setup>
    import Carousel from 'primevue/carousel';
    import Calendar from "primevue/calendar";

    import { ref, watch } from 'vue';
    
    const props = defineProps({ value: Date });
    const emit = defineEmits(['value:updated']);
    const value = ref(props.value);

    const responsiveOptions = ref([
			{
				breakpoint: '1024px',
				numVisible: 3,
				numScroll: 3
			},
			{
				breakpoint: '600px',
				numVisible: 2,
				numScroll: 2
			},
			{
				breakpoint: '480px',
				numVisible: 1,
				numScroll: 1
			}
		]);

    const items = ref([
        { timeOnly: true, style: { "margin-top": "-50px", display:"block", top:"50%", width: "calc(100% - 10px)"  } },
        { timeOnly: false, style: { width: "100%", display:"block"  } }
    ]);

    watch(value, (newValue) => {
        emit("value:updated", newValue);
    });

</script>
<template>
    <Carousel :value="items" :responsiveOptions="responsiveOptions" :numVisible="1">
        <template #item="slotProps">
            <Calendar v-model="value" :inline="true" :style="slotProps.data.style" :timeOnly="slotProps.data.timeOnly" />
        </template>
    </Carousel>
</template>