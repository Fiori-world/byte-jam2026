// bed.html: end the day. "Stay Awake" is a plain link back to the bunker.
const sleepButton = document.getElementById("Sleep");
let state = null;

sleepButton.onclick = async () => {
  // Can't sleep until today's journal entry is dealt with (the backend checks this too)
  if (state && !state.eventDoneToday) {
    if (confirm("You can't sleep yet. You need to deal with today's journal entry first.\n\nGo to the journal now?"))
      goTo("journal.html");
    return;
  }

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
