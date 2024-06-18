using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CompanyRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public CompanyDto Company { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
