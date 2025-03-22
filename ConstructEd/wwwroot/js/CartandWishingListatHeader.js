// /Scripts/site.js

$(document).ready(function () {
    // Fetch wishlist count on page load
    updateWishlistCount();

    // Fetch cart count on page load
    updateCartCount();

    // Wishlist form submission handler
    $(".wish-form").on("submit", function (event) {
        event.preventDefault(); // Prevent form submission from reloading the page

        let icon = $(this).find("i");

        // Toggle UI instantly after form submission
        icon.toggleClass("far fas text-danger");

        // Add a small animation for visual feedback
        icon.css("transform", "scale(1.2)");
        setTimeout(() => icon.css("transform", "scale(1)"), 300);

        // Update wishlist count dynamically
        setTimeout(updateWishlistCount, 500); // Delay to ensure DB update
    });

    // Cart form submission handler
    $(".cart-form").on("submit", function (event) {
        event.preventDefault(); // Prevent form submission from reloading the page

        let icon = $(this).find("i");

        // Toggle UI instantly after form submission
        icon.toggleClass("fa-thin fa-solid text-success");

        // Add a small animation for visual feedback
        icon.css("transform", "scale(1.2)");
        setTimeout(() => icon.css("transform", "scale(1)"), 300);

        // Update cart count dynamically
        setTimeout(updateCartCount, 500); // Delay to ensure DB update
    });

    // Function to update wishlist count
    function updateWishlistCount() {
        $.get("/WishList/GetWishlistCount", function (data) {
            $("#wishlist-count").text(data);
        }).fail(function () {
            console.error("Failed to fetch wishlist count.");
            alert("Something went wrong while updating the wishlist count.");
        });
    }

    // Function to update cart count
    function updateCartCount() {
        $.get("/ShoppingCart/GetCartCount", function (data) {
            $("#cart-count").text(data);
        }).fail(function () {
            console.error("Failed to fetch cart count.");
            alert("Something went wrong while updating the cart count.");
        });
    }
});