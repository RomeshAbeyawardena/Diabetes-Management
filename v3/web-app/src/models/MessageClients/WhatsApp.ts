import { IMessageClientFactory, UrlMessageClientBase, MessageClientType } from "./index";

export default class WhatsAppMessageClient extends UrlMessageClientBase {
    constructor() {
        super(MessageClientType.WhatsApp, "whatsapp://send?text=");
    }

    prepareUrlWithMessage(url: string, message: string): string {
        return url + message;
    }

}