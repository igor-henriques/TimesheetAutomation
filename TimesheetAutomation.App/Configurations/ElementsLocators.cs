namespace TimesheetAutomation.App.Configurations;

internal sealed record ElementsLocators
{
    public string UsuarioXPath { get; init; }
    public string SenhaXPath { get; init; }
    public string LoginXPath { get; init; }
    public string MenuXPath { get; init; }
    public string CentroDeCustoXPath { get; init; }
    public string AtividadeXPath { get; init; }
    public string InicioHoraXPath { get; init; }
    public string InicioMinutoXPath { get; init; }
    public string FinalHoraXPath { get; init; }
    public string FinalMinutoXPath { get; init; }
    public string CalendarioDiaId { get; init; }
    public string GravarDadosXPath { get; init; }
    public string RegistroTabelaXPath { get; init; }
}
