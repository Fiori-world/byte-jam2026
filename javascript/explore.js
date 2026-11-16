// explore.html: pick where to scavenge. "Stay Home" is a plain link.
const heading = document.querySelector("h1");
const spotButtons = ["gasStation", "office", "lake"].map(id => document.getElementById(id));

async function scavenge(location) {
  spotButtons.forEach(b => b.disabled = true);   // no double clicks
  try {
    const r = await apiPost(`/bunker/scavenge/${location}`);
    alert(r.result);                             // popup, since we leave the page next
    if (r.state.gameOver) return gameOver();
    goTo("bunker.html");
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