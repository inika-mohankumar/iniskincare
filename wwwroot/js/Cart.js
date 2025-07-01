document.addEventListener("DOMContentLoaded", function () {
    console.log("✅ Cart.js loaded");

    document.querySelectorAll(".add-to-cart-form").forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault(); // prevent full page reload

            const productId = this.dataset.productid;

            fetch("/Cart/AddToCart?productId=" + productId, {
                method: "POST"
            })
                .then(response => {
                    if (response.ok) {
                        console.log("✅ Item added");
                        
                        showCartMessage("Item added to cart!");
                    } else {
                        showCartMessage("Login to continue.");
                    }
                })
                .catch(() => {
                    showCartMessage("Error adding item.");
                });
        });
    });
});

// ✅ Move this OUTSIDE — now globally accessible
function showCartMessage(message) {
    const msg = document.getElementById("cart-message");
    if (msg) {
        msg.textContent = message;
        setTimeout(() => {
            msg.textContent = "";
            msg.style.display = "none";
        }, 3000);

        msg.style.display = "block";
        msg.style.color = "green";
    } else {
        console.warn("🛑 cart-message element not found");
    }
}





        
