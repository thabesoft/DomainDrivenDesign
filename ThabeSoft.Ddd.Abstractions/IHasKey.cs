namespace ThabeSoft.Ddd;

/// <summary>
/// 定义具有唯一主键标识的对象
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IHasKey<out TKey> where TKey : notnull
{
    TKey Id { get; }
}