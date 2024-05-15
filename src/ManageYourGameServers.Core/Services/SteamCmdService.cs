using System.Diagnostics;
using System.IO.Compression;

namespace ManageYourGameServers.Core.Services;

public class SteamCmdService
{
    public async Task AcquireSteamCmdAsync()
    {
        var steamCmdUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
        var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "steamcmd.zip");

        using var client = new HttpClient();

        try
        {
            using var response = await client.GetAsync(steamCmdUrl, HttpCompletionOption.ResponseHeadersRead);
            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = File.Create(downloadPath);

            await stream.CopyToAsync(fileStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task ExtractSteamCmdExecutableAsync()
    {
        var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "steamcmd.zip");
        var extractionPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SteamCMD");

        try
        {
            await Task.Run(() => ZipFile.ExtractToDirectory(downloadPath, extractionPath));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task RunSteamCmdAsync()
    {
        var steamCmdFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SteamCMD", "steamcmd.exe");
        var serverDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SoF");
        var arguments = $"+login anonymous +force_install_dir \"{serverDirectoryPath}\" +app_update 2465200 validate +quit";

        using var process = new Process();
        process.StartInfo.FileName = steamCmdFilePath;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        await ReadStreamAsync(process.StandardOutput, Console.Out);
        await ReadStreamAsync(process.StandardError, Console.Error);

        await process.WaitForExitAsync();
    }

    // TODO: Output of SteamCMD doesn't buffer properly. This will take some work to output in real time.
    // This link outlines the specifics of the issue: https://github.com/ValveSoftware/Source-1-Games/issues/3658
    private static async Task ReadStreamAsync(StreamReader reader, TextWriter writer)
    {
        var buffer = new char[4096];
        int bytesRead;

        while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            var output = new string(buffer, 0, bytesRead);
            await writer.WriteAsync(output);
        }
    }

    private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            Console.WriteLine(e.Data);
        }
    }

    private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            Console.WriteLine(e.Data);
        }
    }
}
