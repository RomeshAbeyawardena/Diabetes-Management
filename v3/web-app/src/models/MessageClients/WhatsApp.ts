import { IMessageClientFactory, MessageClientBase, MessageClientType } from "./index";

export default class WhatsAppMessageClient extends MessageClientBase {
    constructor() {
        super(MessageClientType.WhatsApp);
    }

    send(message: string): void {
        window.open("whatsapp://send?text=" + encodeURI(message), "_blank");
    }

}