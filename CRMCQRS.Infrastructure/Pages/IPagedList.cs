namespace CRMCQRS.Infrastructure.Pages
{
    public interface IPagedList<T>
    {
        int IndexFrom { get; }

        int PageIndex { set; get; }

        int PageSize { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        IList<T> Items { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }
}