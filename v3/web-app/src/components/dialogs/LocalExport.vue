<script setup>
import Button from "primevue/button";
import ProgressSpinner from "primevue/progressspinner";
import { MessageClientType } from "../../models/MessageClients";
import QRCodeStyling from "qr-code-styling";
import { onMounted, ref } from "vue";
import { useStore } from "../../stores/main";
import "../../scss/local-export.scss";
import "../../scss/social-media.scss";

const store = useStore();
const loading = ref(false);
const qr = ref(null);

const props = defineProps({
  value: String,
});

const logoUrl = window.location + "android-chrome-512x512.png";
const dataUrl = window.location + "?sc=" + props.value;
const qrOptions = new QRCodeStyling({
  width: 300,
  height: 300,
  type: "svg",
  data: dataUrl,
  image: logoUrl,
  dotsOptions: {
    color: "#fff",
    type: "rounded",
  },
  backgroundOptions: {
    color: "#20262e",
  },
  imageOptions: {
    crossOrigin: "anonymous",
    margin: 20,
  },
});

function toggleLoadingClass() {
  const css = "qr-code";

  if (loading.value) {
    return css + " isloading";
  }

  return css;
}

function shareLink(messageClientType){
  store.messageClientSender.send(messageClientType, dataUrl);
}

onMounted(async () => {
  loading.value = true;
  qrOptions.append(qr.value);
  await qrOptions._svgDrawingPromise;
  loading.value = false;
});
</script>

<template>
  <div class="local-save">
    <ProgressSpinner
      v-if="loading"
      style="width: 50px; height: 50px"
      strokeWidth="8"
      fill="var(--surface-ground)"
      animationDuration=".5s"
    />
    <div ref="qr" :class="toggleLoadingClass()"></div>
    <div class="social-media-nav">
      <div class="grid">
        <div class="col">
          <Button @click="shareLink(MessageClientType.WhatsApp)"
            icon="pi pi-whatsapp"
            class="p-button-rounded p-button-whatsapp"
          ></Button>
        </div>
        <div class="col">
          <Button @click="shareLink(MessageClientType.Twitter)"
            icon="pi pi-twitter"
            class="p-button-rounded p-button-twitter"
          ></Button>
        </div>
        <div class="col">
          <Button @click="shareLink(MessageClientType.Facebook)"
            icon="pi pi-facebook"
            class="p-button-rounded p-button-facebook"
          ></Button>
        </div>
        <div class="col">
          <Button @click="shareLink(MessageClientType.Email)"
            icon="pi pi-send"
            class="p-button-rounded p-button-primary"
          ></Button>
        </div>
        <div class="col">
          <Button @click="shareLink(MessageClientType.Download)"
            icon="pi pi-download"
            class="p-button-rounded p-button-primary"
          ></Button>
        </div>
      </div>
    </div>
  </div>
</template>