using System;
using System.Reflection;
using System.Runtime.InteropServices;

/// @namespace UsefulClasses.Exceptions
/// @brief Exceptions used by the useful classes to represent problems.

/// @namespace UsefulClasses
/// @brief Basic namespace for all classes.

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("UsefulClasses")]
[assembly: AssemblyDescription("Useful classes to aid development.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Tony Richards")]
[assembly: AssemblyProduct("Useful Classes")]
[assembly: AssemblyCopyright("Copyright © Tony Richards 2011")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b10fd0ad-b7d8-4bb4-bd69-2b593f558d92")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("1.0.*")]

[assembly: CLSCompliant(true)]