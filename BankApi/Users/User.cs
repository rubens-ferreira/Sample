namespace BankApi.Users;

using System.ComponentModel.DataAnnotations.Schema;
using BankApi.Accounts;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public Guid? CurrentAccountID { get; set; }

    public Account CurrentAccount { get; set; }
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
