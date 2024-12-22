using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Npgsql;
using System.Globalization;

namespace Crematorium.Persistense.Repository.Postgre;

public class RitualUrnRepository : IRitualUrnRepository
{
    private readonly NpgsqlDataSource _source;

    public RitualUrnRepository(NpgsqlDataSource source)
    {
        _source = source;
    }

    public async Task CreateAsync(RitualUrn urn, CancellationToken cancellationToken = default)
    {
        await using var connection = _source.OpenConnection();

        await using var command = new NpgsqlCommand($"CALL CreateRitualUrn('{urn.Name}', {urn.Price.ToString(CultureInfo.GetCultureInfo("en-US"))}, '{urn.Image}');", connection);

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
        await using var connection = _source.OpenConnection();
        await using var command = new NpgsqlCommand($"CALL DeleteRitualUrnById({id});", connection);

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<IEnumerable<RitualUrn>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var connection = _source.OpenConnection();
        await using var command = new NpgsqlCommand($"SELECT * FROM PartialSearchRitualUrn('{name}')", connection);

        return await GetManyUrns(command);
    }

    public async Task<IEnumerable<RitualUrn>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = _source.OpenConnection();
        await using var command = new NpgsqlCommand("SELECT Id, Name, Price, Image FROM RitualUrn;", connection);

        var res = await GetManyUrns(command);

        connection.Close();

        return res;
    }

    public async Task<RitualUrn?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0)
        {
            return null;
        }

        await using var connection = _source.OpenConnection();
        await using var command = new NpgsqlCommand($"SELECT * FROM RitualUrn WHERE Id={id}", connection);

        using var reader = command.ExecuteReader();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        RitualUrn urn = new();

        urn.Id = (int)reader["Id"];
        urn.Name = (string)reader["Name"];
        urn.Price = (decimal)reader["Price"];
        urn.Image = (string)reader["Image"];

        return urn;
    }

    public async Task UpdateAsync(RitualUrn urn, CancellationToken cancellationToken = default)
    {
        await using var connection = _source.OpenConnection();
        await using var command = new NpgsqlCommand($"CALL UpdateRitualUrn({urn.Id}, '{urn.Name}', {urn.Price.ToString(CultureInfo.GetCultureInfo("en-US"))}, '{urn.Image}');", connection);

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private async Task<IEnumerable<RitualUrn>> GetManyUrns(NpgsqlCommand command)
    {
        var result = new List<RitualUrn>();

        using var reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            var id = (int)reader["Id"];
            var name = (string)reader["Name"];
            var price = (decimal)reader["Price"];
            var image = (string)reader["Image"];

            result.Add(new RitualUrn() { Id = id, Name = name, Price = price, Image = image });
        }

        return result;
    }
}
