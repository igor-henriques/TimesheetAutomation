namespace TimesheetAutomation.App.Services;

internal class DriverService : IDriverService
{
    private readonly ChromeDriver _driver;
    private readonly WebDriverWait _wait;

    public DriverService()
    {
        this._driver = new ChromeDriver(DriverUtils.GetChromeDriverService(), DriverUtils.GetChromeOptions());
        this._wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        this._wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
    }

    public void Navigate(string URL)
    {
        _driver.Navigate().GoToUrl(URL);
    }

    public void Refresh()
    {
        _driver.Navigate().Refresh();
    }

    public IWebElement GetElement(By elementLocator, bool verifyExistence = true)
    {
        if (verifyExistence)
            WaitUntilElementExists(elementLocator);

        return _driver.FindElement(elementLocator);
    }

    public string GetElementContent(By elementLocator, bool verifyExistence = true)
    {
        var element = GetElement(elementLocator, verifyExistence);

        return element?.Text;
    }

    public async Task ClickOnElement(By elementLocator, bool verifyExistence = true)
    {
        var element = GetElement(elementLocator, verifyExistence);

        while (!element.Displayed | !element.Enabled)
            await Task.Delay(100);

        element.Click();
    }

    public void InsertTextOnElement(By elementLocator, string text, bool verifyExistence = true)
    {
        var element = GetElement(elementLocator, verifyExistence);

        element.SendKeys(text);
    }
    public string GetCurrentURL()
    {
        return _driver.Url;
    }

    public bool ElementExists(By elementLocator)
    {
        try
        {
            return _driver.FindElement(elementLocator) != null;
        }
        catch (Exception) { }

        return false;
    }

    public void WaitUntilElementExists(By elementLocator)
    {
        _wait.Until(_driver =>
        {
            try
            {
                var func = ExpectedConditions.ElementIsVisible(elementLocator);
                var element = func.Invoke(_driver);

                return element != null;
            }
            catch { return false; }
        });
    }
}
