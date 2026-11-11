document.getElementById("playButton").onclick = async () => {
  try {
    await apiPost("/game/new");             // fresh game on the backend
    window.location.href = "journal.html";
  } catch (err) {
    alert("Can't reach the game server: " + err.message);
  }
};

// TODO: make these pages
document.getElementById("tutButton").onclick  = () => window.location.href = "tutorial.html";
document.getElementById("credButton").onclick = () => window.location.href = "credits.html";
