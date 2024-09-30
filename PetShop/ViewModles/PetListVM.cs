namespace PetShop.ViewModles
{
    public class PetListVM
    {
        public List<PetVM> Pets { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}