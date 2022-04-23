import { IResponse } from "./response";

export interface IUser {
    userId:number;
    displayName:string;
    emailAddress:string;
    verified:boolean;
    created:Date;
    modified:Date;
}

export interface IRegisterRequest {
    displayName:string;
    emailAddress:string;
    password:string;
}

export interface ILoginRequest {
    emailAddress:string;
    password:string;
}

export interface IUserService {
    register(request: IRegisterRequest):Promise<IResponse<IUser>>;
}