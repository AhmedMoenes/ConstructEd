$(document).ready(function () {
    function loadCourses(category) {
        $.ajax({
            url: '/Course/GetCoursesByCategory',
            type: 'GET',
            data: { category: category },
            success: function (data) {
                let coursesHtml = '';
                if (data.length === 0) {
                    coursesHtml = '<p class="text-center">No courses found for this category.</p>';
                } else {
                    data.forEach(course => {
                        coursesHtml += `
                            <div class="col-lg-4 col-md-6 mb-4">
                                <div class="card course-card">
                                    <div class="card-body">
                                        ${course.image ? `<img src="/Image/${course.image}" class="card-img-top" alt="${course.title}" />` : ''}
                                        <h5 class="card-title">${course.title}</h5>
                                        <p class="card-text">Category: ${course.category.replace("_", " ")}</p>
                                        <p class="card-text">Duration: ${course.duration} hrs</p>
                                        <a href="/Course/Details/${course.id}" class="btn btn-main btn-block">View Details</a>
                                    </div>
                                </div>
                            </div>`;
                    });
                }
                $('#coursesContainer').html(coursesHtml);
            },
            error: function () {
                $('#coursesContainer').html('<p class="text-center text-danger">Error loading courses.</p>');
            }
        });
    }

    // Load all courses initially
    loadCourses(-1);

    // Event listener for category change
    $('#categoryFilter').change(function () {
        let selectedCategory = $(this).val();
        loadCourses(selectedCategory);
    });
});
