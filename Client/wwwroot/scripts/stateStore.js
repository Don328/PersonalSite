var stateStore = {}

stateStore.getSessionStorage = function (key) {
    return sessionStorage.getItem(key);
}

stateStore.setSessionStorage = function (key, data) {
    sessionStorage.setItem(key, data);
}