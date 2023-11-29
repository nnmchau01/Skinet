namespace Domain.Helpers;

public static class PagingHelper<T>
{
    public static PagingResult<T> ToPaging(List<T>? list
        , int pageNumber
        , int rowOfPage
        , string searchText
        , decimal minPrice
        , decimal maxPrice)
    {
        if (list?.Count > 0)
        {
            if (pageNumber <= 0 || rowOfPage <= 0)
            {
                pageNumber = 1;
                rowOfPage = 1;
            }

            int totalItems = list.Count;
            int skip = (pageNumber - 1) * rowOfPage;
            List<T> data = list.Count > 0
                ? list
                    .Skip(skip)
                    .Take(rowOfPage)
                    .ToList()
                : new List<T>();

            PagingResult<T> returnData = new PagingResult<T>
            {
                Data = data,
                TotalItems = list.Count,
                TotalPages = (int)Math.Ceiling((decimal)totalItems / rowOfPage),
                SearchText = searchText,
                PageNumber = pageNumber,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            };

            return returnData;
        }

        return new PagingResult<T>();
    }
}

public class PagingResult<T>
{
    public List<T> Data { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public string SearchText { get; set; }
    public int PageNumber { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
}