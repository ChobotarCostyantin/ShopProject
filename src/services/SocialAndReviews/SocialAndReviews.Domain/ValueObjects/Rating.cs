using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Exceptions;

namespace SocialAndReviews.Domain.ValueObjects
{
    public class Rating : ValueObject
    {
        public int Value { get; }

        // Конструктор приватний або захищений, створення через фабричний метод або публічний конструктор з валідацією
        public Rating(int value)
        {
            if (value < 1 || value > 5)
            {
                throw new DomainException("Rating must be between 1 and 5.");
            }
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator int(Rating rating) => rating.Value;
    }
}