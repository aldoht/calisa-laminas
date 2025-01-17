namespace laminas_calisa.Models;

public class DashboardOrdersDetailed
{
    public int Id { get; set; }
    public required string Cliente { get; set; }
    public string? Alias { get; set; }
    public string? Notas { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public required string Tipo { get; set; }
    public required string Acabado { get; set; }
    public int Calibre { get; set; }
    public required string Largo { get; set; }
    public int Cantidad { get; set; }
    public required string Peso { get; set; }
    public string? Descripcion { get; set; }
    public required string Terminado { get; set; }
    public string? FechaTerminado { get; set; }
    public required string Pagado { get; set; }
    public string? FechaPagado { get; set; }
    public required string TipoDePago { get; set; }
    public required string Usuario { get; set; }
}