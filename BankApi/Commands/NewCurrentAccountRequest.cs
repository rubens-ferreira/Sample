namespace BankApi.Commands;

public class NewCurrentAccountRequest
{
    public int UserID { get; set; }
    public int InitialCredit { get; set; }
}
