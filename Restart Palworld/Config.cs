// LFInteractive LLC. 2021-2024
﻿using Chase.CommonLib.FileSystem.Configuration;
using Newtonsoft.Json;

namespace Restart_Palworld;

public class Config : AppConfigBase<Config>
{
    [JsonProperty("username")]
    public string Username { get; set; } = "";

    [JsonProperty("password")]
    public string Password { get; set; } = "";

    [JsonProperty("interval")]
    public TimeSpan Interval { get; set; } = TimeSpan.FromHours(1);

    [JsonProperty("restart-on-application-start")]
    public bool RestartOnApplicationStart { get; set; } = true;

    [JsonProperty("show-message-on-restart")]
    public bool ShowMessageOnRestart { get; set; } = false;

    [JsonProperty("server-id")]
    public string ServerId { get; set; } = "";
}