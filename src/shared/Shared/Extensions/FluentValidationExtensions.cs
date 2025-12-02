using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Shared.Extensions
{
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Перевіряє унікальність значення в базі даних (case-insensitive для PostgreSQL).
        /// </summary>
        /// <typeparam name="TModel">Тип DTO/Request</typeparam>
        /// <typeparam name="TEntity">Тип сутності в БД</typeparam>
        /// <param name="ruleBuilder">Будівельник правил валідації</param>
        /// <param name="table">DbSet або IQueryable таблиці (наприклад, _context.Categories)</param>
        /// <param name="propertyExpression">Вираз, що вказує на властивість сутності (наприклад, x => x.Name)</param>
        public static IRuleBuilderOptions<TModel, string> MustBeUniqueAsync<TModel, TEntity>(
            this IRuleBuilder<TModel, string> ruleBuilder,
            IQueryable<TEntity> table,
            Expression<Func<TEntity, string>> propertyExpression)
            where TEntity : class
        {
            return ruleBuilder.MustAsync(async (value, cancellationToken) =>
            {
                if (string.IsNullOrEmpty(value)) return true; // Пусті значення пропускаємо (для них є NotEmpty)

                // Динамічно будуємо лямбду: entity => EF.Functions.ILike(entity.Property, value)

                // 1. Параметр лямбди (entity)
                var entityParam = propertyExpression.Parameters[0];

                // 2. Доступ до властивості (entity.Property)
                var propertyAccess = propertyExpression.Body;

                // 3. Значення для пошуку (value)
                var valueConstant = Expression.Constant(value, typeof(string));

                // 4. Отримуємо метод EF.Functions.ILike
                var ilikeMethod = typeof(NpgsqlDbFunctionsExtensions)
                    .GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike),
                        new[] { typeof(DbFunctions), typeof(string), typeof(string) });

                if (ilikeMethod == null)
                    throw new InvalidOperationException("Npgsql ILike method not found.");

                // 5. Виклик: EF.Functions.ILike(...)
                var efFunctions = Expression.Constant(EF.Functions);
                var ilikeCall = Expression.Call(ilikeMethod, efFunctions, propertyAccess, valueConstant);

                // 6. Фінальна лямбда: entity => ...
                var lambda = Expression.Lambda<Func<TEntity, bool>>(ilikeCall, entityParam);

                // Виконуємо запит: !AnyAsync(...)
                var exists = await table.AnyAsync(lambda, cancellationToken);
                return !exists;
            }).WithMessage("'{PropertyName}' must be unique.");
        }
    }
}