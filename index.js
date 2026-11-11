document.getElementById("playButton").onclick = async () => {
    await apiPost("/game/new");
    window.location.href = "events.html";
}