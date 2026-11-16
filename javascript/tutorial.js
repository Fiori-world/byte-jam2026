// tutorial.html: static page; keeps the credits list matching the real shop
async function syncCredits() {
  try {
    const shop = await apiGet("/game/shop");
    const items = document.querySelectorAll("ul li");
    Object.entries(shop.credits).forEach(([difficulty, credits], i) => {
      if (items[i]) items[i].textContent = `${difficulty} - ${credits} Credits`;
    });
  } catch {
    // backend not running: the HTML's own text stays
  }
}

syncCredits();