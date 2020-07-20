using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class UserAccount
    {
        public ApplicationUser UserToShow { get; set; }
        public bool IsCurrentUser { get; set; }

    }
}
