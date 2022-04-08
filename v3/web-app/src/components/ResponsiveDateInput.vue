<script setup>
    import InputMask from 'primevue/inputmask';
    import { ref, computed, watch } from "vue";
    import dayjs from "dayjs";
    import customParseFormat from 'dayjs/plugin/customParseFormat';
    import "../scss/responsive-date-input.scss";
    dayjs.extend(customParseFormat);

    const props = defineProps({ 
        disabled: Boolean,
        modelValue: Date, 
        dateFormat: String,
        format: String, 
        mobileFormat: String });

    const emit = defineEmits(['update:modelValue', 'input:click']);
    const disabled = ref(props.disabled);
    const value = ref(props.modelValue);
    const val = ref(new dayjs(props.modelValue).format(props.format));
    const mobileVal = ref(new dayjs(props.modelValue).format(props.mobileFormat));

    watch(() => props.modelValue, (newValue, oldValue) => {
        val.value = new dayjs(newValue).format(props.format);
        mobileVal.value = new dayjs(newValue).format(props.mobileFormat);
    });


    function input(event) {
        let isValid = event !== val.value && event.length && !event.includes("_");
        if(isValid) {
            let newDate = dayjs(event, props.format);
            if(newDate.isValid())
            {
                let d = newDate.toDate()
                emit('update:modelValue', d);
                value.value = d;
                mobileVal.value = newDate.format(props.mobileFormat);
            }
        }
    }

    function inputMobile(event) {
        let isValid = event !== mobileVal.value && event.length && !event.includes("_");
        if(isValid) {
            var date = dayjs(props.modelValue).format(props.dateFormat);

            let newDate = dayjs(date.concat(" ", event), props.format);
            if(newDate.isValid())
            {
                let d = newDate.toDate();
                emit('update:modelValue', d);
                value.value = d;
                val.value = newDate.format(props.format);
            }
        }
    }

    function inputClick(event) {
        emit("input:click", event);
    }

    function getMask(input) {
        if(input)
        {
            let r = new RegExp("[A-z]", "g");
            return input.replace(r, "9");
        }

        return input;
    }
</script>
<template>
    <div class="responsive-date-input">
        <InputMask class="inputDate" 
            style="width: 100%" 
            @update:model-value="input($event)" 
            @click="inputClick($event)"
            :mask="getMask(props.format)"
            v-model="val" />
        <InputMask :disabled="disabled" class="inputMobileDate" 
            style="width: 100%" 
            @update:model-value="inputMobile($event)" 
            @click="inputClick($event)"
            :mask="getMask(props.mobileFormat)"
            v-model="mobileVal" />
    </div>
</template>