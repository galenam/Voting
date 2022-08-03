namespace Models;
public class Settings
{
    public string FilePathXslx { get; set; }
    public string FilePathPdf { get; set; }
    public IEnumerable<PagePdf> PagesPdf { get; set; }
}