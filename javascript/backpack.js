// backpack.html: rebuilds the Supplies list from the real game state
const list = document.querySelector(".right");

// What each item does (keep in sync with Scavenge/UseItem in logic.cs)
const ITEM_INFO = {
  "Radio":         "Warns you of danger outside",
  "Map":           "Never come back empty-handed",
  "Gas Mask":      "Halves damage outside",
  "Crowbar":       "+1 food & water when scavenging",
  "First Aid Kit": "+40 health",
  "Medicine":      "+20 health, +2 morale"
};
const USABLE = ["First Aid Kit", "Medicine"];

function heading(text) {
  const h = document.createElement("h3");
  h.textContent = text;
  return h;
}

function row(name, value, cls, onUse) {
  const div = document.createElement("div");
  div.className = "item";
  const label = document.createElement("span");
  label.textContent = name;
  const info = document.createElement("span");
  info.textContent = value;
  if (cls) info.className = cls;
  div.append(label, info);
  if (onUse) {
    const b = document.createElement("button");
    b.textContent = "Use";
    b.onclick = onUse;
    div.append(b);
  }
  return div;
}

function draw(s) {
  list.querySelectorAll("h3, .item").forEach(el => el.remove());   // keep the "Supplies" h2

  list.append(
    heading("Food & Water"),
    row("Rations", `${s.food} left`,  s.food  <= 2 ? "warning" : "good"),
    row("Water",   `${s.water} left`, s.water <= 2 ? "warning" : "good"),
    heading("Equipment")
  );

  if (s.items.length === 0) list.append(row("Nothing packed", ""));
  s.items.forEach(item => list.append(row(
    item,
    ITEM_INFO[item] ?? "",
    "good",
    USABLE.includes(item) ? () => act(`/game/use/${encodeURIComponent(item)}`, draw) : null
  )));

  if (s.gameOver) gameOver();
}

loadState(draw);