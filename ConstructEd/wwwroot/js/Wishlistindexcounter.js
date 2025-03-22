$(document).ready(function () {
    // Fetch wishlist count on page load
    updateWishlistCount();

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

    // Handle remove item functionality (FIXED)
    $(document).on("click", ".remove-item", function (e) {
        e.preventDefault();

        const button = $(this);
        const form = button.closest(".remove-item-form");
        const row = button.closest("tr");
        const token = form.find('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: "/WishList/RemoveFromCart",
            type: "POST",
            data: {
                id: button.data("id"),
                type: button.data("type"),
                __RequestVerificationToken: token
            },
            success: function (response) {
                if (response.success) {
                    row.fadeOut(300, function () {
                        $(this).remove();
                        updateWishlistCount(); // Update count after removal
                    });
                } else {
                    alert("Failed to remove item: " + (response.message || "Unknown error"));
                }
            },
            error: function () {
                alert("An error occurred while removing the item!");
            }
        });
    });

    function updateWishlistCount() {
        $.get("/WishList/GetWishlistCount", function (data) {
            $("#wishlist-count").text(data);
        }).fail(function () {
            console.error("Failed to fetch wishlist count.");
        });
    }
});