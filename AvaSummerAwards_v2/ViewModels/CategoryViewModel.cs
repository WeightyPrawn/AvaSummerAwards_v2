using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awards.ViewModels
{
    public class CategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<NomineeViewModel> Nominees { get; set; }
        public bool UserHasVoted { get; set; }
    }
}