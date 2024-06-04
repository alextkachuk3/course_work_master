$(document).ready(function () {
    $('#calculateFibonacci').click(function () {
        let index = $('#fibonacciIndex').val();

        $.ajax({
            url: 'https://localhost:7362/fibonacci',
            type: 'POST',
            data: { n: index },
            success: function (response) {
                $('#result').text(`Результат: ${response}`);
            },
            error: function () {
                alert('Error in calculating Fibonacci number.');
            }
        });
    });
});
