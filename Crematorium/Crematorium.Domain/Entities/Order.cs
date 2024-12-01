namespace Crematorium.Domain.Entities
{
    public class Order : Base
    {
        public User Customer { get; set; } = null!;
        //public Date? RegistrationDate { get; set; }

        public StateOrder State { get; set; } = StateOrder.Decorated;

        //public Date DateOfStart { get; set; } = null!;

        public DateTime DateOfStart { get; set; }

        public Corpose CorposeId { get; set; } = null!;

        public RitualUrn? RitualUrnId { get; set; }

        public Hall? HallId { get; set; }
    }

    public enum StateOrder
    {
        Decorated = 1,  //оформленный
        Approved = 2,   //подтвержденный
        Closed = 3,     //закрытый
        Cancelled = 4   //отмененный
    }
}
