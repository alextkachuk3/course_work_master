using MathService.ApiService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MathService.ApiService.Controllers
{    
    public class MathController : Controller
    {
        private readonly ILogger<MathController> _logger;
        private readonly MatrixService _matrixService;
        private readonly RedisCacheService _cacheService;

        public MathController(ILogger<MathController> logger, MatrixService matrixService, RedisCacheService cacheService)
        {
            _logger = logger;
            _matrixService = matrixService;
            _cacheService = cacheService;
        }

        [HttpPost("multiply")]
        public async Task<IActionResult> Multiply(string A, string B)
        {
            string cacheKey = $"{A}-{B}";
            var cachedResult = await _cacheService.GetCachedResultAsync(cacheKey);

            if (cachedResult != null)
            {
                var cachedMatrix = JsonSerializer.Deserialize<int[,]>(cachedResult);
                return Ok(cachedMatrix);
            }

            try
            {
                var matrixA = _matrixService.ParseMatrix(A);
                var matrixB = _matrixService.ParseMatrix(B);
                var matrixAB = _matrixService.Multiply(matrixA, matrixB);
                var result = ConvertMatrixToString(matrixAB);
                await _cacheService.CacheResultAsync(cacheKey, result);
                return Ok(result);
            }
            catch (FormatException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        private string GenerateCacheKey(int[,] matrixA, int[,] matrixB)
        {
            return $"{ConvertMatrixToString(matrixA)}_{ConvertMatrixToString(matrixB)}";
        }

        private string ConvertMatrixToString(int[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var key = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    key += matrix[i, j] + "-";
                }
            }

            return key.TrimEnd('-');
        }
    }
}
