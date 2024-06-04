$(document).ready(function () {
    let size = { rows: 2, cols: 2 };

    function createMatrix(container, size) {
        $(container).empty();
        for (let i = 0; i < size.rows; i++) {
            let row = $('<div class="row p-1"></div>');
            for (let j = 0; j < size.cols; j++) {
                row.append('<div class="col"><input type="number" class="form-control" /></div>');
            }
            $(container).append(row);
        }
    }

    function serializeMatrix(container, size) {
        let values = [];
        $(container).find('input').each(function () {
            values.push($(this).val() || 0);
        });
        return `${size.rows};${size.cols};${values.join(';')}`;
    }

    function deserializeMatrix(data, container) {
        let [rows, cols, ...values] = data.split(';');
        rows = parseInt(rows);
        cols = parseInt(cols);
        createMatrix(container, { rows, cols });
        let inputs = $(container).find('input');
        values.forEach((value, index) => {
            $(inputs[index]).val(value);
        });
    }

    function adjustMatrixSize(size, rowsIncrement, colsIncrement) {
        size.rows = Math.max(1, size.rows + rowsIncrement);
        size.cols = Math.max(1, size.cols + colsIncrement);
    }

    $('#increaseRows').click(function () {
        adjustMatrixSize(size, 1, 0);
        createMatrix('#matrix', size);
    });

    $('#decreaseRows').click(function () {
        adjustMatrixSize(size, -1, 0);
        createMatrix('#matrix', size);
    });

    $('#increaseCols').click(function () {
        adjustMatrixSize(size, 0, 1);
        createMatrix('#matrix', size);
    });

    $('#decreaseCols').click(function () {
        adjustMatrixSize(size, 0, -1);
        createMatrix('#matrix', size);
    });

    $('#submitMatrix').click(function () {
        let matrix = serializeMatrix('#matrix', size);

        $.ajax({
            url: 'https://localhost:7362/inverse',
            type: 'POST',
            data: { A: matrix },
            success: function (response) {
                deserializeMatrix(response, '#result');
            },
            error: function () {
                alert('Error in matrix invers.');
            }
        });
    });

    createMatrix('#matrix', size);
});
