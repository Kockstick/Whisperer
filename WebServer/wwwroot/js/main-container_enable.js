Enable();

async function Enable() {
    element = document.querySelector(".main-container");
    if (element == null)
        return;
    
    const delay = ms => new Promise(resolve => setTimeout(resolve, ms));
    await delay(100);

    element.classList.add("container-enabled");
}