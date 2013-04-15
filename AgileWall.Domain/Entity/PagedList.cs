namespace AgileWall.Domain.Entity {
    using System;
    using System.Collections.Generic;

    public class PagedList<T> where T : BaseEntity
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        public PagedList(int pageIndex, int pageSize, IEnumerable<T> source)
        {
            this.Items = new List<T>();
            this.Items.AddRange(source);

            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public long TotalCount
        {
            get
            {
                return this.Items.Count;
            }
        }

        public int TotalPageCount
        {
            get
            {
                if (this.PageSize == int.MinValue)
                {
                    this.PageSize = 1;
                }
                return (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);
            }
        }

        public List<T> Items { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return (this.PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (this.PageIndex < this.TotalPageCount);
            }
        }
    }
}