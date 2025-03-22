$(document).ready(function () {
    function handleUnauthorized(xhr) {
        if (xhr.status === 401) {
            window.location.href = "/Account/Login"; // Redirect to login if unauthorized
        } else {
            alert("An error occurred!");
        }
    }

    $(".wish-form").on("submit", function (event) {
        event.preventDefault();

        let form = $(this);
        let icon = form.find("i");
        let inWishlist = form.data("inwishlist") === true; // Ensure boolean value

        let actionUrl = inWishlist
            ? "/WishList/RemoveFromCart"
            : "/WishList/AddToWish";

        $.post(actionUrl, form.serialize(), function (response) {
            if (response.success) {
                let newStatus = !inWishlist;
                form.data("inwishlist", newStatus);  // ✅ Update flag correctly

                // Toggle heart icon style
                if (newStatus) {
                    icon.removeClass("far").addClass("fas"); // Filled heart
                } else {
                    icon.removeClass("fas").addClass("far"); // Empty heart
                }
            } else {
                alert(response.message || "Failed to update wishlist!");
            }
        }).fail(handleUnauthorized);
    });

    $(".cart-form").on("submit", function (event) {
        event.preventDefault();

        let form = $(this);
        let icon = form.find("i");
        let inCart = form.data("incart") === true;

        let actionUrl = inCart
            ? "/ShoppingCart/RemoveFromCart"
            : "/ShoppingCart/AddToCart";

        $.post(actionUrl, form.serialize(), function (response) {
            if (response.success) {
                let newStatus = !inCart;
                form.data("incart", newStatus);

                // Toggle shopping cart icon and color
                if (newStatus) {
                    icon.removeClass("fa-cart-plus").addClass("fa-shopping-cart");
                    icon.css("color", "green");
                } else {
                    icon.removeClass("fa-shopping-cart").addClass("fa-cart-plus");
                    icon.css("color", "gray");
                }
            } else {
                alert(response.message || "Failed to update cart!");
            }
        }).fail(handleUnauthorized);
    });
});