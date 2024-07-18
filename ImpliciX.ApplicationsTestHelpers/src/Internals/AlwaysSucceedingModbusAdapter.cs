using System.Collections.Generic;
using System.Linq;
using ImpliciX.Language.Modbus;

namespace ImpliciX.ApplicationsTestHelpers.Internals
{
  public class AlwaysSucceedingModbusAdapter : IModbusAdapter
  {
    public ushort[] ReadRegisters(string _, RegisterKind kind, ushort startAddress,
      ushort registersToRead) =>
      Enumerable.Range(startAddress, registersToRead).Select(i => _registers[(ushort)i]).ToArray();

    public void WriteRegisters(string _, ushort startAddress, ushort[] registersToWrite)
    {
      for (ushort i = 0; i < registersToWrite.Length; i++)
        _registers[(ushort)(startAddress + i)] = registersToWrite[i];
    }

    private readonly Dictionary<ushort, ushort> _registers = new Dictionary<ushort, ushort>();
  }
}