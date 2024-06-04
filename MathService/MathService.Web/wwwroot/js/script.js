$(document).ready(function () {
    let sizeA = { rows: 2, cols: 2 };
    let sizeB = { rows: 2, cols: 2 };

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
        return `${size.rows}-${size.cols}-${values.join('-')}`;
    }

    function deserializeMatrix(data, container) {
        let [rows, cols, ...values] = data.split('-');
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

    $('#increaseARows').click(function () {
        adjustMatrixSize(sizeA, 1, 0);
        createMatrix('#matrixA', sizeA);
    });

    $('#decreaseARows').click(function () {
        adjustMatrixSize(sizeA, -1, 0);
        createMatrix('#matrixA', sizeA);
    });

    $('#increaseACols').click(function () {
        adjustMatrixSize(sizeA, 0, 1);
        createMatrix('#matrixA', sizeA);
    });

    $('#decreaseACols').click(function () {
        adjustMatrixSize(sizeA, 0, -1);
        createMatrix('#matrixA', sizeA);
    });

    $('#increaseBRows').click(function () {
        adjustMatrixSize(sizeB, 1, 0);
        createMatrix('#matrixB', sizeB);
    });

    $('#decreaseBRows').click(function () {
        adjustMatrixSize(sizeB, -1, 0);
        createMatrix('#matrixB', sizeB);
    });

    $('#increaseBCols').click(function () {
        adjustMatrixSize(sizeB, 0, 1);
        createMatrix('#matrixB', sizeB);
    });

    $('#decreaseBCols').click(function () {
        adjustMatrixSize(sizeB, 0, -1);
        createMatrix('#matrixB', sizeB);
    });

    $('#submitMatrices').click(function () {
        let matrixA = serializeMatrix('#matrixA', sizeA);
        let matrixB = serializeMatrix('#matrixB', sizeB);

        $.ajax({
            url: 'https://localhost:7362/multiply',
            type: 'POST',
            data: { A: matrixA, B: matrixB },
            success: function (response) {
                deserializeMatrix(response, '#result');
            },
            error: function () {
                alert('Error in matrix multiplication.');
            }
        });
    });
    
    createMatrix('#matrixA', sizeA);
    createMatrix('#matrixB', sizeB);
});
