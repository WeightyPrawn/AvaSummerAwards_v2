using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.ViewModels
{
    public class NomineeViewModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public List<NominationViewModel> Nominations { get; set; }
        public VoteViewModel Vote { get; set; }
    }
}