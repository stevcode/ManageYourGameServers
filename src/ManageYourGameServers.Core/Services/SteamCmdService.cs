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
        var extractionPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        try
        {
            await Task.Run(() => ZipFile.ExtractToDirectory(downloadPath, extractionPath));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
