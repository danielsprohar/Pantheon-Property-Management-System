using Microsoft.EntityFrameworkCore;
using System;

namespace Pantheon.Infrastructure.Helpers
{
    public class DbExceptionHelper
    {
        public static void HandleConcurrencyException(DbUpdateConcurrencyException ex, Type entityType)
        {
            // https://docs.microsoft.com/en-us/ef/core/saving/concurrency#resolving-concurrency-conflicts
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity.GetType() == entityType)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        proposedValues[property] = proposedValue;
                    }

                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }
        }
    }
}