const statusesEl = document.querySelector(".statuses");
const [medicineButton, brushButton] = document.querySelectorAll(".selfCare button");

function feelings(s) {
    if (s.health < 30) f.push("Badly hurt"); else if (s.health < 70) f.push("Sore");
    if (s.food === 0) f.push("Starving"); else if (s.food <= 2) f.push("Hungry");
    if (s.water === 0) f.push("Parched"); else if (s.water <= 2) f.push("Thirsty");
    if (s.morale < 10) f.push("Hopeless"); else if (s.morale < 20) f.push("Bored");
    return f.length ? f : ["Okay, for now"];
}

function draw(s) {
    statusesEl.querySelectorAll("p").forEach(p => p.remove());
    feelings(s).forEach(text => {
        const p = document.createElement("p");
        p.textContent = text;
        statusesEl.append(p);
    });

    medicineButton.disabled = !s.items.includes("Medicine");
    brushButton.disabled = s.brushedToday;
}

medicineButton.onClick = () => act("game/use/Medicine", draw);
brushButton.onClick = () => act("/mirror/brush", draw);

loadState(draw);