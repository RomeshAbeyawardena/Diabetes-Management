import axios from "axios"

export default {
    create(baseUrl, headers, timeout) {
        return axios.create({
            baseURL: baseUrl,
            headers: headers,
            timeout: timeout
        });
    }
}