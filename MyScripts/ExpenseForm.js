

var expenseIndex = $('#expensesTable tr').length;  // Initialize expenseIndex to the number of existing rows

// Add new expense row
$('#addExpense').click(function () {
    var newRow = `<tr>
                    <td><input type="text" name="Expenses[${expenseIndex}].Description" class="form-control" /></td>
                    <td><input type="date" name="Expenses[${expenseIndex}].ExpenseDate" class="form-control" /></td>
                    <td><input type="number" name="Expenses[${expenseIndex}].Amount" class="form-control amountInput" /></td>
                    <td><button type="button" class="btn btn-danger removeExpense">Remove</button></td>
                  </tr>`;
    $('#expensesTable').append(newRow);


    expenseIndex++
    calculateTotal();
});

$(document).on('click', '.removeExpense', function () {
    $(this).closest('tr').remove();
    reindexExpenses();
    calculateTotal();
});

function reindexExpenses() {
    // Reindex each row to ensure sequential indexes
    $('#expensesTable tr').each(function (index) {
        $(this).find('input').each(function () {
            let field = $(this).attr('name');
            if (field) {
                // Update the name attribute to reflect the current index
                var newName = field.replace(/Expenses\[\d+\]/, `Expenses[${index}]`);
                $(this).attr('name', newName);
            }
        });
    });

    // Update the expenseIndex to reflect the current number of rows
    expenseIndex = $('#expensesTable tr').length;
}

// Calculate total expense amount
function calculateTotal() {
    var total = 0;
    $('.amountInput').each(function () {
        var amount = parseFloat($(this).val()) || 0;
        total += amount;
    });
    $('#totalAmount').text(total.toFixed(2));  // Update total amount display
}

// Update total amount on input change
$(document).on('input', '.amountInput', function () {
    calculateTotal();
});

// Recalculate total on page load
$(document).ready(function () {
    calculateTotal();
});

// Validate form before submission to ensure at least one expense
$('#expenseForm').click(function (e) {
    var expensesCount = $('#expensesTable tr').length;
    if (expensesCount === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'No Expenses Added',
            text: 'At least one expense must be added.',
            confirmButtonText: 'OK'
        });
        e.preventDefault();  // Prevent form submission if no expenses
    }
});