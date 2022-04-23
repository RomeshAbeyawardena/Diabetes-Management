export interface IResponse<T> {
    statusCode: number;
    statusMessage: string;
    data:T;
}