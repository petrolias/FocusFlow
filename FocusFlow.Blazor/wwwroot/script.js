window.setCookie = function (name, value, minutes) {
    const expires = new Date(Date.now() + minutes * 60000).toUTCString();
    document.cookie = `${name}=${value}; expires=${expires}; path=/`;
};

window.clearCookiesAndReload = () => {
    // Clear all cookies
    document.cookie.split(";").forEach(c => {
        document.cookie = c
            .replace(/^ +/, "")
            .replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
    });        
};

window.cookieManager = {
    get: function (name) {
        const value = "; " + document.cookie;
        const parts = value.split("; " + name + "=");
        if (parts.length === 2) return parts.pop().split(";").shift();
        return null;
    }
};