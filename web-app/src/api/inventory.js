import Api from "./index";

let api = null;
export default {
    api: null,
    create(baseUrl, apiKey) {
        this.api = Api.create(baseUrl, {
            "x-api-key": apiKey
        }, 5000);

        api = this.api;
    },
    inventory: {
        baseUrl: "api/",
        async get(userId, key, type, version) {
            return await api.get(this.baseUrl + "GetInventory", { params:{ userId, key, type, version } });
        },
        async post(items) {
            var formData = new FormData();
            
            for(let v in items){
                formData.append(v, items[v]);
            }

            return await api.post(this.baseUrl + "SaveInventory", formData);
        }
    }
}