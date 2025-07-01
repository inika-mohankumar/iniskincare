function filterCategory(category) {
    document.querySelectorAll(".category").forEach(div => {
        div.style.display = "none"; 
        if (category === "all" || div.classList.contains(category)) {
            div.style.display = "block"; 
        }
    });
}




