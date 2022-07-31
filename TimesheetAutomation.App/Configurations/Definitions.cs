namespace TimesheetAutomation.App.Configurations;

internal record Definitions
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
    public IEnumerable<DateOnly> SafelistHolidayDates { get; private set; } = Enumerable.Empty<DateOnly>();
    private Definitions() { }

    public static async Task<Definitions> CreateAsync()
    {
        var definitions = JsonConvert.DeserializeObject<Definitions>(await File.ReadAllTextAsync("./Configurations/Definitions.json"));

        if (string.IsNullOrEmpty(definitions.Usuario) | string.IsNullOrEmpty(definitions.Senha))
        {
            LogWriter.Write("Usuario e/ou senha vazios no arquivo Definitions.json");
            Environment.Exit(0);
        }

        definitions.Locators = JsonConvert.DeserializeObject<ElementsLocators>(await File.ReadAllTextAsync("./Configurations/ElementsLocators.json"));

        if (definitions.SafelistDates.Any())
            definitions.SafelistHolidayDates = definitions.SafelistDates.Select(d => DateOnly.Parse(d));

        return definitions;
    }
}