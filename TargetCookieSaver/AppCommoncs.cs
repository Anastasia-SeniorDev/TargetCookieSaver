using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetCookieSaver
{
    public static class AppCommon
    {
        public static PuppeteerSharp.Browser browser { get; set; }

        public static bool isbrowserClosed = false;
        public static bool isbrowserInitiated { get; set; } = false;
        public static async void KillBrowser()
        {
            isbrowserClosed = false;
            await browser.CloseAsync();
            Process[] processCollection = Process.GetProcesses().ToList().ToArray();
            foreach (Process p in processCollection)
            {
                try
                {
                    if (p.MainModule.FileName.Contains("chrome-win"))
                    {
                        p.Kill();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            isbrowserClosed = true;
        }

        public static async void LaunchBrowser(bool showbrowser)
        {
            browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Args = new[]
    {

        "--no-sandbox",
        "--disable-plugins",
        "--disable-sync",
        "--disable-gpu",
        "--disable-speech-api",
        "--disable-remote-fonts",
        "--disable-shared-workers",
        "--disable-webgl",
        "--no-experiments",
        "--no-first-run",
        "--no-default-browser-check",
        "--no-wifi",
        "--no-pings",
        "--no-service-autorun",
        "--disable-databases",
        "--disable-default-apps",
        "--disable-demo-mode",
        "--disable-notifications",
        "--disable-permissions-api",
        "--disable-background-networking",
        "--disable-3d-apis",
        "--ignore-certificate-errors",
        "--enable-features=NetworkService",
        "--ignore-certificate-errors-spki-list",
        "-disable-setuid-sandbox",
        "--disable-bundled-ppapi-flash",
    },
                DefaultViewport = null,
                Headless = showbrowser,
                IgnoreHTTPSErrors = true


            });
            isbrowserInitiated = true;
        }
    }
}
