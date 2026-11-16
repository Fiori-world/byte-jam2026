// Shared by every page: all calls to the C# backend go through here
// The backend serves the pages too, so the API is on the same site ("/api") — wherever it's hosted.
// Live Server (port 5500) is only for local testing; there the backend runs separately on port 5000.
const API = location.port === "5500"
  ? `${location.protocol}//${location.hostname}:5000/api`
  : "/api";

// Each device gets its own game: a random id, made once and kept in this browser,
// is sent with every request so the backend knows whose game it is.
const PLAYER_ID = (() => {
  const makeId = () => crypto.randomUUID
    ? crypto.randomUUID()
    : [...crypto.getRandomValues(new Uint8Array(16))].map(b => b.toString(16).padStart(2, "0")).join("");
  try {
    let id = localStorage.getItem("playerId");
    if (!id) {
      id = makeId();
      localStorage.setItem("playerId", id);
    }
    return id;
  } catch {
    return makeId();   // browser blocks storage: the game can't be remembered between pages
  }
})();

// Go to another page. Every page that calls this is in html/, so a relative name works
function goTo(page) {
  window.location.href = page;
}

async function apiGet(path) {
  return handle(await fetch(API + path, { headers: { "X-Player-Id": PLAYER_ID } }));
}

async function apiPost(path, body) {
  const headers = { "X-Player-Id": PLAYER_ID };
  if (body) headers["Content-Type"] = "application/json";
  return handle(await fetch(API + path, {
    method: "POST",
    headers,
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

// Buttons that wrap a link: clicking anywhere on the button follows the link,
// unless the page's own JS handles that button (like Play on the intro screen)
document.querySelectorAll("button > a").forEach(a => {
  const button = a.parentElement;
  button.addEventListener("click", e => {
    if (button.onclick) return;              // the page handles this button itself
    if (!e.target.closest("a")) a.click();   // only when the click missed the link itself
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
  window.location.href = "../index.html";
}