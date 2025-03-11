using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public class PaymentMethodRepository : BaseRepository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(JAwelsAndDiamondsEntities context) : base(context)
        {
        }
    }
}