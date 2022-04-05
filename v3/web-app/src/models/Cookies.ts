export interface ICookieHelper {
    getCookies() : ICookie[];
    setCookie(cookie: ICookie) : void;
    getCookie(name: String, value: String) : ICookie;
}

export interface ICookie {
    name: String;
    value: String;
}

export class Cookie implements ICookie {
    constructor(name: String, value: String) {
        this.name = name;
        this.value = value;
    }

    name: String;
    value: String;
}

export class CookieHelper implements ICookieHelper {
    getCookies(): ICookie[] {
        let cookies = new Array<Cookie>();
        let entries = document.cookie.split("; ");
        for(let entry of entries) {
            let splitEntry = entry.split("=");
            cookies.push(this.getCookie(splitEntry[0], splitEntry[1]));
        }

        return cookies;
    }
    
    setCookie(cookie: ICookie) : void {

    }

    getCookie(name: String, value?: String): ICookie {
        return new Cookie(name, value);
    }
    
    
}