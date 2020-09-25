using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Collections.Generic;

namespace ANETemplate_Renamer
{
	class Program
	{

		static void Start(string newName, string oldName)
        {
			Console.WriteLine("\nANETemplate Renamer by Juan Calle\n");
			Console.WriteLine($"\n### Renaming the project from {oldName} to {newName} ###");
		}
		static int Main(String[] args)
		{
			// Create a root command with some options
			var rootCommand = new RootCommand
			{
				new Option<string>(
					"--new-name",
					"The new name for the project. " +
					"If this option is not specified, it will be requested in the program."),
				new Option<string>(
					"--old-name",
					()=>"TemplateANE",
					"Indicates the current name of the project, it will be replaced by the new name.")
			};

			rootCommand.Description = "My sample app";

			// Note that the parameters of the handler method are matched according to the names of the options
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
