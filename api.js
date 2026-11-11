// Shared by every page: all calls to the C# backend go through here
const API = "http://localhost:5000/api";

async function apiGet(path) {
  const res = await fetch(API + path);
  return handle(res);
}

async function apiPost(path) {
  const res = await fetch(API + path, { method: "POST" });
  return handle(res);
}

// Turn the response into data, or into a readable error
async function handle(res) {
  const text = await res.text();
  let data;
  try { data = JSON.parse(text); } catch { data = text; }
  if (!res.ok) throw new Error(typeof data === "string" ? data : "Something went wrong.");
  return data;
}
