namespace Models;

public class Owner
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Owner(string name)
    {
        Name = name;
    }
}