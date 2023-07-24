namespace AlumniProject.Dto;

public class PagingResultDTO<T>
{
    public List<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }

    public bool HasNextPage
    {
        get { return (CurrentPage * PageSize) < TotalItems; }
    }
    public bool HasPreviousPage
    {
        get { return CurrentPage > 1; }
    }
}
