using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.ViewModels
{
    public class NewNominationViewModel
    {
        public int CategoryID { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
        //public bool Anonymous { get; set; }
    }
}