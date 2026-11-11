const optionButtons = document.querySelectorAll(".options button");
const confirmButton = document.getElementById("confirmButton");
const descriptionEl = document.getElementById("eventDescription");
const resultEl      = document.getElementById("result");
let selectedSlot = null;

// Show or hide the option buttons, their descriptions and Confirm
function showOptions(choices) {
  optionButtons.forEach((btn, i) => {
    const choice = choices[i];
    const desc = document.getElementById(`optionDesc${i + 1}`);
    btn.hidden = desc.hidden = !choice;
    btn.disabled = false;
    btn.classList.remove("selected");
    if (choice) {
      btn.textContent = choice.text;
      btn.dataset.slot = choice.slot;
      desc.textContent = choice.description;
    }
  });
  confirmButton.hidden = choices.length === 0;
}

// Load a random event from the backend
async function loadEvent() {
  selectedSlot = null;
  resultEl.textContent = "";
  try {
    const ev = await apiGet("/journal/event");
    descriptionEl.textContent = ev.description;
    showOptions(ev.choices);
  } catch (err) {
    descriptionEl.textContent = err.message;   // e.g. "No events available."
    showOptions([]);
  }
}

// Select an option (highlight it)
optionButtons.forEach(btn => btn.onclick = () => {
  selectedSlot = btn.dataset.slot;
  optionButtons.forEach(b => b.classList.toggle("selected", b === btn));
});

// Confirm the selected option
confirmButton.onclick = async () => {
  if (!selectedSlot) return;
  try {
    const r = await apiPost(`/journal/choose/${selectedSlot}`);
    resultEl.textContent = r.result;
    optionButtons.forEach(b => b.disabled = true);   // event is used up
    confirmButton.hidden = true;
    // TODO: show stats (r.state.day, r.state.food, r.state.water, r.state.health)
    // TODO: if r.state.gameOver → go to a game over page
    // TODO: decide how the next event loads (button, timer, or back to bunker)
  } catch (err) {
    resultEl.textContent = err.message;
  }
};

document.getElementById("backButton").onclick = () => {
  window.location.href = "events.html";   // TODO: change to your bunker page
};

loadEvent();
