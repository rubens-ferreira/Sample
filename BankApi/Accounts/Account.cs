namespace BankApi.Accounts;

using System.ComponentModel.DataAnnotations.Schema;
using BankApi.Transactions;
using BankApi.Users;

public class Account
{
    public Guid AccountID { get; set; }
    public int UserID { get; set; }
    public int Balance { get; set; }

    public User User { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
