using System.Diagnostics;

namespace TimesheetAutomation.App.Utils;

internal class DriverUtils
{
    public static ChromeDriverService GetChromeDriverService()
    {
        ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();

        driverService.HideCommandPromptWindow = true;

        return driverService;
    }

    public static ChromeOptions GetChromeOptions()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArguments(new List<string>()
        {
            "--disable-blink-features=AutomationControlled",
            "--disable-dev-shm-usage",
            "--no-sandbox",
            "--headless",
            "--disable-impl-side-painting",
            "--disable-setuid-sandbox",
            "--disable-seccomp-filter-sandbox",
            "--disable-breakpad",
            "--disable-client-side-phishing-detection",
            "--disable-cast",
            "--disable-cast-streaming-hw-encoding",
            "--disable-cloud-import",
            "--disable-popup-blocking",
            "--ignore-certificate-errors",
            "--disable-session-crashed-bubble",
            "--disable-ipv6",
            "--allow-http-screen-capture",
            "--start-maximized"
        });

        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);

        return options;
    }

    public static void CleanDriverGarbage()
    {
        foreach (Process instance in Process.GetProcessesByName("chromedriver"))
        {
            try
            {
                instance.Kill();
            }
            catch (Exception) { }
        }

        foreach (Process instance in Process.GetProcessesByName("conhost"))
        {
            try
            {
                instance.Kill();
            }
            catch (Exception) { }
        }
    }
}
