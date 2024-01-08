namespace BankApi.Transactions;

using System.ComponentModel.DataAnnotations.Schema;
using BankApi.Accounts;

public class Transaction
{
    public Guid TransactionID { get; set; }
    public Guid AccountID { get; set; }
    public int Amount { get; set; }

    public Account Account { get; set; }
}
