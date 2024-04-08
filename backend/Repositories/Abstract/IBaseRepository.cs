using System.Linq.Expressions;
using QWiz.Helpers.Paginator;

namespace QWiz.Repositories.Abstract;

public interface IBaseRepository<T>
{
    T Insert(T entity);

    List<T> Insert(List<T> entities);

    T Update(T entity);

    List<T> Update(List<T> entities);

    void Delete(object id, bool hard = false);
    void Delete(IEnumerable<object> ids, bool hard = false);
    void Delete(IEnumerable<T> entities, bool hard = false);

    T GetById(object id);

    int Count(Expression<Func<T, bool>>? expression = null);

    T MaxBy(Func<T, object> selector, Expression<Func<T, bool>>? expression = null);

    T MinBy(Func<T, object> selector, Expression<Func<T, bool>>? expression = null);

    double Sum(Func<T, double> selector, Expression<Func<T, bool>>? expression = null);

    bool Any(Expression<Func<T, bool>> expression);

    List<TResult> GroupBy<TResult, TKey>(
        Func<T, TKey> keySelector,
        Func<TKey, IEnumerable<T>, TResult> resultSelector,
        Expression<Func<T, bool>>? expression = null
    );

    PagedResponse<List<T>> GetAll(PaginationFilter filter,
        HttpRequest request,
        Expression<Func<T, bool>>? expression = null,
        params Expression<Func<T, object>>[]? includes);

    List<T> GetAll(
        Expression<Func<T, bool>>? expression = null,
        string attribute = "id",
        Order order = Order.Asc,
        params Expression<Func<T, object>>[]? includes
    );

    List<T> GetAll(params Expression<Func<T, object>>[]? includes);

    T GetLastBy(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[]? includes);

    T GetFirstBy(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[]? includes);
}