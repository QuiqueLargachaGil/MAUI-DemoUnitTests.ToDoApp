namespace LongoToDoApp.Infrastructure.Mappers.Base
{
	public abstract class BaseMapper<TSource, TDestination> : IMapper<TSource, TDestination> where TSource : class where TDestination : class
	{
		public TDestination Mapper(TSource source)
		{
			if (source is null)
			{
				return null;
			}

			return MapperImplementation(source);
		}

		protected abstract TDestination MapperImplementation(TSource source);
	}
}
