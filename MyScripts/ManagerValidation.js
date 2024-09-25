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

    $('#editForm').submit(function (event) {
        var remarks = $('#remarks').val().trim();

        if (remarks === '') {
            Swal.fire({
                icon: 'warning',
                title: 'No Remarks Found',
                text: 'Please enter your remarks.',
                confirmButtonText: 'OK'
            });
            event.preventDefault(); 
        }
    });


});