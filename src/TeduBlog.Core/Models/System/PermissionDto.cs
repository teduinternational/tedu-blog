using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Core.Models.System
{
    public class PermissionDto
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsDto> RoleClaims { get; set; }
    }
}
