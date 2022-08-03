namespace Services;

public interface IFillDataService
{
    Task<bool> FillDb();
}