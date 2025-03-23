$(document).ready(function () {
    function handleUnauthorized(xhr) {
        if (xhr.status === 401) {
            window.location.href = "/Account/Login"; // Redirect if unauthorized
        } else {
            alert("An error occurred!");
        }
    }

    // Handle Buy Now Button
    $(document).on("click", "#buyNowButton", function () {
        let courseId = $(this).data("id"); // Get course ID from button attribute

        if (!courseId) {
            alert("Course ID is missing!");
            return;
        }

        $.ajax({
            url: "/ShoppingCart/AddToCart",
            type: "POST",
            data: { id: courseId, type: "Course" },
            success: function (response) {
                console.log("Success:", response);

                if ($("#paymentModal").length) {
                    $("#paymentModal").modal("show");
                } else {
                    alert("Payment modal not found!");
                }
            },
            error: function (xhr) {
                handleUnauthorized(xhr);
            }
        });
    });

    // Handle Wishlist Submission
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

    // Handle Cart Submission
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

                // Toggle shopping cart icon and color
                if (newStatus) {
                    icon.removeClass("fas fa-cart-plus").addClass("fas fa-shopping-cart");
                    icon.css("color", "green");
                } else {
                    icon.removeClass("fas fa-shopping-cart").addClass("fas fa-cart-plus");
                    icon.css("color", "gray");
                }
            } else {
                alert(response.message || "Failed to update cart!");
            }
        }).fail(handleUnauthorized);
    });
});