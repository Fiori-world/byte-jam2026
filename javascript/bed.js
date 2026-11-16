// bed.html: end the day. "Stay Awake" is a plain link back to the bunker.
const sleepButton = document.getElementById("Sleep");
let state = null;

sleepButton.onclick = async () => {
  if (state && !state.eventDoneToday &&
      !confirm("You haven't checked the journal today. Sleep anyway?")) return;

  sleepButton.disabled = true;   // no double sleeping
  try {
    const r = await apiPost("/bunker/sleep");
    alert(r.result);             // popup, since we leave the page next
    if (r.state.gameOver) return gameOver();
  } catch (err) {
    alert(err.message);
  }
  goTo("bunker.html");
};

loadState(s => {
  state = s;
  if (s.gameOver) gameOver();
});
