using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Api.Dtos;
using System;

namespace SimpleEcommerce.Api.Extensions
{
    public static class QuerableExtensions
    {
        public static async Task<PagedDto<T>> ToPaged<T>(this IQueryable<T> query, int skip, int lenght, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.Negative(skip, nameof(skip));
            Guard.Against.NegativeOrZero(lenght, nameof(lenght));

            var count = await query.CountAsync();

            var items = await query
                .Skip(skip)
                .Take(lenght)
                .ToListAsync(cancellationToken);

            return new PagedDto<T>(items, count, skip, lenght);

        }
    }
}
