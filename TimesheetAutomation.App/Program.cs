DriverUtils.CleanDriverGarbage();

if (!IsVpnConnected())
    throw new Exception("Desconectado da VPN.");

var definitions = await Definitions.GetInstanceAsync();

IDriverService _driverService = new DriverService();
IWorkdayService workdayService = new WorkdayService();

try
{
    await LoginAsync();
    await MarkTimesheet();

    _driverService?.Dispose();
}
catch (Exception ex)
{
    LogWriter.Write(ex.ToString());
    _driverService?.Dispose();
    throw;
}

async Task LoginAsync()
{
    _driverService.Navigate(definitions.MainURL);

    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.UsuarioXPath), definitions.Usuario);
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.SenhaXPath), definitions.Senha);
    await _driverService.ClickOnElement(By.XPath(definitions.Locators.LoginXPath));

    _driverService.WaitUntilElementExists(By.XPath(definitions.Locators.MenuXPath));
}

async Task MarkTimesheet()
{
    for (int day = 1; day <= DateTime.Now.Day; day++)
    {
        if (!await IsWorkDay(day))
            continue;

        string baseUrl = string.Format(definitions.MainURL, DateTime.Now.Year, DateTime.Now.Month, day);

        _driverService.Navigate(baseUrl);

        if (IsDayAlreadyChecked())
            continue;

        foreach (var horario in definitions.Horarios)
        {
            _driverService.InsertTextOnElement(By.XPath(definitions.Locators.CentroDeCustoXPath), definitions.CentroDeCusto);
            _driverService.InsertTextOnElement(By.XPath(definitions.Locators.AtividadeXPath), definitions.Atividade);
            _driverService.InsertTextOnElement(By.XPath(definitions.Locators.InicioHoraXPath), horario.HoraInicio.ToString("00"));
            _driverService.InsertTextOnElement(By.XPath(definitions.Locators.InicioMinutoXPath), horario.MinutoInicio.ToString("00"));
            _driverService.InsertTextOnElement(By.XPath(definitions.Locators.FinalHoraXPath), horario.HoraFinal.ToString("00"));
            _driverService.InsertTextOnElement(By.XPath(definitions.Locators.FinalMinutoXPath), horario.MinutoFinal.ToString("00"));
            await _driverService.ClickOnElement(By.XPath(definitions.Locators.GravarDadosXPath));

            VerifyTimesheet(horario);
        }
    }
}

bool IsDayAlreadyChecked()
{
    _driverService.WaitUntilElementExists(By.XPath(definitions.Locators.GravarDadosXPath));

    return _driverService.GetElementContent(By.XPath(definitions.Locators.RegistroTabelaXPath)).Equals("0.00") ? false : true;
}

void VerifyTimesheet(Horario horario)
{
    if (string.IsNullOrEmpty(_driverService.GetElementContent(By.XPath(definitions.Locators.RegistroTabelaXPath))))
    {
        LogWriter.Write($"Houve um erro ao marcar ponto no dia {DateTime.Today.ToShortDateString()}.");
        return;
    }                                                                                                                                                                                                                                                                                                                            

    LogWriter.Write($"Ponto do dia {DateTime.Today.ToShortDateString()} marcado com sucesso.\n" +
        $"Hora Início:{horario.HoraInicio.ToString("00")}:{horario.MinutoInicio.ToString("00")}\n" +
        $"Hora Final:{horario.HoraFinal.ToString("00")}:{horario.MinutoFinal.ToString("00")}");
}

async ValueTask<bool> IsWorkDay(int day)
{
    var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);

    bool isWeekend = currentDate is { DayOfWeek: DayOfWeek.Sunday } or { DayOfWeek: DayOfWeek.Saturday };

    if (isWeekend)
        return false;

    if (definitions.SafelistHolidayDates.Any(d => d.Day.Equals(currentDate.Day)     &
                                                  d.Month.Equals(currentDate.Month) &
                                                  d.Year.Equals(currentDate.Year)))
    {
        return false;
    }

    var holidays = await workdayService.GetHolidays();

    if (holidays.Any(d => d.Day.Equals(currentDate.Day) &
                     d.Month.Equals(currentDate.Month) &
                     d.Year.Equals(currentDate.Year)))
    {
        return false;
    }

    return true;
}

bool IsVpnConnected()
{
    if (NetworkInterface.GetIsNetworkAvailable())
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface Interface in interfaces)
        {
            if (Interface.OperationalStatus == OperationalStatus.Up)
            {
                return (Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback);
            }
        }
    }

    return false;
}