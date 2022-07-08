namespace Models;
public class Settings{
    public string FilePath { get; set; }

    public Settings(string filePath)
    {
        FilePath = filePath;
    }
}