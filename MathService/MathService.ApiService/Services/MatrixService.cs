using System;

namespace MathService.ApiService.Services
{
    public class MatrixService
    {
        public int[,] Multiply(int[,] matrixA, int[,] matrixB)
        {
            int aRows = matrixA.GetLength(0);
            int aCols = matrixA.GetLength(1);
            int bRows = matrixB.GetLength(0);
            int bCols = matrixB.GetLength(1);

            if (aCols != bRows)
            {
                throw new InvalidOperationException("Invalid matrices dimensions for multiplication.");
            }

            int[,] result = new int[aRows, bCols];

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

        public int[,] ParseMatrix(string matrixString)
        {
            var parts = matrixString.Split('-');
            int height = int.Parse(parts[0]);
            int width = int.Parse(parts[1]);
            var matrix = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = int.Parse(parts[2 + i * width + j]);
                }
            }

            return matrix;
        }
    }
}
