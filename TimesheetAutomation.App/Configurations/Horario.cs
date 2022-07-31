namespace TimesheetAutomation.App.Configurations;

internal sealed record Horario
{
    public int HoraInicio { get; init; }
    public int MinutoInicio { get; init; }
    public int HoraFinal { get; init; }
    public int MinutoFinal { get; init; }
}