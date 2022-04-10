export interface IApiHelper {
    ConvertToFormData(value: object) : FormData;
}

export class ApiHelper {
    ConvertToFormData(value: object) : FormData {
        const formData = new FormData();

        for (const key in value) {
            if (Object.prototype.hasOwnProperty.call(value, key)) {
                const element = value[key];
                formData.append(key, element);
            }
        }

        return formData;
    }
}