import { MessageClientBase, MessageClientType } from "./index";

export default class FacebookMessageClient extends MessageClientBase {
    constructor() {
        super(MessageClientType.Facebook);
    }

    send(message: string): void {
        throw new Error("Method not implemented.");
    }

}