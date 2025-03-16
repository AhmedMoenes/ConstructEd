$(document).ready(function () {
    $('#paymentModal').on('show.bs.modal', function () {
        let totalPrice = $('#totalPrice').text().trim();
        $('#paymentAmount').val(totalPrice);
    });

    $('#CardNumber').on('input', function () {
        let value = $(this).val().replace(/\D/g, '');
        value = value.replace(/(\d{4})/g, '$1 ').trim();
        $(this).val(value);
    });

    $('#ExpiryDate').on('input', function () {
        let value = $(this).val().replace(/\D/g, '');
        if (value.length >= 2) value = value.replace(/(\d{2})(\d{0,2})/, '$1/$2');
        $(this).val(value);
    });

    $('#paymentForm').on('submit', function (event) {
        let expiry = $('#ExpiryDate').val();
        let currentYear = new Date().getFullYear() % 100;
        let currentMonth = new Date().getMonth() + 1;

        if (expiry.length === 5) {
            let [month, year] = expiry.split('/').map(Number);
            if (year < currentYear || (year === currentYear && month < currentMonth)) {
                alert("Expiry date must be in the future.");
                event.preventDefault();
            }
        } else {
            alert("Invalid expiry date format.");
            event.preventDefault();
        }
    });
});
