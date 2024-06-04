using System;

namespace MathService.ApiService.Services
{
    public class MatrixService
    {
        public double[,] Multiply(double[,] matrixA, double[,] matrixB)
        {
            int aRows = matrixA.GetLength(0);
            int aCols = matrixA.GetLength(1);
            int bRows = matrixB.GetLength(0);
            int bCols = matrixB.GetLength(1);

            if (aCols != bRows)
            {
                throw new InvalidOperationException("Invalid matrices dimensions for multiplication.");
            }

            double[,] result = new double[aRows, bCols];

            for (int i = 0; i < aRows; i++)
            {
                for (int j = 0; j < bCols; j++)
                {
                    for (int k = 0; k < aCols; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return result;
        }

        public double[,] Reverse(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] result = new double[n, n];
            double[,] augmented = new double[n, 2 * n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                }
                augmented[i, n + i] = 1;
            }

            for (int i = 0; i < n; i++)
            {
                double diag = augmented[i, i];
                if (diag == 0)
                    throw new InvalidOperationException("Matrix is not invertible.");

                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] /= diag;
                }

                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = augmented[k, i];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            augmented[k, j] -= factor * augmented[i, j];
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmented[i, n + j];
                }
            }

            return result;
        }

        public double[,] ParseMatrix(string matrixString)
        {
            var parts = matrixString.Split(';');
            int height = int.Parse(parts[0]);
            int width = int.Parse(parts[1]);
            var matrix = new double[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = double.Parse(parts[2 + i * width + j]);
                }
            }

            return matrix;
        }
    }
}
