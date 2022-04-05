<script setup>
    import Button from "primevue/button";
    import InputSwitch from 'primevue/inputswitch';
    import TabView from 'primevue/tabview';
    import TabPanel from 'primevue/tabpanel';
    import { ref } from "vue";
    import { storeToRefs } from "pinia";
    import { useStore } from "../../stores";

    import "../../scss/cookie-policy.scss";
    const store = useStore();
    const { consent } = storeToRefs(store);
    const activeIndex = ref(0);
    
    function editConsent() {
        consent.value.hasConsented = false;
        consent.value.enableMarketing = false;        
    }

    function customiseConsent() {
        activeIndex.value = 1;
    }

    function acceptConsent() {
        consent.value.hasConsented = true;
        store.voidDialogValue();
    }

    function denyConsent() {
        window.location.replace("https://www.google.co.uk/search?q=gdpr")
    }

</script>
<template>
<div class="cookie-policy">
    <TabView v-model:activeIndex="activeIndex">
        <TabPanel header="Consent">
            <h2>This website uses cookies</h2>
            <p>We use cookies to personalise content and ads, to provide social media features and to analyse our traffic. </p>
            <p>We also share information about your use of our site with our social media, advertising and analytics partners 
                who may combine it with other information that you`ve provided to them or that they`ve collected from your use of their 
                services.</p>
        </TabPanel>
        <TabPanel header="Details">
            <div class="grid">
                <div class="col-3">
                    <InputSwitch v-model="consent.enableNecessary" class="p-disabled" />
                </div>
                <div class="col-9">
                    <h3>Necessary</h3>
                    <p>Necessary cookies help make a website usable by enabling basic functions like page navigation and access to secure 
                        areas of the website. The website cannot function properly without these cookies.</p>
                </div>
            </div>
            <div class="grid">
                <div class="col-3">
                    <InputSwitch v-model="consent.enableMarketing" />
                </div>
                <div class="col-9">
                    <h3>Marketing</h3>
                    <p>Marketing cookies are used to track visitors across websites. 
                        The intention is to display ads that are relevant and engaging for the individual user and thereby more valuable for 
                        publishers and third party advertisers.</p>
                </div>
            </div>
        </TabPanel>
        <TabPanel header="About">
            <p>Cookies are small text files that can be used by websites to make a user's experience more efficient.</p>
            <p>The law states that we can store cookies on your device if they are strictly necessary for the operation of this site. 
                For all other types of cookies we need your permission.</p>
            <p>This site uses different types of cookies. Some cookies are placed by third party services that appear on our pages.</p>
            <p>You can at any time change or withdraw your consent from the Cookie Declaration on our website.</p>
            <p>Learn more about who we are, how you can contact us and how we process personal data in our Privacy Policy.</p>
            <p>Please state your consent ID and date when you contact us regarding your consent.</p>
        </TabPanel>
    </TabView>
    <div style="text-align:right">
        <Button class="p-button-danger" icon="pi pi-times" label="Deny" v-on:click="denyConsent" v-if="!consent.hasConsented" />
        <Button icon="pi pi-pencil" label="Customise" v-on:click="customiseConsent" v-if="!consent.hasConsented && activeIndex !== 1" />
        <Button class="p-button-success" icon="pi pi-check" label="Accept all" v-on:click="acceptConsent" v-if="!consent.hasConsented && activeIndex !== 1" />
        <Button class="p-button-success" icon="pi pi-check" label="Save" v-on:click="acceptConsent" v-if="!consent.hasConsented && activeIndex === 1" />
        <Button label="Edit" v-if="consent.hasConsented" />
        <Button label="Close" v-if="consent.hasConsented" />
    </div>
</div>
</template>