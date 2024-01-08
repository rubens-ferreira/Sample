using System.Text.Json;

namespace BankApi.Commands;

public class NewCurrentAccountCommand : Command
{
    public NewCurrentAccountCommand() { }

    public NewCurrentAccountCommand(Command command)
        : base(command)
    {
        if (command.CommandType == CommandType.NewCurrentAccount)
        {
            var data = JsonSerializer.Deserialize<NewCurrentAccountCommand>(command.Json);
            UserID = data.UserID;
            AccountID = data.AccountID;
            TransactionID = data.TransactionID;
            InitialCredit = data.InitialCredit;
        }
    }

    public int UserID { get; set; }
    public Guid? AccountID { get; set; }
    public Guid? TransactionID { get; set; }
    public int InitialCredit { get; set; }
}
