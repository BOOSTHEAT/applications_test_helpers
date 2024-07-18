using ImpliciX.Language.Driver;
using ImpliciX.Language.Modbus;
using ImpliciX.Language.Model;

namespace ImpliciX.ApplicationsTestHelpers
{
  public interface IModbusSimulation
  {
    public void ExecuteCommand(Urn commandUrn, object cmdValue);
    public IClock Clock { get; }
    public IDriverStateKeeper State { get; }
    public IModbusAdapter Adapter { get; }
    public ushort[] Registers(ushort startAddress, ushort length);
    IDataModelValue[] ReadProperties(MapKind mapKind);
  }
}