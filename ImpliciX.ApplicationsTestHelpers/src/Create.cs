using System;
using System.Reflection;
using ImpliciX.ApplicationsTestHelpers.Internals;
using ImpliciX.Language.Driver;
using ImpliciX.Language.Modbus;

namespace ImpliciX.ApplicationsTestHelpers
{
  public class Create
  {
    public static IDriverStateKeeper DriverStateKeeper() => new FakeDriverStateKeeper();
    public static IModbusSimulation ModbusSimulation(Func<ModbusSlaveDefinition> slave, IModbusAdapter adapter = null)
      => new ModbusSimulation(slave(), adapter, Assembly.GetCallingAssembly());
    
    [Obsolete]
    public static IModbusSimulation ModbusSimulation(ModbusSlaveDefinition slave, IModbusAdapter adapter = null)
      => new ModbusSimulation(slave, adapter, Assembly.GetCallingAssembly());

    static Create()
    {
      RegistersMap.Factory = () => new RegistersMapImpl();
      CommandMap.Factory = () => new CommandMapImpl();
    }
  }
}