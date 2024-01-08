using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BankApi.Commands;

public class Command
{
    [Key]
    public Guid CommandID { get; set; }

    [Required]
    public CommandType CommandType { get; set; }

    [Required]
    public CommandStatus CommandStatus { get; set; }

    public string FailureMessage { get; set; }

    [Required]
    public string Json { get; set; }

    public Command() { }

    public Command(Command other)
    {
        CommandID = other.CommandID;
        CommandType = other.CommandType;
        CommandStatus = other.CommandStatus;
        Json = other.Json;
    }
}
