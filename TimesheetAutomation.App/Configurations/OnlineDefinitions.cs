namespace TimesheetAutomation.App.Configurations;

internal record OnlineDefinitions
{
    [JsonProperty("Usuario")]
    public string Usuario { get; init; }

    [JsonProperty("Senha")]
    public string Senha { get; init; }

    [JsonProperty("MainURL")]
    public string MainURL { get; set; }

    [JsonProperty("Centro de Custo")]
    public string CentroDeCusto { get; init; }
    [JsonProperty("Atividade")]
    public string Atividade { get; init; }
    [JsonProperty("Horários")]
    public IEnumerable<Horario> Horarios { get; init; }
    public ElementsLocators Locators { get; private set; }
    [JsonProperty("Safelist")]
    private IEnumerable<string> SafelistDates { get; init; } = Enumerable.Empty<string>();
    public List<DateOnly> SafelistHolidayDates { get; private set; } = new List<DateOnly>();
    private OnlineDefinitions() { }

    public static async Task<OnlineDefinitions> GetInstanceAsync()
    {
        var definitions = JsonConvert.DeserializeObject<OnlineDefinitions>(await File.ReadAllTextAsync("./Configurations/Definitions.json"));

        if (string.IsNullOrEmpty(definitions.Usuario) | string.IsNullOrEmpty(definitions.Senha))
        {
            LogWriter.Write("Usuario e/ou senha vazios no arquivo Definitions.json");
            Environment.Exit(99);
        }

        definitions.Locators = JsonConvert.DeserializeObject<ElementsLocators>(await File.ReadAllTextAsync("./Configurations/ElementsLocators.json"));

        if (!definitions.SafelistDates.Any())
        {
            return definitions;
        }

        List<string> intervalDates = new List<string>();

        definitions.SafelistHolidayDates = definitions.SafelistDates.Select(date =>
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                intervalDates.Add(date);
            }

            return parsedDate;
        }).Where(date => date != default(DateOnly)).ToList();

        if (!intervalDates.Any(x => x.Contains("~")))
        {
            return definitions;
        }

        foreach (var intervalDate in intervalDates)
        {
            var dates = intervalDate.Replace(" ", string.Empty)
                .Split('~')
                .Select(date =>
                {
                    if (!DateOnly.TryParse(date, out DateOnly parsedDate))
                    {
                        LogWriter.Write($"Definição de intervalo de datas inválida: {intervalDate}. Caractere inválido: utilize '~' para separar as datas");
                        Environment.Exit(99);
                    }

                    return parsedDate;
                }).ToArray();

            if (dates.Length != 2)
            {
                LogWriter.Write($"Definição de intervalo de datas inválida: {intervalDate}. Mais que 2 intervalos definido.");
                Environment.Exit(99);
            }

            if (dates[0] > dates[1])
            {
                LogWriter.Write($"Definição de intervalo de datas inválida: {intervalDate}. Data inicial maior que data final.");
                Environment.Exit(99);
            }

            while (dates[0] != dates[1])
            {
                definitions.SafelistHolidayDates.Add(dates[0]);

                dates[0] = dates[0].AddDays(1);
            }
        }

        definitions.SafelistHolidayDates = definitions.SafelistHolidayDates.OrderBy(date => date).ToList();

        return definitions;
    }
}