//SetActive(document.getElementById("active"));

async function SetActive(element) {
    if (element == null)
        return;

    const delay = ms => new Promise(resolve => setTimeout(resolve, ms));
    await delay(100);

    element.classList.add("active");
}