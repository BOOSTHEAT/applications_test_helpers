using System.Reflection;

[assembly:AssemblyCompanyAttribute("BOOSTHEAT")]
[assembly:AssemblyProductAttribute("ImpliciX.ApplicationsTestHelpers")]
[assembly:AssemblyDescription("Helpers to write unit tests for ImpliciX applications")]
[assembly:AssemblyTitleAttribute("ImpliciX ApplicationsTestHelpers "+ThisAssembly.Version)]
[assembly:AssemblyVersion(ThisAssembly.Version)]
[assembly:AssemblyInformationalVersionAttribute(ThisAssembly.InformationalVersion)]
[assembly:AssemblyFileVersionAttribute(ThisAssembly.Version)]

[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "CheckNamespace")]
partial class ThisAssembly
{
  public const string Version = Git.SemVer.Major + "." + Git.SemVer.Minor + "." + Git.SemVer.Patch;
  public const string InformationalVersion = Version + "-" + Git.Branch + "+" + Git.Commit;
}