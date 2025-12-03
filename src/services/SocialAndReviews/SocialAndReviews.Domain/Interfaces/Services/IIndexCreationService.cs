using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Domain.Interfaces.Services
{
    public interface IIndexCreationService
    {
        Task CreateIndexesAsync();
    }
}