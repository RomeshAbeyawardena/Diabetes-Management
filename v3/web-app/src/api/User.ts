import { IResponse, ApiBaseWithHeader } from ".";
import { IApiHelper } from "../plugins/ApiHelper";
import { IApiDefinition } from "./Definition";

export interface IUser {
    userId: string;
	displayName: string;
	displayNameCaseSignature: string;
	emailAddress: string;
	emailAddressCaseSignature: string;
	password: string;
	hash: string;
	created: Date,
	modified: Date
}

export interface IUserApi {
    login(loginRequest: ILoginRequest) : Promise<IResponse<IUser>>;
    register(registerRequest: IRegisterRequest) : Promise<IResponse<IUser>>;
}

export interface ILoginRequest {
    emailAddress: string;
    password: string;
}

export interface IRegisterRequest extends ILoginRequest {
    displayName: string;
}

export class UserApi extends ApiBaseWithHeader implements IUserApi {
    constructor(apiHelper: IApiHelper, apiDefinition: IApiDefinition) {
        super(apiHelper, apiDefinition)
    }

    async login(loginRequest: ILoginRequest): Promise<IResponse<IUser>> {
        const formData = this.apiHelper.ConvertToFormData(loginRequest);
        const response = await this.client.post<IResponse<IUser>>("user", formData);
        return response.data;

    }
    async register(registerRequest: IRegisterRequest): Promise<IResponse<IUser>> {
        const formData = this.apiHelper.ConvertToFormData(registerRequest);
        const response = await this.client.post<IResponse<IUser>>("user", formData);
        return response.data;
    }
}