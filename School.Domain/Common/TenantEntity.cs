using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Domain.Common
{
    public abstract class TenantEntity : SoftDeleteEntity
    {
        public Guid SchoolId { get; set; }
    }
}
