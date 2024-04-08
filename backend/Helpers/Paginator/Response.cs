namespace QWiz.Helpers.Paginator;

public class Response<T>
{
    protected Response()
    {
    }

    public Response(T data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }

    public T Data { get; set; } = default!;


    public bool Succeeded { get; set; }

    public string[]? Errors { get; set; }

    public string? Message { get; set; }
}