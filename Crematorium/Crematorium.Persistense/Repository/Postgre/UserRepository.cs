using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Npgsql;

namespace Crematorium.Persistense.Repository.Postgre;

public class UserRepository : IUserRepository
{
    private readonly NpgsqlDataSource _source;

    public UserRepository(NpgsqlDataSource source)
    {
        _source = source;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL CreateUser('{user.Name}', '{user.Surname}', '{user.MailAdress}', {(int)user.UserRole}, '{user.NumPassport}');");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL DeleteUserById({id});");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<IEnumerable<User>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT * FROM Users WHERE Name = '{name}';");

        return await GetManyUsers(command);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand("SELECT * FROM Users;");

        return await GetManyUsers(command);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0)
        {
            return null;
        }

        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT * FROM Users WHERE Id = {id};");

        using var reader = command.ExecuteReader();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        User user = new();

        user.Id = (int)reader["Id"];
        user.Name = (string)reader["Name"];
        user.Surname = (string)reader["Surname"];
        user.MailAdress = (string)reader["EmailAdress"];
        user.UserRole = (Role)(reader["RoleCode"] != DBNull.Value ? (int)reader["RoleCode"] : 0);
        user.NumPassport = (string)reader["NumPassport"];

        return user;
    }

    public async Task<User?> GetUserByNameAndPassport(string name, string numPassport, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT * FROM Users WHERE Name = '{name}' AND NumPassport = '{numPassport}';");

        using var reader = command.ExecuteReader();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        User user = new();

        user.Id = (int)reader["Id"];
        user.Name = (string)reader["Name"];
        user.Surname = (string)reader["Surname"];
        user.MailAdress = (string)reader["EmailAdress"];
        user.UserRole = (Role)(reader["RoleCode"] != DBNull.Value ? (int)reader["RoleCode"] : 0);
        user.NumPassport = (string)reader["NumPassport"];

        return user;
    }

    public async Task<bool> IsExist(string numPassport, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();

        await using var command = _source.CreateCommand($"SELECT COUNT(1) FROM Users WHERE NumPassport='{numPassport}';");

        var res = command.ExecuteScalar();

        return (long)res > 0;
    }

    public async Task<bool> IsExist(string name, string numPassport, CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();

        await using var command = _source.CreateCommand($"SELECT * FROM IsExistUser('{name}', '{numPassport}');");

        var res = command.ExecuteScalar();

        return (bool)res;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL UpdateUser({user.Id}, '{user.Name}', '{user.Surname}', '{user.MailAdress}', {(int)user.UserRole});");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private async Task<IEnumerable<User>> GetManyUsers(NpgsqlCommand command)
    {
        List<User> result = new List<User>();

        using var reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            var id = (int)reader["Id"];
            var name = (string)reader["Name"];
            var surname = (string)reader["Surname"];
            var emailAddress = (string)reader["EmailAdress"];
            var roleCode = reader["RoleCode"] != DBNull.Value ? (int)reader["RoleCode"] : 0;
            var numPassport = (string)reader["NumPassport"];

            result.Add(new User() { Id = id, Name = name, Surname = surname, MailAdress = emailAddress, UserRole = (Role)roleCode, NumPassport = numPassport });
        }

        return result;
    }
}
