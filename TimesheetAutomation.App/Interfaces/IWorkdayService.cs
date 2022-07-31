namespace TimesheetAutomation.App.Interfaces;

internal interface IWorkdayService
{
    ValueTask<IEnumerable<DateTime>> GetHolidays();
}