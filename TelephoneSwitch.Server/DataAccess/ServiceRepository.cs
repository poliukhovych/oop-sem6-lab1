
namespace TelephoneSwitch.Server.DataAccess
{
    public class ServiceRepository
    {
        private readonly Database _db;

        public ServiceRepository(Database db)
        {
            _db = db;
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            var services = new List<Service>();

            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name, price FROM services;", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                services.Add(new Service
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }

            return services;
        }

        public async Task<decimal> GetServicePriceAsync(int serviceId)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT price FROM services WHERE id = @ServiceId", conn);
            cmd.Parameters.AddWithValue("@ServiceId", serviceId);
            object result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public async Task<List<Service>> GetSubscriberServicesAsync(int subscriberId)
        {
            var services = new List<Service>();

            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand(@"
                                SELECT s.id, s.name, s.price 
                                FROM services s
                                INNER JOIN subscriber_services ss ON s.id = ss.service_id
                                WHERE ss.subscriber_id = @subscriberId;", conn);
            cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                services.Add(new Service
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }

            return services;
        }

        public async Task AssignServiceToSubscriberAsync(int subscriberId, int serviceId)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO subscriber_services (subscriber_id, service_id) VALUES (@subscriberId, @serviceId);", conn);
            cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
            cmd.Parameters.AddWithValue("@serviceId", serviceId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> AddServiceAsync(Service service)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO services (name, price) VALUES (@name, @price);", conn);
            cmd.Parameters.AddWithValue("@name", service.Name);
            cmd.Parameters.AddWithValue("@price", service.Price);
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
