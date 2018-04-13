namespace Awards.ViewModels
{
    public class NominationViewModel
    {
        public int ID { get; set; }
        public int NomineeID { get; set; }
        public string Nominator { get; set; }
        public string Reason { get; set; }
        public bool Anonymous { get; set; }
    }
}