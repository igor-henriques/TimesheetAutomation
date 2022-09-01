namespace TimesheetAutomation.App.Services;

internal class WorkdayService : IWorkdayService
{
    private IEnumerable<DateTime> _holidays;
    private readonly HttpClient _client;

    public WorkdayService()
    {
        this._client = new HttpClient();
    }

    public async ValueTask<IEnumerable<DateTime>> GetHolidays()
    {
        if (_holidays is not null)
            return _holidays;

        var response = await _client.GetAsync($"https://brasilapi.com.br/api/feriados/v1/{DateTime.Now.Year}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro ao obter lista de feriados");

        this._holidays = JsonConvert.DeserializeObject<IEnumerable<Holiday>>(await response.Content.ReadAsStringAsync())
                                    .Select(x => x.date);

        return _holidays;
    }

    private record struct Holiday(DateTime date, string name, string type);
}
