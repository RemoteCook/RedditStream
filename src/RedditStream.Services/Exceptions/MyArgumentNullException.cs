using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RedditStream.Services.Exceptions;

internal class MyArgumentNullException : ArgumentNullException
{
    public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) =>
      throw new ArgumentNullException(paramName);

}
