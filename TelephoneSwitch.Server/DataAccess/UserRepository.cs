
namespace TelephoneSwitch.Server.DataAccess
{
    public class UserRepository
    {
        private readonly Database _db;

        public UserRepository(Database db)
        {
            _db = db;
        }

        public async Task<List<User>> GetSubscribersAsync()
        {
            var subscribers = new List<User>();

            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name, role, phone_number, is_blocked, auth0_id FROM users;", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                subscribers.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    IsBlocked = reader.GetBoolean(4),
                    Auth0Id = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return subscribers;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using (var cmd = new NpgsqlCommand("SELECT id, name, role, phone_number, is_blocked, auth0_id FROM users WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Role = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            IsBlocked = reader.GetBoolean(4),
                            Auth0Id = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<User?> GetUserByAuth0IdAsync(string auth0Id)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name, role, phone_number, is_blocked, auth0_id FROM users WHERE auth0_id = @auth0Id", conn);
            cmd.Parameters.AddWithValue("@auth0Id", auth0Id);
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    IsBlocked = reader.GetBoolean(4),
                    Auth0Id = reader.GetString(5)
                };
            }

            return null;
        }

        public async Task<User> CreateUserAsync(string auth0Id, string name)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand(
                "INSERT INTO users (auth0_id, name, role, phone_number, is_blocked) VALUES (@auth0Id, @name, @role, 19999, FALSE) RETURNING id, name, role, phone_number, is_blocked, auth0_id;",
                conn);

            cmd.Parameters.AddWithValue("@auth0Id", auth0Id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@role", "subscriber");

            using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();

            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Role = reader.GetString(2),
                PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                IsBlocked = reader.GetBoolean(4),
                Auth0Id = reader.GetString(5)
            };
        }

        public async Task<int> AddSubscriberAsync(string name, string phoneNumber)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO users (name, role, phone_number, is_blocked) VALUES (@name, @role, @phoneNumber, @isBlocked);", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@role", "subscriber");
            cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@isBlocked", false);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateUserBlockStatusAsync(int id, bool isBlocked)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE users SET is_blocked = @blocked WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@blocked", isBlocked);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
