namespace laminas_calisa.Models;

public class DashboardOrders
{
    public int Id { get; set; }
    public string Cliente { get; set; }
    public string? Alias { get; set; }
    public string Tipo { get; set; }
    public string Acabado { get; set; }
    public int Calibre { get; set; }
    public string Largo { get; set; }
    public int Cantidad { get; set; }
    public string Peso { get; set; }
    public string? Descripcion { get; set; }
    public string Terminado { get; set; }
    public string Pagado { get; set; }
    public string Usuario { get; set; }
}