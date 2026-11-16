// journal.html: today's event, pick an option, confirm
const descriptionEl = document.querySelector(".description");
const optionButtons = document.querySelectorAll(".options button");
const optionDescs   = document.querySelectorAll(".right p");
const confirmButton = document.querySelector(".confirm");
let selectedSlot = null;

// Show or hide the option buttons, their descriptions and Confirm
function showOptions(choices) {
  optionButtons.forEach((btn, i) => {
    const choice = choices[i];
    btn.hidden = optionDescs[i].hidden = !choice;
    btn.disabled = false;
    btn.classList.remove("selected");
    if (choice) {
      btn.textContent = choice.text;
      btn.dataset.slot = choice.slot;
      optionDescs[i].textContent = choice.description;
    }
  });
  confirmButton.hidden = choices.length === 0;
}

// Load today's event from the backend
async function loadEvent() {
  selectedSlot = null;
  try {
    const ev = await apiGet("/journal/event");
    descriptionEl.textContent = ev.description;
    showOptions(ev.choices);
  } catch (err) {
    descriptionEl.textContent = err.message;   // e.g. "You've already dealt with today's event..."
    showOptions([]);
  }
}

// Select an option (highlight it)
optionButtons.forEach(btn => btn.onclick = () => {
  selectedSlot = btn.dataset.slot;
  optionButtons.forEach(b => b.classList.toggle("selected", b === btn));
});

// Confirm the selected option, then show what happened
confirmButton.onclick = async () => {
  if (!selectedSlot) return;
  try {
    const r = await apiPost(`/journal/choose/${selectedSlot}`);
    showOptions([]);                       // event is used up
    descriptionEl.textContent = r.result;
    if (r.state.gameOver) gameOver();
  } catch (err) {
    descriptionEl.textContent = err.message;
  }
};

// The Back to Bunker button is a plain link in the HTML

loadEvent();
