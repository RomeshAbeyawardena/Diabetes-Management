<template>
    <div class="grid">
        <div class=".d-none col-3 align-centre" v-if="showHeader">
            <label for="unit-value">Consumed</label>
        </div>
        <div class="col-5 align-centre" v-if="showHeader">
            <label for="inventory-description">Description</label>
        </div>
        <div class="col-2 align-centre" v-if="showHeader">
            <label for="unit-value">Units</label>
        </div>
        <div class="col-2 align-centre" v-if="showHeader">
            <label for="actions">
                <i class="pi pi-user-edit"></i>
            </label>
        </div>
        <div class="col-3 xs-show">
            <InputMask  v-model="formattedTime" 
                        mask="99:99" 
                        slotChar="hh:mm"
                        class="full-width"
                        v-on:click="showCalendar" />
        </div>
        <div class="col-3 xs-hide">
            <InputMask  v-model="formattedDateTime" 
                        mask="99/99/9999 99:99" 
                        slotChar="dd/mm/yyyy hh:mm"
                        class="full-width"
                        v-on:click="showCalendar" />
        </div>
        <div class="col-5">
            <auto-complete id="inventory-description" 
                            v-model="item.description" 
                            v-on:item-select="itemSelected"
                            v-on:complete="searchItems"
                            v-on:input="updateParent"
                            :suggestions="searchResults"
                            field="description"
                            class="w-full" />
        </div>
        <div class="col-2">
            <input-number id="unit-value" 
                            v-model="item.unitValue" 
                            v-on:blur="updateParent" 
                            class="full-width" />
            
        </div>
        <div class="col-2 flex align-items-center justify-content-center">
            <Button icon="pi pi-times" 
                    class="p-button-rounded action-buttons p-button-danger"
                    v-on:click="deleteItem" />
        </div>
    </div>

</template>
<script>
import AutoComplete from 'primevue/autocomplete';
import Button from 'primevue/button/Button';
import InputMask from 'primevue/inputmask';
import InputNumber from 'primevue/inputnumber';
import dayjs from "dayjs";
import InventoryDb from "../db/inventory";
import { Store } from "../store";

let customParseFormat = require('dayjs/plugin/customParseFormat');
dayjs.extend(customParseFormat)

const dateTimeFormat = "DD/MM/YYYY HH:mm";
const timeFormat = "HH:mm"
export default {
    components: {
        AutoComplete,
        Button,
        InputMask,
        InputNumber
    },
    computed: {
        isTimeOnly() {
            return false;
        }
    },
    data() {
        return {
            searchResults: [],
            date: dayjs(this.consumedDate),
            formattedTime: "",
            formattedDateTime: "",
            item: {
                id: this.inventoryId,
                description: this.description,
                unitValue: this.unitValue,
                consumedDate: this.consumedDate,
                state: this.state
            }
        }
    },
    name: "inventory-item",
    props: {
        "consumedDate": Date,
        "state": String,
        "description": String,
        "inventoryId": Number,
        "showHeader": Boolean,
        "readOnly": Boolean,
        "unitValue": Number
    },
    
    methods: {
        showCalendar() {
            let dialog = this.$store.state.dialog;
            dialog.subject = this.inventoryId;
            dialog.display = true;
            dialog.value = this.item.consumedDate;
            dialog.showTime = true;
            dialog.header = "Select date/time";
            this.$store.commit(Store.mutations.setDialogOptions, dialog);
        },
        getDate(date) {
            return date.toDate();
        },
        getDateString(timeOnly) {
    
            if(timeOnly) {
                return this.date.format(timeFormat)
            }

            return this.date.format(dateTimeFormat)
        },
        updateParent() {
            this.$emit("item:updated", {
                id: this.item.id,
                description: this.item.description,
                unitValue: this.item.unitValue,
                consumedDate: this.item.consumedDate,
                state: this.item.state
            });
        },
        async searchItems(event) {
            this.searchResults = await InventoryDb.searchItems(event.query);
        },
        itemSelected(event) {
            this.item.description = event.value.description;
            this.item.unitValue = event.value.unitValue;
            this.updateParent();
        },
        deleteItem(event) {
            this.$emit("item:deleted", event, this.item.id);
        }
    }, created() {
        this.formattedTime = this.getDateString(true);
        this.formattedDateTime = this.getDateString();
        let context = this;
        this.$root.$on("dialog:optionSelected", e => { 
            if(e.subject == this.item.id) { 
                context.item.consumedDate = e.value;
                context.date = dayjs(e.value);
                context.updateParent();
                context.formattedTime = this.getDateString(true);
                context.formattedDateTime = this.getDateString();
            }
        });
    },
}
</script>