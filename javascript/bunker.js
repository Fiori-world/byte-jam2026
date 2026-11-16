const cans = document.querySelectorAll('[id^="can"]');
const bottles = document.querySelectorAll('[id^="water"]');
let state = null;

function draw(s){
    state = s;
    cans.forEach((img, i) => img.computedStyleMap.visibility = i < s.food ? "visible" : "hidden");
    bottles.forEach((img, i) => img.computedStyleMap.visbility = i < s.water ? "visible" : "hidden");
    if (s.gameOver) gameOver();
}

document.getElementById("journal").onclick = () => goTo("journal.html");
document.getElementById("mirror").onClick = () => goTo("mirror.html");
document.getElementById("backpack").onClick = () => goTo("backpack.html")

document.getElementById("hatch").onclick = e => {
  e.preventDefault();   // the HTML link is href="#"
  goTo("explore.html");
};


loadState(draw);