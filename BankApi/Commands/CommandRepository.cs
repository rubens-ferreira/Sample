namespace BankApi.Commands;

public interface ICommandRepository
{
    public Task<Command> AddCommandAsync(Command command);
    public Task UpdateCommandAsync(Command command);
}

public class CommandRepository(BankDb db) : ICommandRepository
{
    private readonly BankDb _db = db;

    public async Task<Command> AddCommandAsync(Command command)
    {
        _db.Commands.Add(command);
        await _db.SaveChangesAsync();
        return command;
    }

    public async Task UpdateCommandAsync(Command command)
    {
        var theCommand =
            await _db.Commands.FindAsync(command.CommandID)
            ?? throw new Exception("Invalid Command Parameter");
        theCommand.CommandStatus = command.CommandStatus;
        theCommand.CommandType = command.CommandType;
        theCommand.FailureMessage = command.FailureMessage;
        theCommand.Json = command.Json;
        await _db.SaveChangesAsync();
    }
}
