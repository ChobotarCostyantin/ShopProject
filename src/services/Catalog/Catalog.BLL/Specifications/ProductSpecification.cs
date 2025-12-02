using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using Catalog.BLL.DTOs.Products.Requests;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.BLL.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification(GetProductsRequest request, bool ignorePagination = false)
        {
            if (!request.CategoryId.HasValue)
            {
                Query.Where(m => m.CategoryId == request.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                Query.Where(m => EF.Functions.ILike(m.Name, $"%{request.SearchTerm}%") || EF.Functions.ILike(m.Sku, $"%{request.SearchTerm}%"));
            }

            if (request.MinPrice.HasValue)
            {
                Query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                Query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            if (request.InStock.HasValue && request.InStock.Value)
            {
                Query.Where(p => p.StockQuantity > 0);
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "categoryid":
                        if (request.SortDescending) Query.OrderByDescending(p => p.CategoryId);
                        else Query.OrderBy(p => p.CategoryId);
                        break;

                    case "name":
                        if (request.SortDescending) Query.OrderByDescending(p => p.Name);
                        else Query.OrderBy(p => p.Name);
                        break;

                    case "price":
                        if (request.SortDescending) Query.OrderByDescending(p => p.Price);
                        else Query.OrderBy(p => p.Price);
                        break;

                    default:
                        Query.OrderBy(p => p.ProductId);
                        break;
                }
            }
            else
            {
                Query.OrderBy(p => p.ProductId);
            }

            if (!ignorePagination)
            {
                var skip = (request.PageNumber - 1) * request.PageSize;
                Query.Skip(skip).Take(request.PageSize);
            }

            Query.Include(p => p.Category)
                .Include(p => p.ProductDetails)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag);
        }
    }
}