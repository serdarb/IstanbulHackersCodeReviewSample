namespace AgileWall.Domain.Repo {
    using System.Linq;

    using AgileWall.Domain.Entity;

    public static class RepoExtensions
    {
        /// <summary>
        /// An extention method for repositories. 
        /// We use this method for getting paginated data.
        /// We don't want repositories to get all the data.
        /// All the services GetAll methods uses this extention method and returns data with given size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize) where T : BaseEntity
        {
            return new PagedList<T>(pageIndex, pageSize, query.Skip((pageIndex - 1) * pageSize).Take(pageSize));
        }
    }
}