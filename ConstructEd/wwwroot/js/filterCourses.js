$(document).ready(function () {
    $("#categoryFilter").change(function () {
        var selectedCategory = $(this).val();

        if (selectedCategory === "") {
            selectedCategory = null;
        }

        $.ajax({
            url: "/Course/GetCoursesByCategory",
            type: "GET",
            data: { category: selectedCategory },
            success: function (response) {
                if (response.success) {
                    $(".courses-section .row").empty();
                    $.each(response.courses, function (index, course) {
                        var enrolledButton = course.isEnrolled
                            ? '<button class="btn btn-success w-100" disabled>Enrolled</button>'
                            : `<form class="cart-form" data-courseid="${course.id}" data-incart="${course.isInCart}">
                                    <input type="hidden" name="id" value="${course.id}" />
                                    <input type="hidden" name="type" value="Course" />
                                    <button type="submit" class="btn btn-icon">
                                        <i class="${course.isInCart ? 'fas fa-shopping-cart' : 'fas fa-cart-plus'}"></i>
                                    </button>
                                </form>
                                <form class="wish-form" data-courseid="${course.id}" data-inwishlist="${course.isInWishlist}">
                                    <input type="hidden" name="id" value="${course.id}" />
                                    <input type="hidden" name="type" value="Course" />
                                    <button type="submit" class="btn btn-icon">
                                        <i class="${course.isInWishlist ? 'fas' : 'far'} fa-heart"></i>
                                    </button>
                                </form>`;

                        $(".courses-section .row").append(`
                            <div class="col-lg-4 col-md-6 mb-4">
                                <div class="card course-card">
                                    <div class="card-body">
                                        ${course.image ? `<img src="/Image/${course.image}" class="card-img-top" alt="${course.title}" />` : ''}
                                        <h5 class="card-title">${course.title}</h5>
                                        <p class="card-text courses-card-text">Category: ${course.category.replace(/_/g, ' ')}</p>
                                        <p class="card-text courses-card-text">Duration: ${course.duration} hrs</p>
                                        <a href="/Course/Details/${course.id}" class="btn btn-main btn-block">View Details</a>
                                        <div class="d-flex justify-content-between mt-3">
                                            ${enrolledButton}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `);
                    });
                }
            }
        });
    });
});