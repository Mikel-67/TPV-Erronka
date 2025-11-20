using Erronka1.Modeloak;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MahaiRepository
{
    public async Task<List<Mahaiak>> LortuMahaiakLibreAsync(DateTime data)
    {
        var mahaiaList = new List<Mahaiak>();

        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";

        using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        string sql = @"
            SELECT m.id, m.zenbakia
            FROM mahaiak m
            WHERE m.id NOT IN (
                SELECT r.mahaia_id
                FROM erreserbak r
                WHERE r.data::date = @data::date
            )
            ORDER BY m.zenbakia;
        ";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("data", data);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            mahaiaList.Add(new Mahaiak(
                reader.GetInt32(0),
                reader.GetString(1),
                false
            ));
        }

        return mahaiaList;
    }
}
