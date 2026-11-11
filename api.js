const API = "https://localhost:5000/api";

async function apiGet(path) {
    const res = await fetch(API + path);
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

async function apiPost(path) {
    const res = await fetch (API + path, {method: "POST"});
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

// TODO: add one function per endpoint, for example:
// const getState    = () => apiGet("/game/state");
// const newGame     = () => apiPost("/game/new");
// const randomEvent = () => apiGet("/events/random");
// const choose      = (slot) => apiPost(`/events/choose/${slot}`);