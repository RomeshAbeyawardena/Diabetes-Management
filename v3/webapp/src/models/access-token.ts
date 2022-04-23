import { IResponse } from "./response";

export interface IAccessTokenRequest {
    apiKey: string;
    apiChallenge: string;
    apiIntent: string;
    validate: boolean;
}

export interface IAccessTokenService {
    generate(request: IAccessTokenRequest): Promise<IResponse<string>>;
}