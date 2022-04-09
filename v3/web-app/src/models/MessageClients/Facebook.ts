import { UrlMessageClientBase, MessageClientType } from "./index";

export default class FacebookMessageClient extends UrlMessageClientBase {
    constructor() {
        super(MessageClientType.Facebook, "https://facebook.com/sharer/sharer.php?u=");
    }

    prepareUrlWithMessage(url: string, message: string): string {
        return url + message;
    }
}