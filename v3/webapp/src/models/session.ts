import { ILoginRequest, IUser } from "./user";
import { IResponse } from "./response";

export interface ISession {
    sessionId:number;
    userId:number;
    created:Date;
    expires:Date;
    enabled:boolean;
    user:IUser;
}

export interface ISessionService {
    login(loginRequest: ILoginRequest): Promise<IResponse<ISession>>;
}