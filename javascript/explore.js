// explore.html: pick where to scavenge. "Stay Home" is a plain link.
// After scavenging, the background changes to the place you went and the result is shown.
const scene       = document.querySelector(".scene");
const heading     = document.querySelector("h1");
const spotButtons = ["gasStation", "office", "lake"].map(id => document.getElementById(id));
const resultBox   = document.getElementById("result");
const resultText  = document.getElementById("resultText");
const resultBack  = document.getElementById("resultBack");

const PLACE_NAMES = { gasStation: "the Gas Station", office: "the Office", lake: "the Lake" };

// Load the location pictures now, so the background switches instantly
["../assets/Gas-Station.png", "../assets/Office.webp", "../assets/Lake.webp"]
  .forEach(src => new Image().src = src);

async function scavenge(location) {
  spotButtons.forEach(b => b.disabled = true);   // no double clicks
  try {
    const r = await apiPost(`/bunker/scavenge/${location}`);

    // r.location is where you really ended up (without a map you can get lost)
    scene.classList.add("visiting", `at-${r.location}`);
    heading.textContent = `Scavenging at ${PLACE_NAMES[r.location]}`;
    resultText.textContent = r.result;
    resultBox.hidden = false;

    if (r.state.gameOver) {
      resultBack.textContent = "Continue";
      resultBack.onclick = e => { e.preventDefault(); gameOver(); };
    }
  } catch (err) {
    alert(err.message);
    goTo("bunker.html");
  }
}

spotButtons.forEach(btn => btn.onclick = () => scavenge(btn.id));

// Already been out today? Lock the buttons. No map? Warn the player.
async function init() {
  try {
    const s = await apiGet("/game/state");
    if (s.gameOver) return gameOver();
    if (s.scavengedToday) {
      heading.textContent = "I've already been out today. Time to rest.";
      spotButtons.forEach(b => b.disabled = true);
    } else if (!s.items.includes("Map")) {
      heading.textContent = "Without a map, who knows where I'll end up...";
    }
  } catch {
    heading.textContent = "Can't reach the server. Is the backend running?";
    spotButtons.forEach(b => b.disabled = true);
  }
}

init();