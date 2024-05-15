using ManageYourGameServers.Core.Services;

var steamCmdService = new SteamCmdService();
await steamCmdService.AcquireSteamCmdAsync();
await steamCmdService.ExtractSteamCmdExecutableAsync();
await steamCmdService.RunSteamCmdAsync();

Console.WriteLine("Console Program Ended.");
Console.ReadLine();
