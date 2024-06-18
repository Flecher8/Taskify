using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.Core.Dtos
{
    public class CreateCustomTaskDto
    {
        public string SectionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
