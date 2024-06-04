namespace MathService.ApiService.Services
{
    public class FibonacciService
    {
        public long GetFibonacciAsync(int n)
        {
            if (n <= 1) return n;
            long fibN = GetFibonacciAsync(n - 1) + GetFibonacciAsync(n - 2);
            return fibN;
        }
    }
}
