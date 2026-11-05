namespace ProductCommandAPI.Exceptions
{
	public class CategoryNotFoundException : NotFoundException
	{
		public CategoryNotFoundException(int Id) : base("Category", Id)
		{
		}
	}
}
