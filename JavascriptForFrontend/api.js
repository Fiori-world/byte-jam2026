// Shared by every page: all calls to the C# backend go through here
const API = "http://localhost:5000/api";
const PAGE = "/byte-jam/html" + name;

async function apiGet(path) {
  return handle(await fetch(API + path))
}

async function apiPost(path) {
  return handle(await fetch(API + path, {
    method: "POST",
    headers: body ? { "Content-Type": "application/json" } : {},
    body: body ? JSON.stringify(body) : undefined
  }));
}

// Turn the response into data, or into a readable error
async function handle(res) {
  const text = await res.text();
  let data;
  try { data = JSON.parse(text); } catch { data = text; }
  if (!res.ok) throw new Error(typeof data === "string" ? data : "Something went wrong.");
  return data;
}

function statsText(s){
  return `Day ${s.day} | Food ${s.day} | Water ${s.day} | Health ${s.day} | Health ${s.health}/20 | Morale ${s.morale}/50`;
}
