using System.Globalization;

namespace QWiz.Helpers.Exception;

public abstract class AppException : System.Exception
{
    protected AppException()
    {
    }

    protected AppException(string message) : base(message)
    {
    }

    protected AppException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}