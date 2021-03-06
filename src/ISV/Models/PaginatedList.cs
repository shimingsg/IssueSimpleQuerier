﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISV.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize, params string[] path)
        {
            
            var count = await source.CountAsync();
            var items = source.Skip(
                (pageIndex - 1) * pageSize)
                .Take(pageSize);
            
            foreach (string s in path)
            {
                //items = items.Include(s);
            }

            var items_i = await items.ToListAsync();

            return new PaginatedList<T>(items_i, count, pageIndex, pageSize);
        }
    }

}
