using Crematorium.Domain.Abstractions.Loggers;
using Crematorium.Domain.Enums;
using Npgsql;

namespace Crematorium.Persistense.Loggers;

public class UserAuthLogger : IUserAuthLogger
{
    private readonly NpgsqlDataSource _source;

    public UserAuthLogger(NpgsqlDataSource source)
    {
        _source = source;
    }

    public void Log(string numPassport, LogAction action)
    {
        using var command = _source.CreateCommand("SELECT LogUserAction(@PassportNumber, @ActionType)");
        command.Parameters.AddWithValue("PassportNumber", numPassport);
        command.Parameters.AddWithValue("ActionType", action.ToString());

        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
