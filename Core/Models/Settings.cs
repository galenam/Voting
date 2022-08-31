namespace Models;
public class Settings
{
    public string FilePathXslx { get; set; }
    public string FilePathPdf { get; set; }
    public string DirectoryPathImage { get; set; }
    public IEnumerable<Coordinate> Coordinates { get; set; }
}

public class Coordinate
{
    public int X1 { get; set; }
    public int Y1 { get; set; }
    public int X2 { get; set; }
    public int Y2 { get; set; }
    public override string ToString()
    {
        return $"X1={X1} Y1={Y1} X2={X2} Y2={Y2}";
    }
}