using MathService.ApiService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MathService.ApiService.Controllers;

public class MathController : Controller
{
    private readonly ILogger<MathController> _logger;
    private readonly MatrixService _matrixService;
    private readonly FibonacciService _fibonacciService;
    private readonly RedisCacheService _cacheService;

    public MathController(ILogger<MathController> logger, MatrixService matrixService, FibonacciService fibonacciService, RedisCacheService cacheService)
    {
        _logger = logger;
        _matrixService = matrixService;
        _fibonacciService = fibonacciService;
        _cacheService = cacheService;
    }

    [HttpPost("multiply")]
    public async Task<IActionResult> Multiply([FromForm] string A, [FromForm] string B)
    {
        string cacheKey = $"{A}-{B}";
        var cachedResult = await _cacheService.GetCachedResultAsync(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
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

    [HttpPost("inverse")]
    public async Task<IActionResult> Inverse([FromForm] string A)
    {
        string cacheKey = $"inverse-{A}";
        var cachedResult = await _cacheService.GetCachedResultAsync(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        try
        {
            var matrixA = _matrixService.ParseMatrix(A);
            var reversedMatrix = _matrixService.Inverse(matrixA);
            var result = ConvertMatrixToString(reversedMatrix);
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

    [HttpPost("fibonacci")]
    public async Task<IActionResult> FibonacciNumber(int n)
    {
        if(n <= 0)
        {
            return BadRequest(new { message = "n must be a positive value" });
        }

        string cacheKey = $"fibonacci-{n}";
        var cachedResult = await _cacheService.GetCachedResultAsync(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var result = _fibonacciService.GetFibonacciAsync(n).ToString();
        await _cacheService.CacheResultAsync(cacheKey, result);

        return Ok(result);
    }

    private string ConvertMatrixToString(double[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        StringBuilder sb = new();

        sb.Append(rows);
        sb.Append(';');
        sb.Append(cols);
        sb.Append(';');

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append(matrix[i, j]);
                sb.Append(';');
            }
        }

        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }
}
