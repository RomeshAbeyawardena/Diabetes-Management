import { UrlMessageClientBase, MessageClientType } from "./index";

export default class TwitterMessageClient extends UrlMessageClientBase {
    constructor() {
        super(MessageClientType.Twitter, "https://twitter.com/intent/tweet?");
    }

    prepareUrlWithMessage(url: string, message: string): string {
        return url + "url=" + message;
    }
}