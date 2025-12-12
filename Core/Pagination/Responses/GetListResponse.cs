namespace Core.Pagination.Responses
{
    public class GetListResponse<T> : BasePageableModel
    {
        public IList<T> Items
        {
            get => _items ??= new List<T>();
            set => _items = value;
        }

        private IList<T>? _items;
    }
}
