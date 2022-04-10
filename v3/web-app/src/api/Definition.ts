export interface IApiEndpointDefinition {
    apiKey: string;
    description: string;
    method: string;
    name: string;
}

export interface IApiDefinition {
    baseUrl: string;
    endpoints: IApiEndpointDefinition[];
}

export interface IApiDefinitions {
    inventory: IApiDefinition 
}