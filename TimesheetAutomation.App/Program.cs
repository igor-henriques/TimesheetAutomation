DriverUtils.CleanDriverGarbage();

var definitions = await Definitions.CreateAsync();

IDriverService _driverService = new DriverService();

await LoginAsync();

await MarkTimesheet();

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

async Task VerifyTimesheet()
{

}