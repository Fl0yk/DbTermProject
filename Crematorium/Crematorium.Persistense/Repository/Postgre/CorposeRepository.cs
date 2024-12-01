using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Npgsql;

namespace Crematorium.Persistense.Repository.Postgre;

public class CorposeRepository : ICorposeRepository
{
    private readonly NpgsqlDataSource _source;

    public CorposeRepository(string connectionString)
    {
        _source = NpgsqlDataSource.Create(connectionString);
    }

    public async Task AddAsync(Corpose corpose, CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();

        await using var command = _source.CreateCommand($"CALL CreateCorpse('{corpose.Name}', '{corpose.SurName}', '{corpose.NumPassport}');");
        
        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();

        await using var command = _source.CreateCommand($"CALL DeleteCorpseById({id});");

        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<IEnumerable<Corpose>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var command = _source.CreateCommand($"SELECT Id, Name, Surname, NumPassport FROM Corpose " +
                                                            $"WHERE Name='{name}';");

        return await GetManyCorposes(command);
    }

    public async Task<IEnumerable<Corpose>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT Id, Name, Surname, NumPassport FROM Corpose;");

        return await GetManyCorposes(command);
    }

    public async Task<Corpose?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0)
        {
            return null;
        }

        await using var command = _source.CreateCommand($"SELECT Id, Name, Surname, NumPassport FROM Corpose " +
                                                            $"WHERE Id='{id}';");

        var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        Corpose corpose = new();

        corpose.Id = (int)reader["Id"];
        corpose.Name = (string)reader["Name"];
        corpose.SurName = (string)reader["Surname"];
        corpose.NumPassport = (string)reader["NumPassport"];

        return corpose;
    }

    public async Task UpdateAsync(Corpose corpose, CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL UpdateCorpse({corpose.Id}, '{corpose.Name}', '{corpose.SurName}', '{corpose.NumPassport}');");


        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private async Task<IEnumerable<Corpose>> GetManyCorposes(NpgsqlCommand command)
    {
        List<Corpose> res = new();

        var reader = command.ExecuteReader();

        while (await reader.ReadAsync())
        {
            Corpose corpose = new Corpose();

            corpose.Id = (int)reader["Id"];
            corpose.Name = (string)reader["Name"];
            corpose.SurName = (string)reader["Surname"];
            corpose.NumPassport = (string)reader["NumPassport"];

            res.Add(corpose);
        }

        return res;
    }
}
