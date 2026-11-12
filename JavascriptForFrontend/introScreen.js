const CREDITS = { Easy: 20, Medium: 15, Hard: 10};
const PRICES = { Rations: 1, Water: 1, Radio: 5, Map: 3, "Gas Mask": 2, Crowbar: 3, "First Aid Kit": 4, Medicine: 2}
const ITEMS = ["Radio", "Map", "Gas Mask", "Crowbar", "First Aid Kit", "Medicine"];

let difficulty = null, budget = 0, food = 0, water = 0;
const items = new Set();

const norm = t => t.replace(/\s/g, "").toLowercase();
const shopButtons = [...document.querySelectorAll(".creditSelect button")];
const shopLabels = 