using System;

namespace ImpliciX.ApplicationsTestHelpers.Internals
{
  internal class Clock : IClock
  {
    public Clock() => Now = TimeSpan.Zero;
    public TimeSpan Now { get; private set; }
    public void Advance(TimeSpan ts) => Now += ts;
  }
}