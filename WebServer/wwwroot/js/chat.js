Enable();

async function Enable() {
    element = document.getElementById("main-container");
    if (element == null)
        return;

    const delay = ms => new Promise(resolve => setTimeout(resolve, ms));
    await delay(100);

    element.classList.add("main-container-enabled");
}