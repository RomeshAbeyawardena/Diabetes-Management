<script setup>
    import { ref, watch } from 'vue';
    import Textarea from 'primevue/textarea';

    const props = defineProps({ value: String });
    const emit = defineEmits(['value:updated']);
    const value = ref(props.value);

    watch(value, (newValue) => {
        emit("value:updated", newValue);
    });
</script>
<template>
    <Textarea style="width:100%; margin-bottom:1rem" v-model="value" :autoResize="false" rows="8" cols="30" />
</template>