<script setup>
    import InputMask from 'primevue/inputmask';
    import InputNumber from 'primevue/inputnumber';
    import InputText from 'primevue/inputtext';
    import { ref, computed } from "vue";
    import { Inventory } from "../models/Inventory";
    import dayjs from "dayjs";
    import customParseFormat from 'dayjs/plugin/customParseFormat';
    dayjs.extend(customParseFormat)

    const props = defineProps({ 
        showHeader: Boolean, 
        entry: Inventory 
    });

    const showHeader = ref(props.showHeader);
    const localEntry = ref(props.entry);
    const format = "DD/MM/YYYY HH:mm";
    const inputFormat = "99/99/9999 99:99";
    const inputDate = computed({
        get() {
            return dayjs(localEntry.value.inputDate).format(format);
        },
        set(value) {
            let newDate = dayjs(value, format);
            if(newDate.isValid())
            {
                localEntry.value.inputDate = newDate.toDate();
            }
        }
    });

    const inputValue = computed({
        get() {
            let value = localEntry.value.value;
            if(value != null || value != undefined)
            {
                return value.toString();
            }

            return "";
        },
        set(value) {
            if((value != null || value != undefined) && !isNaN(value)) {
                localEntry.value.value = Number(value);
            }
        }
    })
</script>

<template>
    <div class="grid" v-if="showHeader">
        <div class="col-4">
            <label for="inputDate">Date</label>
        </div>
        <div class="col-6">
            <label for="description">Description</label>
        </div>
        <div class="col-2">
            <label for="value">Value</label>
        </div>
    </div>
    <div class="grid">
        <div class="col-4">
            <InputMask id="inputDate" style="width: 100%"
                    :mask="inputFormat"
                    v-model="inputDate" />
        </div>
        <div class="col-6">
            <InputText id="description" type="text" style="width: 100%"
                v-model="localEntry.description" />
        </div>
        <div class="col-2">
            <InputText id="value"  v-model="inputValue" 
                        type="number" 
                        style="width: 100%" />
        </div>
    </div>
</template>