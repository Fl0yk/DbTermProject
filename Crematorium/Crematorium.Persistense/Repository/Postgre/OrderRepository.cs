using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Npgsql;

namespace Crematorium.Persistense.Repository.Postgre;

public class OrderRepository : IOrderRepository
{
    private readonly NpgsqlDataSource _source;

    public OrderRepository(NpgsqlDataSource source)
    {
        _source = source;
    }

    public async Task ChangeStatus(int orderId, StateOrder newState, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL UpdateOrderStatus({orderId}, {(int)newState});");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"CALL CreateOrder(date('{order.DateOfStart:yyyy-MM-dd}'), {order.HallId!.Id}, {order.CorposeId.Id}, {order.Customer.Id}, {order.RitualUrnId!.Id}, {(int)order.State});");

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<Order> result = new List<Order>();

        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT Id, DateOfActual, StateCode, " +
            $"CorposeName, CorposeSurname, CoproseNumpassport, " +
            $"HallNumber, HallName, HallCapacity, HallPrice, " +
            $"UrnName, UrnPrice, UrnImage, " +
            $"UserId, Name, Surname, EmailAdress, NumPassport, RoleCode " +
            $"FROM FullOrders; ");

        using var reader = command.ExecuteReader();

        while (await reader.ReadAsync())
        {
            var order = CompileOrder(reader);

            result.Add(order);
        }

        return result;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(User user, CancellationToken cancellationToken = default)
    {
        List<Order> result = new List<Order>();

        //using var connection = _source.OpenConnection();
        await using var command = _source.CreateCommand($"SELECT Id, DateOfActual, StateCode, " +
            $"CorposeName, CorposeSurname, CoproseNumpassport, " +
            $"HallNumber, HallName, HallCapacity, HallPrice, " +
            $"UrnName, UrnPrice, UrnImage, " +
            $"UserId, Name, Surname, EmailAdress, NumPassport, RoleCode " +
            $"FROM FullOrders WHERE UserId = {user.Id}; ");

        using var reader = command.ExecuteReader();

        while (await reader.ReadAsync())
        {
            var order = CompileOrder(reader);

            result.Add(order);
        }

        return result;
    }

    private Order CompileOrder(NpgsqlDataReader reader)
    {
        Order order = new();

        order.Id = (int)reader["Id"];
        order.DateOfStart = DateTime.Parse(reader["DateOfActual"].ToString()!);
        order.State = (StateOrder)reader["StateCode"];

        Corpose corpose = new()
        {
            Name = reader["CorposeName"].ToString() ?? String.Empty,
            SurName = reader["CorposeSurname"].ToString() ?? String.Empty,
            NumPassport = reader["CoproseNumpassport"].ToString() ?? String.Empty
        };

        Hall hall = new();
        if (reader["HallNumber"] is int hallId)
        {
            hall.Id = hallId;
            hall.Name = (string)reader["HallName"];
            hall.Price = (decimal)reader["HallPrice"];
            hall.Capacity = (int)reader["HallCapacity"];
        }

        RitualUrn urn = new();
        if (reader["UrnName"] is string urnName)
        {
            urn.Name = urnName;
            urn.Image = (string)reader["UrnImage"];
            urn.Price = (decimal)reader["UrnPrice"];
        }

        User user = new();

        user.Id = (int)reader["UserId"];
        user.Name = (string)reader["Name"];
        user.Surname = (string)reader["Surname"];
        user.MailAdress = (string)reader["EmailAdress"];
        user.UserRole = (Role)(reader["RoleCode"] != DBNull.Value ? (int)reader["RoleCode"] : 0);
        user.NumPassport = (string)reader["NumPassport"];

        order.Customer = user;
        order.RitualUrnId = urn;
        order.CorposeId = corpose;
        order.HallId = hall;

        return order;
    }
}
