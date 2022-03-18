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
            <label for="actions">Actions</label>
        </div>
        <div class=".d-none col-3">
            <Calendar  v-model="item.consumedDate" 
                        :touchUI="false"
                        :timeOnly="isTimeOnly"
                        :showTime="!isTimeOnly"
                        dateFormat="dd/mm/yy"
                        class="full-width"
                        v-on:blur="updateParent" />
        </div>
        <div class="col-5">
            <auto-complete id="inventory-description" 
                            v-model="item.description" 
                            v-on:item-select="itemSelected"
                            v-on:complete="searchItems"
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
import InputNumber from 'primevue/inputnumber';
import dayjs from "dayjs";
import Inventory from "../db/inventory";
import Calendar from 'primevue/calendar';
let customParseFormat = require('dayjs/plugin/customParseFormat');
dayjs.extend(customParseFormat)

const dateTimeFormat = "DD-MM-YYYY HH:mm";

export default {
    components: {
        AutoComplete,
        Button,
        Calendar,
        InputNumber
    },
    computed: {
        clientSize() {
            return this.$store.state.clientSize;
        },
        isTimeOnly() {
            return this.clientSize.width < 400;
        }
    },
    name: "inventory-item",
    props: {
        "consumedDate": Date,
        "description": String,
        "inventoryId": Number,
        "showHeader": Boolean,
        "readOnly": Boolean,
        "unitValue": Number
    },
    data() {
        return {
            searchResults: [],
            date: dayjs(this.consumedDate),
            item: {
                id: this.inventoryId,
                description: this.description,
                unitValue: this.unitValue,
                consumedDate: this.consumedDate
            }
        }
    },
    methods: {
        getDate(date) {
            return date.toDate();
        },
        getDateString() {
            return this.date.format(dateTimeFormat)
        },
        updateParent() {
            this.$emit("item:updated", {
                id: this.item.id,
                description: this.item.description,
                unitValue: this.item.unitValue,
                consumedDate: this.item.consumedDate
            });
        },
        async searchItems(event) {
            this.item.description = event.query;
            this.searchResults = await Inventory.searchItems(event.query);
        },
        itemSelected(event) {
            this.item.description = event.value.description;
            this.item.unitValue = event.value.unitValue;
            this.updateParent();
        },
        deleteItem() {
            this.$emit("item:deleted", this.item.id);
        }
    }
}
</script>