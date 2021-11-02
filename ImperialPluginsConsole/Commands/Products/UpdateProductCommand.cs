using ImperialPlugins;
using ImperialPluginsConsole.Interfaces;
using ImperialPluginsConsole.Models;
using ImperialPluginsConsole.Models.Attributes;
using System;
using System.IO;
using System.Security;
using System.Threading;

namespace ImperialPluginsConsole.Commands.Products
{
    [CommandParent(typeof(ProductsCommand))]
    public class UpdateProductCommand : ICommand
    {
        public string Name => "Update";

        public string Syntax => "Update -p [Plugin] -b [branch] -v [version] -cl [changelog] -f [filePath] -force -[nc|No Confirmation]";

        public string Description => "Updates a plugin. ";

        private ImperialPluginsClient m_IP;
        private CommandContext m_Context;
        private CacheClient m_Cache;

        public UpdateProductCommand(ImperialPluginsClient iP, CommandContext context, CacheClient cache)
        {
            m_Context = context;
            m_IP = iP;
            m_Cache = cache;
        }

        public void Execute(ICommandOut cmdOut)
        {
            var args = m_Context
                .ArgumentParser
                    .WithDependants("p", "b", "v", "cl", "f")
                    .WithIndependants("force", "nc")
                    .Parse();
            args.Enforce("p", "b", "v", "f");

            var rawPlugin = args["p"];
            var branch = args["b"];
            var rawVersion = args["v"];
            var changeLog = "";
            if (args.ContainsKey("cl"))
                changeLog = args["cl"];
            var requireConfirmation = !args.ContainsKey("nc");
            var filePath = args["f"];

            var force = args.ContainsKey("force");

            string pluginName;

            int pluginID;
            if (!int.TryParse(rawPlugin, out pluginID))
            {
                var pl = m_Cache.GetPluginByName(rawPlugin);
                if (pl == null)
                {
                    cmdOut.Write("Failed to resolve plugin name '", ConsoleColor.Red);
                    cmdOut.Write(rawPlugin, ConsoleColor.Yellow);
                    cmdOut.WriteLine("' to product ID; Unknown product", ConsoleColor.Red);
                    return;
                }
                pluginName = pl.Name;
                pluginID = pl.ID;
            }
            else if (requireConfirmation)
            {
                var pl = m_Cache.GetPlugin(pluginID);

                if (pl == null)
                {
                    cmdOut.Write("Failed to resolve plugin ID ", ConsoleColor.Red);
                    cmdOut.Write(rawPlugin, ConsoleColor.Yellow);
                    cmdOut.WriteLine(" to product; Unknown product ID", ConsoleColor.Red);
                    return;
                }
                pluginName = pl.Name;
            }
            else
            {
                pluginName = "Unknown Plugin";
            }

            if (!File.Exists(filePath))
            {
                cmdOut.Write("Failed to find plugin file at path '", ConsoleColor.Red);
                cmdOut.Write(filePath, ConsoleColor.Yellow);
                cmdOut.WriteLine("'", ConsoleColor.Red);
                return;
            }

            if (!Version.TryParse(rawVersion, out var version))
            {
                cmdOut.Write("Failed to parse '", ConsoleColor.Red);
                cmdOut.Write(rawVersion, ConsoleColor.Yellow);
                cmdOut.WriteLine("' as a version string.", ConsoleColor.Red);
                return;
            }

            var fInf = new FileInfo(filePath);
            var fileName = fInf.Name;

            byte[]? data = null;

            string? failReason = null;
            try
            {
                data = File.ReadAllBytes(filePath);
            }
            catch (PathTooLongException)
            {
                failReason = "Path Too Long";
            }
            catch (DirectoryNotFoundException)
            {
                failReason = "Directory not found.";
            }
            catch (IOException)
            {
                failReason = "Error in I/O";
            }
            catch (UnauthorizedAccessException)
            {
                failReason = "Unauthorized";
            }
            catch (SecurityException)
            {
                failReason = "Security Violation";
            }
            if (data == null)
            {
                failReason = "Data Error";
            }
            else if (data.Length == 0)
            {
                failReason = "File Empty";
            }

            if (failReason != null || data == null)
            {
                cmdOut.Write("Failed to read plugin data; ", ConsoleColor.Red);
                cmdOut.WriteLine(failReason ?? "Unknown Error", ConsoleColor.Yellow);
                return;
            }

            if (requireConfirmation)
            {
                cmdOut.WriteLine();
                cmdOut.WriteLine("Update Details", ConsoleColor.Blue);

                cmdOut.Write(" Plugin: ", ConsoleColor.Yellow);
                cmdOut.Write("{0}", ConsoleColor.Cyan, pluginName);
                cmdOut.Write(" [", ConsoleColor.Yellow);
                cmdOut.Write(pluginID, ConsoleColor.Cyan);
                cmdOut.WriteLine("]", ConsoleColor.Yellow);

                cmdOut.Write(" Branch: ", ConsoleColor.Yellow);
                cmdOut.WriteLine(branch, ConsoleColor.Cyan);

                cmdOut.Write(" Version: ", ConsoleColor.Yellow);
                cmdOut.WriteLine(version, ConsoleColor.Cyan);

                cmdOut.Write(" Change Log: ", ConsoleColor.Yellow);
                cmdOut.WriteLine(changeLog, ConsoleColor.Cyan);

                cmdOut.Write(" Plugin Path: ", ConsoleColor.Yellow);
                cmdOut.WriteLine(fInf.FullName, ConsoleColor.Cyan);

                cmdOut.Write(" Plugin File Size: ", ConsoleColor.Yellow);
                cmdOut.WriteLine("{0} bytes", ConsoleColor.Cyan, data.Length);

                if (string.IsNullOrWhiteSpace(changeLog))
                {
                    cmdOut.Write("WARN: ", ConsoleColor.Red);
                    cmdOut.WriteLine("No changelog provided.", ConsoleColor.Yellow);
                }
                if (force)
                {
                    cmdOut.Write("WARN: ", ConsoleColor.Yellow);
                    cmdOut.WriteLine("Force Update is enabled.", ConsoleColor.DarkCyan);
                }

                cmdOut.WriteLine();
                cmdOut.WriteLine("Are you sure you want to push this update?");
                Thread.Sleep(2000);
                cmdOut.WriteLine("[Y/N]", ConsoleColor.Cyan);
                var k = Console.ReadKey();
                if (k.Key != ConsoleKey.Y)
                {
                    cmdOut.WriteLine("Update aborted.", ConsoleColor.Cyan);
                    return;
                }
            }

            cmdOut.WriteLine("Updating product...", ConsoleColor.Cyan);
            m_IP.UpdatePlugin(pluginID, branch, rawVersion, changeLog, data, fileName, force);
            cmdOut.WriteLine("Product Updated.", ConsoleColor.Cyan);

            m_Cache.StartInit(); // Refresh cache
        }
    }
}