using System.Collections.Concurrent;

namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 可枚举
/// </summary>
public abstract class Enumeration<TSelf> : IValueObject
    where TSelf : notnull, Enumeration<TSelf>
{
    /// <summary>
    /// 这个类型下的所有枚举缓存
    /// </summary>
    private static readonly ConcurrentDictionary<string, TSelf> _cache = [with(StringComparer.OrdinalIgnoreCase)];

    // 获取或者创建
    protected static TSelf GetOrAdd(string name, Func<string, TSelf> factory)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        return _cache.GetOrAdd(name, factory);
    }

    // 获取所有
    protected static IReadOnlyList<TSelf> GetAll()
    {
        return [.. _cache.Values];
    }

    public static bool operator ==(Enumeration<TSelf>? left, Enumeration<TSelf>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Enumeration<TSelf>? left, Enumeration<TSelf>? right)
        => !(left == right);


    /// <summary>
    /// 枚举名称
    /// </summary>
    public string Name { get; }

    protected Enumeration(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if(obj is not TSelf other) return false;
        return StringComparer.OrdinalIgnoreCase.Equals(Name, other.Name);
    }
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }
    public override string ToString()
    {
        return Name;
    }
}

/// <summary>
/// 枚举
/// </summary>
public abstract class Enumeration<TSelf, TValue> : IValueObject
    where TSelf : Enumeration<TSelf, TValue>
    where TValue : IEquatable<TValue>
{
    /// <summary>
    /// 这个类型下的所有枚举缓存
    /// </summary>
    private static readonly ConcurrentDictionary<string, TSelf> _nameCacheDictionary = [with(StringComparer.OrdinalIgnoreCase)];
    private static readonly ConcurrentDictionary<TValue, TSelf> _valueCacheDictionary = [];

    // 获取或者创建
    protected static TSelf GetOrAdd(string name, TValue value, Func<string, TValue, TSelf> factory)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        // 先检查值缓存
        if (_valueCacheDictionary.TryGetValue(value, out var byValue))
        {
            // 值已存在，检查名称是否匹配
            if (!_nameCacheDictionary.TryGetValue(name, out var byName))
            {
                // 值存在但名称不存在：添加别名
                _nameCacheDictionary.TryAdd(name, byValue);
            }
            return byValue;
        }

        // 检查名称缓存（可能值不同）
        if (_nameCacheDictionary.TryGetValue(name, out var byNameOnly))
        {
            // 名称已存在但值不同：冲突
            if (!byNameOnly.Value.Equals(value))
            {
                throw new InvalidOperationException(
                    $"枚举名称 '{name}' 已存在且值为 '{byNameOnly.Value}'，不能赋新值 '{value}'");
            }
            return byNameOnly;
        }

        // 创建新实例
        var created = factory(name, value);
        _nameCacheDictionary.TryAdd(name, created);
        _valueCacheDictionary.TryAdd(value, created);
        return created;
    }

    // 根据名称获取
    protected static bool TryGetByName(string name, out TSelf? result)
    {
        return _nameCacheDictionary.TryGetValue(name, out result);
    }
    // 根据值获取
    protected static bool TryGetByValue(TValue value, out TSelf? result)
    {
        return _valueCacheDictionary.TryGetValue(value, out result);
    }

    // 获取所有
    protected static IReadOnlyList<TSelf> GetAll()
    {
        return [.. _nameCacheDictionary.Values];
    }

    public static bool operator ==(Enumeration<TSelf, TValue>? left, Enumeration<TSelf, TValue>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Enumeration<TSelf, TValue>? left, Enumeration<TSelf, TValue>? right)
        => !(left == right);


    /// <summary>
    /// 枚举名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 值
    /// </summary>
    public TValue Value { get; }


    protected Enumeration(string name, TValue value)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        Name = name;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TSelf other) return false;
        return Value.Equals(other);
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public override string ToString()
    {
        return Name;
    }
}