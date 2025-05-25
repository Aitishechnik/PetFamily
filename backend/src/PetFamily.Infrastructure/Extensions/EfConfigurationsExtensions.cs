using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Extensions
{
    public static class EfConfigurationsExtensions
    {
        public static PropertyBuilder<IReadOnlyList<TVO>> ConvertVOCollectionToJSON<TVO>(this PropertyBuilder<IReadOnlyList<TVO>> builder)
        {
            return builder.HasConversion<string>(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<IReadOnlyList<TVO>>(v, JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<TVO>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()));
        }

        public static PropertyBuilder<TVO> ConvertVOToJSON<TVO>(this PropertyBuilder<TVO> buid)
        {
            return buid.HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<TVO>(v, JsonSerializerOptions.Default)!);
        }
    }
}
