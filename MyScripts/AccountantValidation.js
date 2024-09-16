$(document).ready(function () {
    $('form').on('submit', function (event) {
        var statusDropdown = $('#ExpenseForm_Status');
        if (statusDropdown.val() === '') {
            event.preventDefault(); // Prevent form submission
            Swal.fire({
                icon: 'warning',
                title: 'No Value Selected',
                text: 'Please select a status.',
                confirmButtonText: 'OK'
            });
        }
    });
});