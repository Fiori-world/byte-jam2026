const creditsEl = document.getElementById("credits");
const difficultyButtons = document.querySelectorAll(".difficultyButtons button");
const [addFood, removeFood, addWater, removeWater] = document.querySelectorAll("#Rations button");
const [foodLabel, waterLabel] = document.querySelectorAll("#Rations p");
const itemButtons = document.querySelectorAll("#explore button, #safety button, #medical button")
const playButton = document.querySelector("#startGame button");

let shop = null;
let difficulty = null;
let food = 0, water = 0;
let items = new Set();

const credit = n => n === 1 ? "Credit" : "credits";
const budget = () => shop.credits[difficulty];
const spent = () => food * shop.rationPrice + water * shop.waterPrice + [...items].reduce((sum, i) => sum + shop.prices[i], 0);
const left = () => budget() - spent();

itemButtons.forEach(btn => btn.dataset.item = btn.textContent.replace("Take ", "").trim());

function render(){
    const ready = shop && difficulty;

    difficultyButtons.forEach(b => b.classList.toggle("selected", b.textContent.trim() === difficulty));
    creditsEl.textContent = ready ? `Credits - ${left()} / ${budget()}` : "Credits - choose a difficulty";
    foodLabel.textContent = `Rations - ${shop.rationPrice} ${credit(shop.rationPrice)} (have ${food})`;
    waterLabel.textContent = `Water - ${shop.waterPrice} ${credit(shop.waterPrice)} (have ${water})`;
    
    addFood.disabled = !ready || left() < shop.rationPrice;
    addWater.disabled = !ready || left() < shop.waterPrice;
    removeFood.disabled = food === 0;
    removeWater.disabled = water === 0;

    itemButtons.forEach(btn => {
        const name = btn.dataset.item, price = shop.prices[name];
        const taken = items.has(name);
        btn.previousElementSibling.textContent = `${name} - ${price} ${credit(price)}`;
        btn.textContent = taken ? `Drop ${name}` : `Take ${name}`;
        btn.classList.toggle("selected", taken);
        btn.disabled = !ready || (!taken && left() < price);
    });
    playButton.disabled = !ready;
}

difficultyButtons.forEach(btn => btn.onclick = () => {
    difficulty = btn.textContent.trim();
    food = water = 0;
    items.clear();
    render();
});

addFood.onclick = () => { food++; render(); };
removeFood.onclick = () => { food--; render(); };
addWater.onclick = () => { water++; render(); };
removeWater.onclick = () => { water--; render(); };

itemButtons.forEach(btn => btn.onclick = () => {
    const name = btn.dataset.item;
    items.has(name) ? items.delete(name) : items.add(name);
    render();
});

playButton.onclick = async () => {
    if ((food === 0 || water === 0) && !confirm("You're starting with no food or no water. Go anyways?")) return;
    try{
        await apiPost("/game/new", { difficulty, food, water, items: [...items] });
        goTo("bunker.html");

    }catch (err) {
        alert(err.message);
    }
};

async function init() {
    try {
        shop = await apiGet("/game/shop");
        render();
    } catch {
        creditsEl.textContent = "Can't reach the server. Is the backend running?";
        document.querySelectorAll("button").forEach(b => b.disabled = true);
    }
}

init();