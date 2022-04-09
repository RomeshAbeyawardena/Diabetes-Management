import { MessageClientBase, MessageClientType } from "./index";

export default class WhatsAppMessageClient extends MessageClientBase {
    constructor() {
        super(MessageClientType.WhatsApp);
    }

    send(message: string): void {
        throw new Error("Method not implemented.");
    }

}