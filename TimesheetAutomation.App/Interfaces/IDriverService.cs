namespace TimesheetAutomation.App.Interfaces;

internal interface IDriverService
{
    Task ClickOnElement(By elementLocator, bool verifyExistence = true);
    bool ElementExists(By elementLocator);
    string GetCurrentURL();
    IWebElement GetElement(By elementLocator, bool verifyExistence = true);
    string GetElementContent(By elementLocator, bool verifyExistence = true);
    void InsertTextOnElement(By elementLocator, string text, bool verifyExistence = true);
    void Navigate(string URL);
    void Refresh();
    void WaitUntilElementExists(By elementLocator);
}