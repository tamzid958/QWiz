using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QWiz.Databases;
using QWiz.Helpers.Extensions;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Abstract;

public abstract class BaseRepository<T>(
    AppDbContext context,
    IUriService uriService,
    IHttpContextAccessor httpContextAccessor
) : IBaseRepository<T>
    where T : class
{
    private IUriService UriService { get; } = uriService;
    private AppDbContext Context { get; } = context;

    T IBaseRepository<T>.GetById(object id)
    {
        try
        {
            var obj = Context.Set<T>().Find(id);
            if (obj == null) throw new KeyNotFoundException("not found");
            return obj;
        }
        catch (Exception e)
        {
            throw new Exception($"failed to fetch entity {typeof(T).Name}. Reason: {e}");
        }
    }

    T IBaseRepository<T>.Insert(T entity)
    {
        try
        {
            if (typeof(T).GetProperty("CreatedById") != null)
                typeof(T).GetProperty("CreatedById")?.SetValue(entity,
                    httpContextAccessor.HttpContext?.User.Claims.First(o => o.Type == "UserId").Value);
            if (typeof(T).GetProperty("CreatedAt") != null)
                typeof(T).GetProperty("CreatedAt")?.SetValue(entity, DateTimeOffset.Now);
            if (typeof(T).GetProperty("UpdatedAt") != null)
                typeof(T).GetProperty("UpdatedAt")?.SetValue(entity, DateTimeOffset.Now);


            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();

            return entity;
        }
        catch (Exception e)
        {
            throw new Exception($"failed save entity {typeof(T).Name}. Reason: {e}");
        }
    }

    public List<T> Insert(List<T> entities)
    {
        try
        {
            var user = httpContextAccessor.HttpContext?.User.Claims.First(o => o.Type == "UserId").Value;

            entities.ForEach(entity =>
            {
                if (typeof(T).GetProperty("CreatedById") != null)
                    typeof(T).GetProperty("CreatedById")?.SetValue(entity, user);
                if (typeof(T).GetProperty("CreatedAt") != null)
                    typeof(T).GetProperty("CreatedAt")?.SetValue(entity, DateTimeOffset.Now);
                if (typeof(T).GetProperty("UpdatedAt") != null)
                    typeof(T).GetProperty("UpdatedAt")?.SetValue(entity, DateTimeOffset.Now);
            });

            Context.Set<T>().AddRange(entities);
            Context.SaveChanges();
            return entities;
        }
        catch (Exception e)
        {
            throw new Exception($"failed save entity {typeof(T).Name}. Reason: {e}");
        }
    }

    T IBaseRepository<T>.Update(T entity)
    {
        try
        {
            if (typeof(T).GetProperty("UpdatedAt") != null)
                typeof(T).GetProperty("UpdatedAt")?.SetValue(entity, DateTimeOffset.Now);


            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            if (typeof(T).GetProperty("CreatedById") != null)
                Context.Entry(entity).Property("CreatedById").IsModified = false;
            if (typeof(T).GetProperty("CreatedAt") != null)
                Context.Entry(entity).Property("CreatedAt").IsModified = false;
            Context.SaveChanges();

            return entity;
        }
        catch (Exception e)
        {
            throw new Exception($"failed update entity {typeof(T).Name}. Reason: {e}");
        }
    }

    List<T> IBaseRepository<T>.Update(List<T> entities)
    {
        try
        {
            entities.ForEach(entity =>
            {
                if (typeof(T).GetProperty("UpdatedAt") != null)
                    typeof(T).GetProperty("UpdatedAt")?.SetValue(entity, DateTimeOffset.Now);
                if (typeof(T).GetProperty("CreatedById") != null)
                    Context.Entry(entity).Property("CreatedById").IsModified = false;
                if (typeof(T).GetProperty("CreatedAt") != null)
                    Context.Entry(entity).Property("CreatedAt").IsModified = false;
            });

            Context.Set<T>().UpdateRange(entities);
            Context.SaveChanges();
            return entities;
        }
        catch (Exception e)
        {
            throw new Exception($"failed update entity {typeof(T).Name}. Reason: {e}");
        }
    }

    int IBaseRepository<T>.Count(Expression<Func<T, bool>>? expression)
    {
        try
        {
            return expression == null
                ? Context.Set<T>().AsNoTracking().Count()
                : Context.Set<T>().Where(expression).AsNoTracking().Count();
        }
        catch (Exception e)
        {
            throw new Exception($"failed count entity {typeof(T).Name}. Reason: {e}");
        }
    }

    T IBaseRepository<T>.GetFirstBy(Expression<Func<T, bool>> expression,
        params Expression<Func<T, object>>[]? includes)
    {
        try
        {
            var obj = Context.Set<T>()
                .Where(expression)
                .AsNoTracking()
                .IncludeMultiple(includes)
                .FirstOrDefault();
            return obj ?? throw new KeyNotFoundException("not found");
        }
        catch (Exception e)
        {
            throw new Exception($"failed fetch entity {typeof(T).Name}. Reason: {e}");
        }
    }

    T IBaseRepository<T>.GetLastBy(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[]? includes)
    {
        try
        {
            var obj = Context.Set<T>()
                .Where(expression)
                .AsNoTracking()
                .IncludeMultiple(includes)
                .AsNoTracking()
                .LastOrDefault();
            return obj ?? throw new KeyNotFoundException("not found");
        }
        catch (Exception e)
        {
            throw new Exception($"failed fetch entity {typeof(T).Name}. Reason: {e}");
        }
    }

    List<T> IBaseRepository<T>.GetAll(
        Expression<Func<T, bool>>? expression,
        string attribute,
        Order order,
        params Expression<Func<T, object>>[]? includes)
    {
        try
        {
            return expression != null
                ? Context.Set<T>()
                    .Where(expression)
                    .OrderBy(attribute, order)
                    .AsNoTracking()
                    .IncludeMultiple(includes)
                    .ToList()
                : Context.Set<T>()
                    .OrderBy(attribute, order)
                    .AsNoTracking()
                    .IncludeMultiple(includes)
                    .ToList();
        }
        catch (Exception e)
        {
            throw new Exception($"failed fetch all entities {typeof(T).Name}. Reason: {e}");
        }
    }

    public List<T> GetAll(params Expression<Func<T, object>>[]? includes)
    {
        try
        {
            return Context.Set<T>().IncludeMultiple(includes).AsNoTracking().ToList();
        }
        catch (Exception e)
        {
            throw new Exception($"failed fetch al entities {typeof(T).Name}. Reason: {e}");
        }
    }


    PagedResponse<List<T>> IBaseRepository<T>.GetAll(PaginationFilter filter,
        HttpRequest request,
        Expression<Func<T, bool>>? expression,
        params Expression<Func<T, object>>[]? includes)
    {
        try
        {
            var (totalRecords, validFilter) = PaginatorWrapper(filter, expression);
            return (expression != null
                    ? Context.Set<T>()
                        .Where(expression)
                        .OrderBy(validFilter.Sort, validFilter.Order)
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                    : Context.Set<T>()
                        .OrderBy(validFilter.Sort, validFilter.Order)
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                )
                .AsNoTracking()
                .IncludeMultiple(includes)
                .ToList()
                .CreatePagedResponse(
                    validFilter,
                    totalRecords,
                    UriService,
                    request.Path.Value!
                );
        }
        catch (Exception e)
        {
            throw new Exception($"failed fetch all entities {typeof(T).Name}. Reason: {e}");
        }
    }

    T IBaseRepository<T>.MinBy(Func<T, object> selector, Expression<Func<T, bool>>? expression)
    {
        try
        {
            var obj = expression == null
                ? Context.Set<T>().AsNoTracking().MinBy(selector)
                : Context.Set<T>().Where(expression).AsNoTracking().MinBy(selector);
            return obj ?? throw new KeyNotFoundException("not found");
        }
        catch (Exception e)
        {
            throw new Exception($"failed min by entity {typeof(T).Name}. Reason: {e}");
        }
    }

    T IBaseRepository<T>.MaxBy(Func<T, object> selector, Expression<Func<T, bool>>? expression)
    {
        try
        {
            var obj = expression == null
                ? Context.Set<T>().AsNoTracking().MaxBy(selector)
                : Context.Set<T>().Where(expression).AsNoTracking().MaxBy(selector);
            return obj ?? throw new KeyNotFoundException("not found");
        }
        catch (Exception e)
        {
            throw new Exception($"failed max by entity {typeof(T).Name}. Reason: {e}");
        }
    }

    double IBaseRepository<T>.Sum(Func<T, double> selector, Expression<Func<T, bool>>? expression)
    {
        try
        {
            return expression == null
                ? Context.Set<T>().AsNoTracking().Sum(selector)
                : Context.Set<T>().Where(expression).AsNoTracking().Sum(selector);
        }
        catch (Exception e)
        {
            throw new Exception($"failed sum entity {typeof(T).Name}. Reason: {e}");
        }
    }

    List<TResult> IBaseRepository<T>.GroupBy<TResult, TKey>(
        Func<T, TKey> keySelector,
        Func<TKey, IEnumerable<T>, TResult> resultSelector,
        Expression<Func<T, bool>>? expression
    )
    {
        try
        {
            return expression != null
                ? Context.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                    .AsEnumerable()
                    .GroupBy(keySelector, resultSelector)
                    .ToList()
                : Context.Set<T>()
                    .AsNoTracking()
                    .AsEnumerable()
                    .GroupBy(keySelector, resultSelector)
                    .ToList();
        }
        catch (Exception e)
        {
            throw new Exception($"failed to group by entity {typeof(T).Name}. Reason: {e}");
        }
    }

    bool IBaseRepository<T>.Any(Expression<Func<T, bool>> expression)
    {
        try
        {
            return Context.Set<T>().Any(expression);
        }
        catch (Exception e)
        {
            throw new Exception($"failed to find any entity {typeof(T).Name}. Reason: {e}");
        }
    }

    void IBaseRepository<T>.Delete(IEnumerable<T> entities)
    {
        try
        {
            Context.Set<T>().RemoveRange(entities);
            Context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"failed delete entity {typeof(T).Name}. Reason: {e}");
        }
    }

    void IBaseRepository<T>.Delete(Expression<Func<T, bool>> expression)
    {
        try
        {
            var records = Context.Set<T>().Where(expression);
            Context.Set<T>().RemoveRange(records);
            Context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"failed delete entity {typeof(T).Name}. Reason: {e}");
        }
    }

    void IBaseRepository<T>.Delete(object id)
    {
        try
        {
            var obj = Context.Set<T>().Find(id);
            if (obj == null) throw new KeyNotFoundException("not found");
            Context.Set<T>().Remove(obj);
            Context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"failed delete entity {typeof(T).Name}. Reason: {e}");
        }
    }

    void IBaseRepository<T>.Delete(IEnumerable<object> ids)
    {
        try
        {
            var entities = ids.Select(id => Context.Set<T>().Find(id)!).ToList();
            Context.Set<T>().RemoveRange(entities);
            Context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"failed delete entity {typeof(T).Name}. Reason: {e}");
        }
    }

    private (int totalRecords, PaginationFilter validFilter) PaginatorWrapper(PaginationFilter filter,
        Expression<Func<T, bool>>? expression)
    {
        try
        {
            var totalRecords = expression != null
                ? Context.Set<T>().Where(expression).Count()
                : Context.Set<T>().Count();
            var validFilter = new PaginationFilter(filter.PageNumber,
                filter.PageSize, filter.Sort, filter.Order
            );


            return (totalRecords, validFilter);
        }
        catch (Exception e)
        {
            throw new Exception($"failed fetch all entities {typeof(T).Name}. Reason: {e}");
        }
    }
}