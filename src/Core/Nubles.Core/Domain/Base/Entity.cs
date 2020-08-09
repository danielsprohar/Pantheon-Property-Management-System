namespace Nubles.Core.Domain.Base
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Concurrency token for EF Core
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}