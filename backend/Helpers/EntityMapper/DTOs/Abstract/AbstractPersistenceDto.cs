using Newtonsoft.Json;

namespace QWiz.Helpers.EntityMapper.DTOs.Abstract;

public class AbstractPersistenceDto<TKey>
{
    [JsonIgnore] public TKey? Id { get; set; }
}