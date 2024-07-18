using System.Collections.Generic;
using System.Linq;
using ImpliciX.Language.Core;
using ImpliciX.Language.Driver;
using ImpliciX.Language.Model;

namespace ImpliciX.ApplicationsTestHelpers.Internals
{
  internal class FakeDriverState : IDriverState
  {
    private FakeDriverState(Urn id = null)
    {
      Id = id ?? Urn.BuildUrn("");
    }

    private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
    public Urn Id { get; }

    public Result<T> GetValue<T>(string key) => _values.TryGetValue(key, out object value)
      ? Result<T>.Create((T)value)
      : Result<T>.Create(new Error("", ""));

    public Result<T> GetValueOrDefault<T>(string key, T defaultValue) => _values.TryGetValue(key, out object value)
      ? Result<T>.Create((T)value)
      : Result<T>.Create(defaultValue);

    public IDriverState New(Urn id) => new FakeDriverState(id);

    public IDriverState WithValue(string key, object value)
    {
      _values[key] = value;
      return this;
    }

    public bool Contains(string key) => _values.ContainsKey(key);

    public static FakeDriverState Empty(Urn urn) => new FakeDriverState(urn);
    public bool IsEmpty => !_values.Any();
  }
}