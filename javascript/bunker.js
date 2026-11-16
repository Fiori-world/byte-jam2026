// bunker.html: cans/bottles show supplies.
// Journal, hatch (explore.html), mirror, hammock (bed.html) and backpack are plain links in the HTML.
const cans    = document.querySelectorAll('[id^="can"]');
const bottles = document.querySelectorAll('[id^="water"]');

function draw(s) {
  // visibility (not display) so the other cans don't shift position
  cans.forEach((img, i)    => img.style.visibility = i < s.food  ? "visible" : "hidden");
  bottles.forEach((img, i) => img.style.visibility = i < s.water ? "visible" : "hidden");
  if (s.gameOver) gameOver();
}

loadState(draw);
