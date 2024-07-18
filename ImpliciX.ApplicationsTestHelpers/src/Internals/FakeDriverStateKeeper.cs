using System.Collections.Generic;
using System.Linq;
using ImpliciX.Language.Core;
using ImpliciX.Language.Driver;
using ImpliciX.Language.Model;

namespace ImpliciX.ApplicationsTestHelpers.Internals
{
  internal class FakeDriverStateKeeper : IDriverStateKeeper
  {
    public FakeDriverStateKeeper()
    {
      Log = ImpliciX.Language.Core.Log.Logger;
    }
    private readonly Dictionary<string, IDriverState> _data = new Dictionary<string, IDriverState>();

    public Result<IDriverState> TryRead(Urn urn) => Result<IDriverState>.Create(Read(urn));

    public IDriverState Read(Urn urn)
    {
      if (_data.ContainsKey(urn))
      {
        return _data[urn];
      }

      var state =
        _data.Values
          .Where(s => s.Id.IsPartOf(urn))
          .OrderByDescending(it => it.Id.Value)
          .FirstOrDefault();
      if (state != null)
        return state;
      return FakeDriverState.Empty(urn);
    }

    public IDriverState Update(IDriverState state) => _data[state.Id] = state;

    public ILog Log { get; }

    public Result<Unit> TryUpdate(IDriverState state)
    {
      if (state is { IsEmpty: false }) Update(state);
      return default(Unit);
    }
  }
}