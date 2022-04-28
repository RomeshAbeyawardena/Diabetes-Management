import { Axios, AxiosRequestConfig } from "axios";
import { IAccessTokenService } from "../models/access-token"
import { IInventoryService } from "../models/inventory";
import { ISessionService } from "../models/session";
import { IUserService } from "../models/user";
import { AccessTokenService } from "./access-token-service";
import { InventoryService } from "./inventory-service";
import { SessionService } from "./session-service";
import { UserService } from "./user-service";
import { IJwtService, JwtService } from "./jwt-service";
export interface IAppServices {
    accessTokenService: IAccessTokenService;
    client: Axios;
    inventoryService: IInventoryService;
    sessionService: ISessionService;
    jWtWebService: IJwtService;
    userService: IUserService;
}

export const useAppServices = function(config: AxiosRequestConfig) : IAppServices {
    const client = new Axios(config);
    return {
        accessTokenService: new AccessTokenService(client),
        client: client,
        inventoryService: new InventoryService(client),
        sessionService: new SessionService(client),
        jWtWebService: new JwtService(client),
        userService: new UserService(client)
    }
}