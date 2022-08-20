using ExcelDataReader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
namespace Services;
public class GetDataFromSourceService : IGetDataFromSourceService
{
    private ILogger<GetDataFromSourceService> _logger;
    private Settings _settings;
    public GetDataFromSourceService(ILogger<GetDataFromSourceService> logger, IOptions<Settings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task<IEnumerable<OwnerVoting>> Get()
    {
        var owners = new List<OwnerVoting>();
        try
        {
            using (var stream = File.Open($"{Directory.GetCurrentDirectory()}{_settings.FilePathXslx}", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration { FallbackEncoding = System.Text.Encoding.ASCII }))
                {
                    var initial = new OwnerVoting();
                    do
                    {
                        reader.Read();
                        reader.Read();
                        while (reader.Read())
                        {
                            var current = new OwnerVoting();

                            var votingHandler = new VotingHandler(null, 4, 6);
                            var ownerNameHandler = new OwnerNameHandler(votingHandler, 3);
                            var flatHandler = new FlatHandler(ownerNameHandler, 2);
                            flatHandler.Handle(reader, initial, current);
                        }
                    } while (reader.NextResult());

                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Try to process data");
        }
        return owners;
    }
}

public interface IHandler
{
    void Handle(IExcelDataReader reader, OwnerVoting initial, OwnerVoting current);
}

public abstract class BaseHandler : IHandler
{
    protected int _index;
    protected IHandler? _nextHandler;

    protected BaseHandler(IHandler? handler, int index)
    {
        _nextHandler = handler;
        _index = index;
    }

    public void Handle(IExcelDataReader reader, OwnerVoting initial, OwnerVoting current)
    {
        if (reader[_index] != null)
        {
            ReaderIsNotNull(reader, initial, current);
        }
        else
        {
            ReaderIsNull(initial, current);
        }
        if (_nextHandler != null)
        {
            _nextHandler.Handle(reader, initial, current);
        }
    }

    public abstract void ReaderIsNotNull(IExcelDataReader reader, OwnerVoting initial, OwnerVoting current);
    public abstract void ReaderIsNull(OwnerVoting initial, OwnerVoting current);
}

public class FlatHandler : BaseHandler
{
    public FlatHandler(IHandler handler, int index) : base(handler, index)
    {
    }

    public override void ReaderIsNotNull(IExcelDataReader reader, OwnerVoting initial, OwnerVoting current)
    {
        var flatId = (int)reader.GetDouble(_index);
        initial.FlatId = current.FlatId = flatId;
    }

    public override void ReaderIsNull(OwnerVoting initial, OwnerVoting current)
    {
        if (initial.FlatId > 0)
        {
            current.FlatId = initial.FlatId;
        }
    }
}

public class OwnerNameHandler : BaseHandler
{
    public OwnerNameHandler(IHandler handler, int index) : base(handler, index)
    {
    }

    public override void ReaderIsNotNull(IExcelDataReader reader, OwnerVoting initial, OwnerVoting current)
    {
        var surname = reader.GetString(_index);
        current.OwnerName = initial.OwnerName = surname;
    }

    public override void ReaderIsNull(OwnerVoting initial, OwnerVoting current)
    {
        if (!string.IsNullOrEmpty(initial.OwnerName))
        {
            current.OwnerName = initial.OwnerName;
        }
    }
}

public class VotingHandler : BaseHandler
{
    private int _end;
    public VotingHandler(IHandler? handler, int index, int end) : base(handler, index)
    {
        _end = end;
    }

    public override void ReaderIsNotNull(IExcelDataReader reader, OwnerVoting initial, OwnerVoting current)
    {
        var found = 0;
        for (int i = _index; i <= _end; i++)
        {
            if (reader[i] != null)
            {
                found = i - 3;
                break;
            }
        }
        if (found > 0)
        {
            current.Vote = initial.Vote = (VoteType)found;
        }
    }

    public override void ReaderIsNull(OwnerVoting initial, OwnerVoting current)
    {
        current.Vote = initial.Vote;
    }
}