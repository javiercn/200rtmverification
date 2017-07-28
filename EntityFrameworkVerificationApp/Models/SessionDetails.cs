using EntityFrameworkVerificationApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.Models
{
    public class SessionDetails
    {
        public Show Show { get; set; }
        public Session Session { get; set; }
        public Seat Seat { get; set; }
    }
}
