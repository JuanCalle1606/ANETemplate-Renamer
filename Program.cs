using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Collections.Generic;

namespace ANETemplate_Renamer
{
	class Program
	{
		/// <summary>
		///		The path where the project is located
		/// </summary>
		public static string Path = "../native_library/win/";

		/// <summary>
		///		Here all the renaming happens
		/// </summary>
		/// <param name="newName">The current name of the project</param>
		/// <param name="oldName">The new name of the project</param>
		static void Start(string newName, string oldName)
		{
			Console.WriteLine("\nANETemplate Renamer by Juan Calle\n");

			Console.WriteLine($"\n### Renaming the project from {oldName} to {newName} ###\n");

			if(Execute($"if exist {Path}/{oldName} echo yes").IndexOf("yes") < 0)
			{
				Console.WriteLine($"The current project does not have the name {oldName} so it cannot be renamed");
            }

			Console.WriteLine($"## Working on {Path} ##\n");

			Ren(oldName, newName);
		}
		/// <summary>
		///		Rename a file or directory
		/// </summary>
		/// <param name="oldName">the current name of the file or directory</param>
		/// <param name="newName">the new name of the file or directory</param>
		static void Ren(string oldName, string newName)
		{
			if(Execute($"if exist \"{Path}{oldName}\" (ren \"{Path}{oldName}\" {newName}) else (echo error)").IndexOf("error")>=0)
            {
				Console.WriteLine($"{Path}{oldName} cannot be renamed because it does not exist, this may be because it has been deleted or a renaming process was canceled");
            }
		}
		/// <summary>
		///		Program entry point
		/// </summary>
		/// <param name="args">The command line arguments</param>
		/// <returns></returns>
		static int Main(String[] args)
		{
			// Create a root command with some options
			var rootCommand = new RootCommand
			{
				new Option<string>(
					new string[]{"--new-name","-nn"},
					"The new name for the project. " +
					"If this option is not specified, it will be requested in the program."),
				new Option<string>(
					new string[]{"--old-name","-on"},
					()=>"TemplateANE",
					"Indicates the current name of the project, it will be replaced by the new name.")
			};

			rootCommand.Description = "My sample app";

			rootCommand.Handler = CommandHandler.Create<string, string>((string newName,string oldName) =>
			{
				Console.Clear();
				while (newName == null || newName.Length < 1)
				{
					Console.Write(
						"\nANETemplate Renamer by Juan Calle\n" +
						"\nInsert the name of the new project: ");
					newName = Console.ReadLine();
					Console.Clear();
				}
				Start(newName, oldName);
				Console.ReadKey();
			});

			// Parse the incoming args and invoke the handler
			return rootCommand.InvokeAsync(args).Result;
		}
		/// <summary>
		///		Run a command in the windows cmd
		/// </summary>
		/// <param name="_Command">The complete command as it would be written in cmd</param>
		/// <returns></returns>
		private static string Execute(string _Command)
		{
			System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + _Command);

			procStartInfo.RedirectStandardOutput = true;
			procStartInfo.UseShellExecute = false;

			procStartInfo.CreateNoWindow = false;

			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = procStartInfo;
			proc.Start();

			return proc.StandardOutput.ReadToEnd();
		}
	}
}
