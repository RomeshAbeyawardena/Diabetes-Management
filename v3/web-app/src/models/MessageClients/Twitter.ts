import { MessageClientBase, MessageClientType } from "./index";

export default class TwitterMessageClient extends MessageClientBase {
    constructor() {
        super(MessageClientType.Twitter);
    }

    send(message: string): void {
        throw new Error("Method not implemented.");
    }

}