// Shared by every page: all calls to the C# backend go through here
const API = "http://localhost:5000/api";

// Change the folder here once if the HTML files move
function goTo(page) {
  window.location.href = "/byte-jam2026/html/" + page;
}

async function apiGet(path) {
  return handle(await fetch(API + path));
}

async function apiPost(path, body) {
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

function statsText(s) {
  return `Day ${s.day} | Food ${s.food} | Water ${s.water} | Health ${s.health}/100 | Morale ${s.morale}/50`;
}

document.querySelectorAll("button > a").forEach(a => {
  a.parentElement.addEventListener("click", e => {
    if (e.target !== a) a.click();
  });
});

function say(text){
  const el = document.getElementById("message");
  if (el) el.textContent = text; else alert(text);
}

async function act(path, draw){
  try{
    const r = await apiPost(path);
    say(r.result);
    draw(r.state);
  } catch (err) {
    say(err.message)
  }
}

async function loadState(draw) {
  try{
    draw(await apiGet("/game/state"));
  
  } catch {
    say("Can't reach the server. Is the backend running?")
  }
}

function gameOver(){
  alert("You didn't survive...")
  window.location.href = "/byte-jam2026/index.html";
}