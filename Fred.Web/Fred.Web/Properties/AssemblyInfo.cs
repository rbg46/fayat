using System.Reflection;
using System.Runtime.InteropServices;
using Fred.Web.Bootstrapper.DependencyInjection;
using WebActivatorEx;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Fred.Web")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Fayat Construction Informatique")]
[assembly: AssemblyProduct("Fred.Web")]
[assembly: AssemblyCopyright("Copyright 2016 © Fayat Construction Informatique")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2630fa96-7f2f-4fe7-880d-b5338cad4c47")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: PreApplicationStartMethod(typeof(DependencyInjectionWebApiActivator), nameof(DependencyInjectionWebApiActivator.Start))]
[assembly: PreApplicationStartMethod(typeof(DependencyInjectionMvcActivator), nameof(DependencyInjectionMvcActivator.Start))]

[assembly: ApplicationShutdownMethod(typeof(DependencyInjectionWebApiActivator), nameof(DependencyInjectionWebApiActivator.Shutdown))]
[assembly: ApplicationShutdownMethod(typeof(DependencyInjectionMvcActivator), nameof(DependencyInjectionMvcActivator.Shutdown))]