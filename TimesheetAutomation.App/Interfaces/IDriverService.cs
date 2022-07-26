namespace TimesheetAutomation.App.Interfaces;

internal interface IDriverService : IDisposable
{
    Task ClickOnElement(By elementLocator, bool verifyExistence = true);
    IWebElement GetElement(By elementLocator, bool verifyExistence = true);
    string GetElementContent(By elementLocator, bool verifyExistence = true);
    void InsertTextOnElement(By elementLocator, string text, bool verifyExistence = true);
    void Navigate(string URL);
    void WaitUntilElementExists(By elementLocator);
}