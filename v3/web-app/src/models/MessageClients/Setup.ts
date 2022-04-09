import { MessageClientFactory, IMessageClientFactory, IMessageClientSender, MessageClientSender } from ".";
import FacebookMessageClient from "./Facebook";
import TwitterMessageClient from "./Twitter";
import WhatsAppMessageClient from "./WhatsApp";
import { IPluginBuilder } from "../../plugins/Plugin";

export interface IMessageClientPlugin {
    messageClientFactory: IMessageClientFactory
    messageClientSender: IMessageClientSender
}

export class MessageClientPlugin implements IMessageClientPlugin, IPluginBuilder<IMessageClientPlugin> {
    messageClientFactory: IMessageClientFactory
    messageClientSender: IMessageClientSender

    constructor() {
        this.messageClientFactory = new MessageClientFactory();
        new FacebookMessageClient()
            .register(this.messageClientFactory);
        new TwitterMessageClient()
            .register(this.messageClientFactory);
        new WhatsAppMessageClient()
            .register(this.messageClientFactory);

        this.messageClientSender = new MessageClientSender(this.messageClientFactory);
    }

    build(): IMessageClientPlugin {
        return this;
    }
}