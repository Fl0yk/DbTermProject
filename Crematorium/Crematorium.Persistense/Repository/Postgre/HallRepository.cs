using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Npgsql;
using System.Globalization;

namespace Crematorium.Persistense.Repository.Postgre;

public class HallRepository : IHallRepository
{
    private readonly NpgsqlDataSource _source;

    public HallRepository(NpgsqlDataSource source)
    {
        _source = source;
    }

    public async Task AddAsync(Hall hall, CancellationToken cancellationToken = default)
    {
        await using var command = _source.CreateCommand($"CALL CreateHall('{hall.Name}', '{hall.Capacity}', " +
                                            $"'{hall.Price.ToString(CultureInfo.GetCultureInfo("en-US"))}');");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task AddFreeDate(int id, DateTime date, CancellationToken cancellationToken = default)
    {
        await using var command = _source.CreateCommand($"INSERT INTO Dates (HallId, Date) VALUES ({id}, date('{date:yyyy-MM-dd}'));");

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
        await using var command = _source.CreateCommand($"CALL DeleteHallById('{id}');");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<IEnumerable<Hall>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var command = _source.CreateCommand($"SELECT * FROM PartialSearchHall('{name}')");

        return await GetManyWithDates(command);
    }

    public async Task<IEnumerable<Hall>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var command = _source.CreateCommand($"SELECT h.Id, h.Name, h.Capacity, h.Price, d.date " +
                                                        $"FROM Hall AS h " +
                                                        $"LEFT OUTER JOIN Dates AS d ON h.Id=d.HallId " +
                                                        $"ORDER BY h.Id;");

        return await GetManyWithDates(command);
    }

    public async Task<Hall?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0)
        {
            return null;
        }

        await using var command = _source.CreateCommand($"SELECT h.Id, h.Name, h.Capacity, h.Price, d.date " +
                                                        $"FROM Hall AS h " +
                                                        $"LEFT OUTER JOIN Dates AS d ON h.Id=d.HallId " +
                                                         $"WHERE h.Id = {id} " +
                                                        $"ORDER BY h.Id;");

        using var reader = command.ExecuteReader();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        string name = (string)reader["Name"];
        var capacity = (int)reader["Capacity"];
        var price = (decimal)reader["Price"];

        Hall res = new() { Id = id, Name = name, Capacity = capacity, Price = price, FreeDates = new() };

        do
        {
            var tmp = reader["date"];

            if (tmp is DBNull)
            {
                continue;
            }

            var date = DateTime.Parse((string)tmp);

            res.FreeDates.Add(new() { Data = date.ToString(), Id = id });
        } while (await reader.ReadAsync());

        return res;
    }

    public async Task UpdateAsync(Hall hall, CancellationToken cancellationToken = default)
    {
        await using var command = _source.CreateCommand($"CALL UpdateHall('{hall.Id}', '{hall.Capacity}', " +
                                            $"'{hall.Price.ToString(CultureInfo.GetCultureInfo("en-US"))}');");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private async Task<IEnumerable<Hall>> GetManyWithDates(NpgsqlCommand command)
    {
        List<Hall> res = new();

        using var reader = command.ExecuteReader();

        int i = -1;

        while (await reader.ReadAsync())
        {
            int id = (int)reader["Id"];

            if (id == i)
            {
                var tmp = reader["date"];
                if (tmp is DBNull)
                {
                    continue;
                }

                var date = DateTime.Parse((string)tmp);

                res.Last().FreeDates.Add(new() { Data = date.ToString(), Id = id });
            }
            else
            {
                string name = (string)reader["Name"];
                var capacity = (int)reader["Capacity"];
                var price = (decimal)reader["Price"];

                res.Add(new Hall() { Id = id, Name = name, Capacity = capacity, Price = price, FreeDates = new() });

                var tmp = reader["date"];
                if (tmp is DBNull)
                {
                    continue;
                }

                var date = DateTime.Parse((string)tmp);

                res.Last().FreeDates.Add(new() { Data = date.ToString(), Id = id });

                i = id;
            }
        }

        return res;
    }
}
