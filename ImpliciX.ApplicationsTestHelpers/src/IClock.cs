using System;

namespace ImpliciX.ApplicationsTestHelpers
{
  public interface IClock
  {
    public TimeSpan Now { get; }
    void Advance(TimeSpan ts);
  }
}