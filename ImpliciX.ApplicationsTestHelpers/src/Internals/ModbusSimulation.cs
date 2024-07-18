using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImpliciX.Data;
using ImpliciX.Data.Factory;
using ImpliciX.Language.Driver;
using ImpliciX.Language.Modbus;
using ImpliciX.Language.Model;

namespace ImpliciX.ApplicationsTestHelpers.Internals
{
  internal class ModbusSimulation : IModbusSimulation
  {

    public ModbusSimulation(ModbusSlaveDefinition modbusSlaveDefinition, IModbusAdapter adapter, Assembly modelAssembly)
    {
      _definition = modbusSlaveDefinition;
      Clock = new Clock();
      State = new FakeDriverStateKeeper();
      _modelFactory = new ModelFactory(modelAssembly);
      _adapter = adapter ?? new AlwaysSucceedingModbusAdapter();
    }

    public void ExecuteCommand(Urn commandUrn, object cmdValue)
    {
      var argValue = (commandUrn is CommandUrn<NoArg> && cmdValue is null) ? default(NoArg) : cmdValue;
      var arg = ((IModelCommand)_modelFactory.Create(commandUrn, argValue).Value).Arg;
      var command = _definition.CommandMap.ModbusCommandFactory(commandUrn)
        .Invoke(arg, Clock.Now, State.Read(commandUrn)).Value;
      _adapter.WriteRegisters(_settings.Factory, command.StartAddress, command.DataToWrite);
      State.TryUpdate(command.State);
    }

    public IClock Clock { get; }
    public IDriverStateKeeper State { get; }
    public IModbusAdapter Adapter => _adapter;

    public ushort[] Registers(ushort startAddress, ushort length) =>
      _adapter.ReadRegisters(string.Empty, RegisterKind.Holding, startAddress, length);

    public IDataModelValue[] ReadProperties(MapKind mapKind)
    {
      var mapDefinition = _definition.ReadPropertiesMaps.GetValueOrDefault(mapKind, RegistersMap.Empty());
      var segmentsDefinitions = mapDefinition.SegmentsDefinition;

      var registerSegments = new RegistersSegment[segmentsDefinitions.Length];

      for (var index = 0; index < segmentsDefinitions.Length; index++)
      {
        var segDef = segmentsDefinitions[index];
        var readRegisters = _adapter.ReadRegisters(_settings.Factory, segDef.Kind, segDef.StartAddress,
          segDef.RegistersToRead);
        registerSegments[index] = new RegistersSegment(segDef, readRegisters);
      }

      var modbusRegisters = ModbusRegisters.Create(registerSegments);
      var decodedValues = mapDefinition.Eval(modbusRegisters, Clock.Now, State)
        .GetValueOrDefault(Array.Empty<IDataModelValue>()).ToArray();
      return decodedValues;
    }

    private readonly ModbusSlaveDefinition _definition;
    private readonly ModelFactory _modelFactory;
    private readonly IModbusAdapter _adapter;

    private static ModbusSlaveSettings _settings = new ModbusSlaveSettings{Id = 200, TimeoutSettings = new TimeoutSettings(), ReadPaceInSystemTicks = 1};
  }
}