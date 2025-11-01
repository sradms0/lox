using System.Diagnostics;
using FluentAssertions;
using Frontend;
using NUnit.Framework;
using Shared;

namespace Integration;

[TestFixture]
public class ExitHandlerShould : CommonTestBase
{
    [Test]
    public void Exit()
    {
        // Arrange
        const int ExitUsageCode = 64;
        int? exitCodeResult = null;
        var exitHandlerExecutablePath = Path.Combine("..", "Integration.ExitHandlerExecutable", "bin", "Debug", "net9.0", "Integration.ExitHandlerExecutable.dll");
        var projectAPath = Path.GetFullPath(Path.Combine($"{AppContext.BaseDirectory}/../../../", exitHandlerExecutablePath));
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"\"{projectAPath}\" {ExitUsageCode} ",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };
        
        // Act
        using (var process = Process.Start(processStartInfo))
        {
            if (process != null)
            {
                process.WaitForExit();
                exitCodeResult = process.ExitCode;
            }
        }
        
        // Assert
        exitCodeResult.Should().Be(ExitUsageCode);
    }
}