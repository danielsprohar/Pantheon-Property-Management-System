namespace Nubles.Core.Domain.Models
{
    public class CustomerDriverLicense
    {
        public enum DBColumnLength
        {
            Number = 32,
            State = 32
        }

        public string Number { get; set; }

        public string State { get; set; }

        public string PhotoUrl { get; set; }
    }
}