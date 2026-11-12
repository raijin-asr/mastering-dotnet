namespace ProductManagementApi.Exceptions;

public class DuplicateProductNameException : Exception
{
    public DuplicateProductNameException()
        : base("A product with this name already exists.")
    {
    }
}