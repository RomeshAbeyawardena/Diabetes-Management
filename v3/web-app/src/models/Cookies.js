export class Cookie {
    constructor() {

    }
    getCookies() {
        return document.cookie.split(";");
    }
    setCookie(name, value) {
        let cookies = this.getCookies();
        let cookieIndex = cookies.findIndex(s => s.trim() == name);
        if(cookieIndex !== -1){
            cookies[cookieIndex] = value;
        }
        else {
            cookies.push()
        }
    }
}