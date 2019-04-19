using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。 
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください
[assembly: AssemblyTitle("XIE.Tasks")]
#if LINUX
	#if DEBUG
	[assembly: AssemblyDescription("XIE.Tasks for Linux (Debug)")]
	[assembly: AssemblyConfiguration("Debug")]
	#else
	[assembly: AssemblyDescription("XIE.Tasks for Linux")]
	[assembly: AssemblyConfiguration("Release")]
	#endif
#else
	#if DEBUG
	[assembly: AssemblyDescription("XIE.Tasks for Windows (Debug)")]
	[assembly: AssemblyConfiguration("Debug")]
	#else
	[assembly: AssemblyDescription("XIE.Tasks for Windows")]
	[assembly: AssemblyConfiguration("Release")]
	#endif
#endif
[assembly: AssemblyCompany("Eggs Imaging Laboratory")]
[assembly: AssemblyProduct("XIE")]
[assembly: AssemblyCopyright("Copyright (c) 2013 Eggs Imaging Laboratory")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
//[assembly: CLSCompliant(false)]

// ComVisible を false に設定すると、このアセンブリ内の型は COM コンポーネントには 
// 参照不可能になります。COM からこのアセンブリ内の型にアクセスする場合は、 
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]

// 次の GUID は、このプロジェクトが COM に公開される場合の、typelib の ID です
[assembly: Guid("3de2ca58-c123-4d47-9eae-0a80af9e91a1")]

// アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// すべての値を指定するか、下のように '*' を使ってリビジョンおよびビルド番号を 
// 既定値にすることができます:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.6")]

