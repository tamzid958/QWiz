namespace QWiz.Helpers.Extensions;

public static class FunctionExtension
{
    public static Func<TOut, TR> Convert<TIn, TOut, TR>(Func<TIn, TR> func) where TIn : TOut
    {
        return p => func((TIn)p!);
    }

    public static Func<TNewIn, TOut> Map<TOrigIn, TNewIn, TOut>(Func<TOrigIn, TOut> input,
        Func<TNewIn, TOrigIn> convert)
    {
        return x => input(convert(x));
    }
}