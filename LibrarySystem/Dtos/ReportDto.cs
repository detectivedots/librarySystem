namespace LibrarySystem.Dtos
{
    public class ReportDto
    {
        public int TotalBorrowed { get; set; }   
        public int TotalReturned { get; set; } 
        public int TotalBooks {  get; set; }
        public BookDto MostBorrowed { get; set; }
        public CategoryDto MostPopularCategory {  get; set; }
    }
}
