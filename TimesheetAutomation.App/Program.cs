DriverUtils.CleanDriverGarbage();

var definitions = await Definitions.CreateAsync();

IDriverService _driverService = new DriverService();

try
{
    await LoginAsync();
    await MarkTimesheet();

    VerifyTimesheet();

    _driverService.Dispose();
}
catch (Exception ex)
{
    LogWriter.Write(ex.ToString());

    throw;
}

async Task LoginAsync()
{
    _driverService.Navigate(definitions.MainURL);

    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.UsuarioXPath), definitions.Usuario);
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.SenhaXPath), definitions.Senha);
    await _driverService.ClickOnElement(By.XPath(definitions.Locators.LoginXPath));

    _driverService.WaitUntilElementExists(By.XPath(definitions.Locators.MenuXPath));

    _driverService.Navigate(definitions.MainURL);
}

async Task MarkTimesheet()
{
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.CentroDeCustoXPath), definitions.CentroDeCusto);
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.AtividadeXPath), definitions.Atividade);
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.InicioHoraXPath), definitions.HorarioInicio.Hora.ToString("00"));
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.InicioMinutoXPath), definitions.HorarioInicio.Minuto.ToString("00"));
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.FinalHoraXPath), definitions.HorarioFinal.Hora.ToString("00"));
    _driverService.InsertTextOnElement(By.XPath(definitions.Locators.FinalMinutoXPath), definitions.HorarioFinal.Minuto.ToString("00"));
    await _driverService.ClickOnElement(By.XPath(definitions.Locators.GravarDadosXPath));
}

void VerifyTimesheet()
{
    if (string.IsNullOrEmpty(_driverService.GetElementContent(By.XPath(definitions.Locators.RegistroTabelaXPath))))
    {
        LogWriter.Write($"Houve um erro ao marcar ponto no dia {DateTime.Today.ToShortDateString()}.");
        return;
    }

    LogWriter.Write($"Ponto do dia {DateTime.Today.ToShortDateString()} marcado com sucesso.\n" +
        $"Hora Início:{definitions.HorarioInicio.Hora.ToString("00")}:{definitions.HorarioInicio.Minuto.ToString("00")}\n" +
        $"Hora Final:{definitions.HorarioFinal.Hora.ToString("00")}:{definitions.HorarioFinal.Minuto.ToString("00")}");
}