using Npgsql;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace вход_курсач
{
    // Параметры подключения к PostgreSQL. Хранятся в файле config.json рядом с программой.
    public class DbSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "tppo";
        public string Username { get; set; } = "postgres";
        public string Password { get; set; } = "";

        private static readonly string FilePath =
            Path.Combine(AppContext.BaseDirectory, "config.json");

        [JsonIgnore]
        public string ConnectionString => new NpgsqlConnectionStringBuilder
        {
            Host = Host,
            Port = Port,
            Database = Database,
            Username = Username,
            Password = Password
        }.ConnectionString;

        // Возвращает null, если файла настроек ещё нет или он повреждён
        public static DbSettings? Load()
        {
            if (!File.Exists(FilePath))
                return null;

            try
            {
                return JsonSerializer.Deserialize<DbSettings>(File.ReadAllText(FilePath));
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(this, options));
        }
    }
}
