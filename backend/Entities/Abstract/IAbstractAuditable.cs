namespace QWiz.Entities.Abstract;

public interface IAbstractAuditable<TKey, TUser, TUserKey> : IAbstractPersistence<TKey>
{
    public TUserKey? CreatedById { set; get; }

    public TUser? CreatedBy { set; get; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}