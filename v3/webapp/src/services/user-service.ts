import { IResponse } from "../models/response";
import { IRegisterRequest, IUser, IUserService } from "../models/user";
import { ApiClient } from "./api-client-base";
export const baseUrl = "user";

export class UserService extends ApiClient implements IUserService {
    async register(request: IRegisterRequest): Promise<IResponse<IUser>> {
        const form = this.processForm(request);
        const response = await this.client.post<IResponse<IUser>>(baseUrl, form);
        return response.data;
    }

}