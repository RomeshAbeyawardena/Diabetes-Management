<script setup>
import HelloWorld from './components/HelloWorld.vue'
import Dialog from 'primevue/dialog';
import { ref } from 'vue'
import { useStore } from './stores';

const store = ref(useStore());

</script>

<template>
  <Dialog />
  <HelloWorld msg="Hello Vue 3 + Vite" />
</template>
