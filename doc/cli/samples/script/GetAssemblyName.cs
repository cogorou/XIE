using System;
using System.Reflection;
using System.IO;

namespace GetAssemblyName
{
	/// <summary>
	/// https://msdn.microsoft.com/ja-jp/library/office/ee539422(v=office.14).aspx
	/// </summary>
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1 || args[0] == "?")
			{
				PrintUsage();
				return;
			}
			
			string filename = args[0];

			try
			{
				AssemblyName an = AssemblyName.GetAssemblyName(filename);
				Console.WriteLine("Fully specified assembly name:\n");
				Console.WriteLine(an.ToString());
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Cannot locate the assembly. Check the path and try again.");
			}

			Console.Read();
		}

		static void PrintUsage()
		{
			Console.WriteLine("Usage: GetAssemblyName.exe <path and filename>\n");
			Console.WriteLine(@"Example: GetAssemblyName.exe C:\MyAssembly.dll");
			Console.Read();
		}
	}
}
