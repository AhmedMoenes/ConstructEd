$(document).ready(function () {
    $(".wish-form").on("submit", function (event) {
        event.preventDefault();
        let form = $(this);
        let icon = form.find("i");
        let inWishlist = form.data("inwishlist") === true;

        let actionUrl = inWishlist ? "/WishList/RemoveFromCart" : "/WishList/AddToWish";

        $.post(actionUrl, form.serialize(), function (response) {
            if (response.success) {
                let newStatus = !inWishlist;
                form.data("inwishlist", newStatus);
                if (newStatus) {
                    icon.removeClass("far").addClass("fas");
                } else {
                    icon.removeClass("fas").addClass("far");
                }
            } else {
                alert(response.message || "Failed to update wishlist!");
            }
        }).fail(function () {
            alert("An error occurred!");
        });
    });

    $(".cart-form").on("submit", function (event) {
        event.preventDefault();
        let form = $(this);
        let icon = form.find("i");
        let inCart = form.data("incart") === true;

        let actionUrl = inCart ? "/ShoppingCart/RemoveFromCart" : "/ShoppingCart/AddToCart";

        $.post(actionUrl, form.serialize(), function (response) {
            if (response.success) {
                let newStatus = !inCart;
                form.data("incart", newStatus);
                if (newStatus) {
                    icon.removeClass("fas fa-cart-plus").addClass("fas fa-shopping-cart").css("color", "green");
                } else {
                    icon.removeClass("fas fa-shopping-cart").addClass("fas fa-cart-plus").css("color", "gray");
                }
            } else {
                alert(response.message || "Failed to update cart!");
            }
        }).fail(function () {
            alert("An error occurred!");
        });
    });
});