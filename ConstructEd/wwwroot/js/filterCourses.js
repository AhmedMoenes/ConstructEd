$(document).ready(function () {
    $("#categoryFilter").change(function () {
        var selectedCategory = $(this).val();

        // Convert empty string to null for "All Categories"
        if (selectedCategory === "") {
            selectedCategory = null;
        }

        $.ajax({
            url: "/Course/GetCoursesByCategory",
            type: "GET",
            data: { category: selectedCategory },
            success: function (response) {
                if (response.success) {
                    $(".courses-section .row").empty(); // Clear current courses
                    $.each(response.courses, function (index, course) {
                        $(".courses-section .row").append(`
<div class="col-lg-4 col-md-6 mb-4">
<div class="course-card">
<div class="card-body">
<img src="/Image/${course.image}" class="card-img-top" alt="${course.title}" />
<h5 class="card-title">${course.title}</h5>
<p class="card-text">Duration: ${course.duration}</p>
<p class="card-text">Price: ${course.price}</p>
<a href="/Course/Details/${course.id}" class="btn btn-primary btn-block">View Details</a>
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