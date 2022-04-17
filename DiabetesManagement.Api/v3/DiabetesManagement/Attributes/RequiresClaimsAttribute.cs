using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Attributes
{
    public class RequiresClaimsAttribute : Attribute
    {
        public RequiresClaimsAttribute(params string[] claims)
        {
            Claims = claims;
        }
        public IEnumerable<string> Claims { get; set; }
    }
}
