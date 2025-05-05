window.setCookie = function (name, value, minutes) {
    const expires = new Date(Date.now() + minutes * 60000).toUTCString();
    document.cookie = `${name}=${value}; expires=${expires}; path=/`;
};

window.loginUser = async function (url, jsonString) {
    const data = JSON.parse(jsonString);
    const response = await fetch(url, {
        method: "POST",
        credentials: "include", // 🔥 important for cookies
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });
    return response.status;
};