// LFInteractive LLC. 2021-2024
﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Timer = System.Timers.Timer;

namespace Restart_Palworld;

internal class Program
{
    private static void Main()
    {
        string currentPath = AppDomain.CurrentDomain.BaseDirectory;
        string file = Path.Combine(currentPath, "settings.json");

        Config.Instance.Initialize(file);
        if (string.IsNullOrWhiteSpace(Config.Instance.Username) || string.IsNullOrWhiteSpace(Config.Instance.Password) || string.IsNullOrWhiteSpace(Config.Instance.ServerId))
        {
            MessageBoxW(0, "Please fill out the settings.json file with your username, password and server-id (and optionally the interval in seconds", "Restart Palworld", 0);
            Process.Start(new ProcessStartInfo()
            {
                FileName = file,
                UseShellExecute = true,
            });
            return;
        }

        if (Config.Instance.RestartOnApplicationStart)
            Restart();

        Timer timer = new(Config.Instance.Interval)
        {
            AutoReset = true,
            Enabled = true,
        };
        timer.Elapsed += (sender, args) => Restart();
        timer.Start();
    }

    private static void Restart()
    {
        using HttpClient client = new();
        string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Config.Instance.Username}:{Config.Instance.Password}"));
        using HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"https://dathost.net/api/0.1/game-servers/{Config.Instance.ServerId}/start"),
        };
        request.Headers.Add("Authorization", $"Basic {auth}");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            MessageBoxW(0, "Failed to restart server", "Restart Palworld", 0);
        }
        else
        {
            if (Config.Instance.ShowMessageOnRestart)
                MessageBoxW(0, "Server restarted successfully!", "Restart Palworld", 0);
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(int hWnd, string msg, string caption, int type);
}