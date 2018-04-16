using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.ViewModels
{
    public class NewNominationViewModel
    {
        public int? NominationId { get; set; }
        public int CategoryID { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
    }
}