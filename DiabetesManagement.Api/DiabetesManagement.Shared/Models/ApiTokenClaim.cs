using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Models
{
    public class ApiTokenClaim : ApiToken
    {
        public Guid ApiTokenClaimId { get; set; }
        public string? Claim { get; set; }
        public DateTimeOffset ApiTokenClaimCreated { get; set; }
    }
}
