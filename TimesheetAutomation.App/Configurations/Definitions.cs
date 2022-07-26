namespace TimesheetAutomation.App.Configurations;

internal record Definitions
{
    [JsonProperty("Usuario")]
    public string Usuario { get; init; }

    [JsonProperty("Senha")]
    public string Senha { get; init; }

    [JsonProperty("MainURL")]
    public string MainURL { get; private set; }
    
    [JsonProperty("Centro de Custo")]
    public string CentroDeCusto { get; init; }
    [JsonProperty("Atividade")]
    public string Atividade { get; init; }
    [JsonProperty("HorarioInicio")]
    public Horario HorarioInicio { get; init; }
    [JsonProperty("HorarioFinal")]
    public Horario HorarioFinal { get; init; }
    public ElementsLocators Locators { get; private set; }
    private Definitions() { }

    public static async Task<Definitions> CreateAsync()
    {
        var definitions = JsonConvert.DeserializeObject<Definitions>(await File.ReadAllTextAsync("./Configurations/Definitions.json"));

        definitions.Locators = JsonConvert.DeserializeObject<ElementsLocators>(await File.ReadAllTextAsync("./Configurations/ElementsLocators.json"));
        definitions.MainURL = string.Format(definitions.MainURL, DateTime.Now.Year, DateTime.Now.Month, 20);

        return definitions;
    }
}