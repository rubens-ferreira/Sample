namespace BankApi.Commands;

public enum CommandStatus
{
    Created,
    Running,
    Executed,
    Compensating,
    Failed
}
