using Event_Management.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.API.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _transactionService;

        public PaymentTransactionController(IPaymentTransactionService transactionService)
        {
            _transactionService = transactionService;
        }


    }
}
