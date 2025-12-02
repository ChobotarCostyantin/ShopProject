using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using Catalog.BLL.DTOs.Categories.Requests;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.BLL.Specifications
{
    public class CategorySpecification : Specification<Category>
    {
        public CategorySpecification(GetCategoriesRequest request, bool ingorePagination = false)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                Query.Where(m => EF.Functions.ILike(m.Name, $"%{request.Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "name":
                        if(request.SortDescending) Query.OrderByDescending(m => m.Name);
                        else Query.OrderBy(m => m.Name);
                        break;

                    default:
                        if(request.SortDescending) Query.OrderByDescending(m => m.Name);
                        else Query.OrderBy(m => m.Name);
                        break;
                }
            }
            else
            {
                Query.OrderBy(m => m.CategoryId);
            }

            if (!ingorePagination)
            {
                var skip = (request.PageNumber - 1) * request.PageSize;
                Query.Skip(skip).Take(request.PageSize);
            }
        }
    }
}