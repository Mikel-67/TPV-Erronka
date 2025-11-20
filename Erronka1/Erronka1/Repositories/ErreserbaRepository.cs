using Npgsql;
using Erronka1.Modeloak;
using System;
using System.Threading.Tasks;

namespace Erronka1.Repositories
{
    public class ErreserbaRepository
    {
        private readonly string connectionString;

        public ErreserbaRepository(string conn)
        {
            connectionString = conn;
        }

        public async Task GordeErreserbaAsync(Erreserba erreserba)
        {
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            string sql = @"
                INSERT INTO erreserbak (mahaia_id, data)
                VALUES (@mahaiaId, @data);
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("mahaiaId", erreserba.Mahaia.Id);
            cmd.Parameters.AddWithValue("data", erreserba.Data);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
