<script setup>
    import Knob from 'primevue/knob';

    import { ref, watch } from 'vue';
    const props = defineProps({ value: Number });
    const emit = defineEmits(['value:updated']);
    const value = ref(props.value);

    watch(value, (newValue) => {
        emit("value:updated", newValue);
    });
</script>
<template>
    <div style="text-align:center">
    <Knob v-model="value" :step="1" :min="1" :max="100" />
    </div>
</template>