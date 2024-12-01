namespace Crematorium.Domain.Entities
{
    public class RitualUrn : Entity
    {
        //public byte[] Image { get; set; } = new byte[0];

        public string Image { get; set; } = "";
        public decimal Price { get; set; }
    }
}
