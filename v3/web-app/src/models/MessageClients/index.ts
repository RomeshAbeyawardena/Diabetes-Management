export enum MessageClientType {
    Facebook = "facebook",
    Twitter = "twitter",
    WhatsApp = "whatsapp",
    Email = "email",
    Download = "download"
}

export interface IMessageClient {
    type: MessageClientType;
    register(messageClientFactory: IMessageClientFactory): void;
    canSend(messageClientType: MessageClientType): boolean;
    send(message: string) : void;
}

export abstract class MessageClientBase implements IMessageClient {
    type: MessageClientType;

    constructor(messageClientType: MessageClientType) {
        this.type = messageClientType;
    }

    canSend(messageClientType: MessageClientType): boolean {
        return this.type == messageClientType;
    }

    register(messageClientFactory: IMessageClientFactory) : void {
        messageClientFactory.set(this);
    }
    
    abstract send(message: string) : void;
}

export interface IMessageClientFactory {
    messageClients: IMessageClient[];

    get(messageClientType: MessageClientType) : IMessageClient[];
    set(messageClient : IMessageClient): void;
}

export interface IMessageClientSender {
    messageClientFactory: IMessageClientFactory;

    send(messageClient: MessageClientType, message: string): void;
    register(messageClient: IMessageClient): void
}

export class MessageClientFactory implements IMessageClientFactory {
    messageClients: IMessageClient[];
    
    constructor() {
        this.messageClients = new Array<IMessageClient>();
    }

    get(messageClientType: MessageClientType): IMessageClient[] {
        return this.messageClients.filter(m => m.canSend(messageClientType));
    }
    set(messageClient: IMessageClient): void {
        this.messageClients.push(messageClient);
    }
}

export class MessageClientSender implements IMessageClientSender {
    messageClientFactory: IMessageClientFactory;

    constructor(messageClientFactory: IMessageClientFactory) {
        this.messageClientFactory = messageClientFactory;
    }

    send(messageClientType: MessageClientType, message: string): void {
        const messageClients = this.messageClientFactory.get(messageClientType);
        messageClients.forEach(m => m.send(message));
    }

    register(messageClient: IMessageClient): void {
        this.messageClientFactory.set(messageClient);
    }

}