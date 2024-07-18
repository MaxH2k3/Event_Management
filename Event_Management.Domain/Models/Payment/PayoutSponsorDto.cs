using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Payment
{
    public class PayoutSponsorDto
    {
        public Guid TransactionId { get; set; }
        public string EmailSubject { get; set; }    

    }
}
