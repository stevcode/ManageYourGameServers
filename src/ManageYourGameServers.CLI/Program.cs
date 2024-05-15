using ManageYourGameServers.Core.Services;

var steamCmdService = new SteamCmdService();
await steamCmdService.AcquireSteamCmdAsync();
await steamCmdService.ExtractSteamCmdExecutableAsync();