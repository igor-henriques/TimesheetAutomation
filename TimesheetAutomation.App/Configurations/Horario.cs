namespace TimesheetAutomation.App.Configurations;

internal sealed record Horario
{
    public int Hora { get; init; }
    public int Minuto { get; init; }
}