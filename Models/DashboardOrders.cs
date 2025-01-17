namespace laminas_calisa.Models;

public class DashboardOrders
{
    public int Id { get; set; }
    public required string Cliente { get; set; }
    public string? Alias { get; set; }
    public required string Tipo { get; set; }
    public required string Acabado { get; set; }
    public int Calibre { get; set; }
    public required string Largo { get; set; }
    public int Cantidad { get; set; }
    public required string Peso { get; set; }
    public string? Descripcion { get; set; }
    public required string Terminado { get; set; }
    public required string Pagado { get; set; }
    public decimal Precio { get; set; }
    public required string TipoDePago { get; set; }
    public required string Usuario { get; set; }
}