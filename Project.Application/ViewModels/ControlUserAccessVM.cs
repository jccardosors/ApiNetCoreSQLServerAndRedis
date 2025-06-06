using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.ViewModels
{
    public class ControlUserAccessVM
    {
        public int Id { get; set; }
        public DateTime LastAccess { get; set; }
        public int TryNumber { get; set; }
        public bool Blocked { get; set; }
        public string UserEmail { get; set; }
    }
}
