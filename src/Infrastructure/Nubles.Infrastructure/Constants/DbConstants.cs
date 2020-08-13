namespace Nubles.Infrastructure.Constants
{
    public class DbConstants
    {
        /// <summary>
        /// UTC date for Microsoft SQL server.
        /// </summary>
        public const string UtcDate = "GETUTCDATE()";

        /// <summary>
        /// Specifies that the DbColumnType should <em>only</em> contain the date component.
        /// </summary>
        public const string DateDbType = "DATE";
    }
}