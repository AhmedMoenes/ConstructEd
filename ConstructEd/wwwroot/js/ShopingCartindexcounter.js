$(document).ready(function () {
    // Fetch cart count on page load
    updateCartCount();

    $(".cart-form").on("submit", function (event) {
        event.preventDefault();

        let icon = $(this).find("i");

        // Toggle UI instantly after form submission
        icon.toggleClass("fa-thin fa-solid text-success");

        // Add animation for visual feedback
        icon.css("transform", "scale(1.2)");
        setTimeout(() => icon.css("transform", "scale(1)"), 300);

        // Update cart count dynamically
        setTimeout(updateCartCount, 500); // Delay to ensure DB update
    });

    // Handle remove item functionality
    $('.btn-remove').on('click', function (e) {
        e.preventDefault();

        const button = $(this);
        const form = button.closest('.remove-item-form');
        const row = button.closest('tr');

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    row.fadeOut(300, function () {
                        $(this).remove();
                        updateCartState();
                        updateCartCount(); // Call function to update cart count after item removal
                    });
                } else {
                    alert('Failed to remove item: ' + (response.message || 'Unknown error'));
                }
            },
            error: function () {
                alert('An error occurred while removing the item!');
            }
        });
    });

    function updateCartCount() {
        $.get("/ShoppingCart/GetCartCount", function (data) {
            $("#cart-count").text(data);
        }).fail(function () {
            console.error("Failed to fetch cart count.");
        });
    }

    function updateCartState() {
        const remainingItems = $(".cart-item").length;

        if (remainingItems === 0) {
            $("#cartTable, #cartSummary").hide();
            $("#emptyCartMessage").show();
        } else {
            let newTotal = 0;
            $(".cart-item").each(function () {
                const priceText = $(this).find(".price").text().replace("$", "");
                newTotal += parseFloat(priceText) || 0;
            });

            $("#totalPrice").text(newTotal.toFixed(2));
        }
    }
});