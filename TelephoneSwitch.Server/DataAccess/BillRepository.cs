
namespace TelephoneSwitch.Server.DataAccess
{
    public class BillRepository
    {
        private readonly Database _db;

        public BillRepository(Database db)
        {
            _db = db;
        }

        public async Task<List<Bill>> GetUnpaidBillsAsync()
        {
            var bills = new List<Bill>();

            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, subscriber_id, amount, is_paid FROM bills WHERE is_paid = FALSE;", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                bills.Add(new Bill
                {
                    Id = reader.GetInt32(0),
                    SubscriberId = reader.GetInt32(1),
                    Amount = reader.GetDecimal(2),
                    IsPaid = reader.GetBoolean(3)
                });
            }

            return bills;
        }

        public async Task<List<Bill>> GetSubscriberBillsAsync(int subscriberId)
        {
            var bills = new List<Bill>();

            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand(@"
                                SELECT b.id, b.subscriber_id, b.amount, b.is_paid 
                                FROM bills b
                                WHERE b.subscriber_id = @subscriberId;", conn);
            cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                bills.Add(new Bill
                {
                    Id = reader.GetInt32(0),
                    SubscriberId = reader.GetInt32(1),
                    Amount = reader.GetDecimal(2),
                    IsPaid = reader.GetBoolean(3)
                });
            }

            return bills;
        }

        public async Task<int> AddBillAsync(int subscriberId, decimal amount)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("INSERT INTO bills (subscriber_id, amount, is_paid) VALUES (@subscriberId, @amount, @isPaid);", conn);
            cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@isPaid", false);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> PayBillAsync(int billId)
        {
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("UPDATE bills SET is_paid = TRUE WHERE id = @billId;", conn);
            cmd.Parameters.AddWithValue("@billId", billId);
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
