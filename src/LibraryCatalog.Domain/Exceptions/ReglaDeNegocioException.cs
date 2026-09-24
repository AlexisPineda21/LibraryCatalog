namespace LibraryCatalog.Domain.Exceptions;

public sealed class ReglaDeNegocioException : Exception
{
    public ReglaDeNegocioException(string mensaje)
        : base(mensaje)
    {
    }
}
