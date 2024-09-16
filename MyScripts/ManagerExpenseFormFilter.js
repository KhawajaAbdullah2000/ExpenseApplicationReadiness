
$(document).ready(function () {
    updateRangeValue(selectedAmountRange); 

    if (sweetAlertMessage && sweetAlertType) {
        Swal.fire({
            icon: sweetAlertType,
            title: sweetAlertMessage,
            showConfirmButton: true
        });
    }
});

function updateRangeValue(value) {
    document.getElementById('rangeValue').innerText = value;
}
