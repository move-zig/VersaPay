namespace VersaPayTest;

using Microsoft.Extensions.Options;

public class MockOptions<T> : IOptions<T> where T : class
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    required public T Value { get; set; }
}


