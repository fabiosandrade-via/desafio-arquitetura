using AutoMapper;
using System;

public class MappingStringDateTime : ITypeConverter<string?, DateTime?>
{
    public DateTime? Convert(string? source, DateTime? destination, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return null;
        }

        return DateTime.TryParse(source, out var result) ? result : null;
    }
}
