using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Npgsql;
using System.Globalization;

namespace Crematorium.Persistense.Repository.Postgre;

public class RitualUrnRepository : IRitualUrnRepository
{
    private readonly NpgsqlDataSource _source;

    public RitualUrnRepository(string connectionString)
    {
        _source = NpgsqlDataSource.Create(connectionString);
    }

    public async Task CreateAsync(RitualUrn urn, CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL CreateRitualUrn('{urn.Name}', {urn.Price.ToString(CultureInfo.GetCultureInfo("en-US"))}, '{urn.Image}');");

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
        _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL DeleteRitualUrnById({id});");

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
        _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT * FROM PartialSearchRitualUrn('{name}')");

        return await GetManyUrns(command);
    }

    public async Task<IEnumerable<RitualUrn>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();
        await using var command = _source.CreateCommand("SELECT Id, Name, Price, Image FROM RitualUrn;");

        return await GetManyUrns(command);
    }

    public async Task<RitualUrn?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0)
        {
            return null;
        }

        _source.OpenConnection();

        await using var command = _source.CreateCommand($"SELECT * FROM RitualUrn WHERE Id={id}");

        var reader = command.ExecuteReader();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        RitualUrn urn = new();

        urn.Id = (int)reader["Id"];
        urn.Name = (string)reader["Name"];
        urn.Price = (decimal)reader["Price"];
        urn.Image = (string)reader["Image"];//StringToBytes((string)reader["Image"]);

        return urn;
    }

    public async Task UpdateAsync(RitualUrn urn, CancellationToken cancellationToken = default)
    {
        _source.OpenConnection();

        await using var command = _source.CreateCommand($"CALL UpdateRitualUrn({urn.Id}, '{urn.Name}', {urn.Price.ToString(CultureInfo.GetCultureInfo("en-US"))}, '{urn.Image}');");

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

        var reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            var id = (int)reader["Id"];
            var name = (string)reader["Name"];
            var price = (decimal)reader["Price"];
            //var image = new byte[0]; //StringToBytes((string)reader["Image"]);
            var image = (string)reader["Image"];

            result.Add(new RitualUrn() { Id = id, Name = name, Price = price, Image = image });
        }

        return result;
    }

    private static string BytesToString(byte[] data)
    {
        return string.Join(',', data);
    }

    private static byte[] StringToBytes(string data)
    {
        return data.Split(',').Select(byte.Parse).ToArray();
    }
}
