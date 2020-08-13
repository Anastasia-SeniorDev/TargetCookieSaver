using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TargetCookieSaver
{
    public class TargetSiteBrowser
    {

        Page page;
        public bool showBrowser { get; set; } = true;
        public TargetSiteBrowser(bool showBrowser)
        {
            this.showBrowser = showBrowser;
        }
        public async void Login(string username, string password,ProgressBar pbar)
        {
            AppCommon.isbrowserInitiated = false;
            AppCommon.LaunchBrowser(showBrowser);
            while (AppCommon.isbrowserInitiated == false)
            {
                Application.DoEvents();
            }
            page = await AppCommon.browser.NewPageAsync();
            var timeout = TimeSpan.FromSeconds(240).Milliseconds; // default value
            page.DefaultNavigationTimeout = timeout;
            page.DefaultTimeout = timeout;
            var options = new NavigationOptions { Timeout = timeout };
            await page.GoToAsync("https://www.target.com/", options);
            await page.ClickAsync("#account", null);
            await page.WaitForSelectorAsync("#accountNav-signIn > a");
            await Task.Delay(2000);
            await page.ClickAsync("#accountNav-signIn > a", null);
            await Task.Delay(3000);
            ElementHandle loginbutton = await PopuplateCredentials(username, password);
            await loginbutton.ClickAsync();
            string codecommand = "document.documentElement.outerHTML";
            string pagecode = await page.EvaluateExpressionAsync<string>(codecommand);
            await Task.Delay(3000);
            try
            {
                SkipScreens();
            }
            catch (Exception ex)
            {
            }
            await Task.Delay(6000);
            await SaveCookies();
            pbar.Visible = false;
            await page.CloseAsync();
            page.Dispose();
            await AppCommon.browser.CloseAsync();
        }

        private async Task SaveCookies()
        {
            var cookies = await page.GetCookiesAsync();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(cookies);
            File.WriteAllText("Cookies.json", json);
            MessageBox.Show("Cookies Saved successfully...");
        }

        private async Task<ElementHandle> PopuplateCredentials(string username, string password)
        {
            var usernamee = await page.WaitForSelectorAsync("#username");
            var passworde = await page.WaitForSelectorAsync("#password");
            var loginbutton = await page.WaitForSelectorAsync("#login");
            await usernamee.TypeAsync(username);
            await passworde.TypeAsync(password);
            return loginbutton;
        }

        public async void LoginWithCookie(string path,ProgressBar pbar)
        {
            AppCommon.isbrowserInitiated = false;
            AppCommon.LaunchBrowser(showBrowser);
            while (AppCommon.isbrowserInitiated == false)
            {
                Application.DoEvents();
            }
            page = null;
            page = await AppCommon.browser.NewPageAsync();
            string json = File.ReadAllText("Cookies.json");
            var re = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CookieParam>>(json);
            await page.SetCookieAsync(re.ToArray());
            var timeout = TimeSpan.FromSeconds(240).Milliseconds;    //default value
            page.DefaultNavigationTimeout = timeout;
            page.DefaultTimeout = timeout;
            var options = new NavigationOptions { Timeout = timeout };
            await page.GoToAsync("https://www.target.com/p/mr-coffee-12-cup-programmable-coffee-maker-with-dishwashable-design-bvmc-lmx120/-/A-54617167/", options);
            await Task.Delay(4000);
            MessageBox.Show("Cookie login successfull..");
            pbar.Visible = false;
            //await page.WaitForNavigationAsync();
            await page.CloseAsync();
            await AppCommon.browser.CloseAsync();
        }

        public async void SkipScreens()
        {
            ElementHandle[] elements = await page.QuerySelectorAllAsync("a");
            foreach (ElementHandle eh in elements)
            {
                var innertext = await eh.GetPropertyAsync("innerText");
                var value = await innertext.JsonValueAsync();
                if (value.ToString() == "Skip")
                {
                    await eh.ClickAsync();
                    break;
                }
            }

            ElementHandle[] elementHandles = await page.QuerySelectorAllAsync("button");
            foreach (ElementHandle eh in elementHandles)
            {
                var innertext = await eh.GetPropertyAsync("innerText");
                var value = await innertext.JsonValueAsync();
                if (value.ToString() == "Skip")
                {
                    await eh.ClickAsync();
                    break;
                }
            }
        }
    }
}
