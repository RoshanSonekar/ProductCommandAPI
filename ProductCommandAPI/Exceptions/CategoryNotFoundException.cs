namespace ProductCommandAPI.Exceptions
{
	public class CategoryNotFoundException : NotFoundException
	{
		public CategoryNotFoundException(Guid Id) : base("Category", Id)
		{
		}
	}
}
