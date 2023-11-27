using System.Text;
using Bogus;
using MassTransit;
using Medo;
using Npgsql;

namespace GuidVSUuid7;

public interface INpgSqlConnectionDatabase
{
    void EnsureDatabase();
    void EnsureTable();
    void InsertDataGuid(int rows);
    void InsertDataGuidUUID7(int rows);
    void InsertDataGuidLong(int rows);
    void InsertDataNewId(int rows);
}

public class NpgSqlConnectionDatabase : INpgSqlConnectionDatabase
{
    private readonly string _connectionString;
    private readonly NpgsqlDataSource _dataSource;
    private readonly Faker _faker;
    public NpgSqlConnectionDatabase(string connectionString)
    {
        _connectionString = connectionString;
        _dataSource = NpgsqlDataSource.Create(connectionString);
        _faker = new Faker();
    }

    public void EnsureDatabase()
    {
        NpgsqlConnectionStringBuilder connBuilder = new()
        {
            ConnectionString = _connectionString
        };

        string dbName = connBuilder.Database;

        var masterConnection = _connectionString.Replace(dbName, "postgres");

        using NpgsqlConnection connection = new(masterConnection);
        connection.Open();
        using var checkIfExistsCommand = new NpgsqlCommand($"SELECT 1 FROM pg_catalog.pg_database WHERE datname = '{dbName}'", connection);
        var result = checkIfExistsCommand.ExecuteScalar();

        if (result == null)
        {
            using var command = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", connection);
            command.ExecuteNonQuery();
        }
    }

    public void EnsureTable()
    {
        var sql = @"CREATE TABLE IF NOT EXISTS MyTable (
id uuid not NULL UNIQUE PRIMARY KEY,
created_at timestamp,
meta text not null
)";
        var command = _dataSource.CreateCommand(sql);
        command.ExecuteNonQuery();

        var sql1 = @"CREATE TABLE IF NOT EXISTS MyTableNew (
id uuid not NULL UNIQUE PRIMARY KEY,
created_at timestamp,
meta text not null
)";
        var command1 = _dataSource.CreateCommand(sql1);
        command1.ExecuteNonQuery();

        var sql2 = @"CREATE TABLE IF NOT EXISTS MyTableLong (
id bigint not NULL UNIQUE PRIMARY KEY,
created_at timestamp,
meta text not null
)";
        var command2 = _dataSource.CreateCommand(sql2);
        command2.ExecuteNonQuery();

        var sql3 = @"CREATE TABLE IF NOT EXISTS MyTableNewId (
id uuid not NULL UNIQUE PRIMARY KEY,
created_at timestamp,
meta text not null
)";
        var command3 = _dataSource.CreateCommand(sql3);
        command3.ExecuteNonQuery();
    }

    public void InsertDataGuid(int rows)
    {
        var sb = new StringBuilder();
        for (int i = 1; i <= rows; i++)
        {
            sb.AppendFormat("((@id{0}), (@created_at{0}), (@meta{0})),", i);
        }
        var s = sb.ToString();
        s = s.Remove(s.Length - 1);
        var sql = string.Concat("INSERT INTO MyTable (id, created_at, meta) VALUES ", s);
        var command = _dataSource.CreateCommand(sql);
        for (int i = 1; i <= rows; i++)
        {
            command.Parameters.Add(new NpgsqlParameter<Guid>($"id{i}", Guid.NewGuid()));
            command.Parameters.Add(new NpgsqlParameter<DateTime>($"created_at{i}", DateTime.Now));
            command.Parameters.Add(new NpgsqlParameter<string>($"meta{i}", _faker.Lorem.Paragraphs(2)));
        }

        command.ExecuteNonQuery();
    }

    public void InsertDataGuidUUID7(int rows)
    {
        var sb = new StringBuilder();
        for (int i = 1; i <= rows; i++)
        {
            sb.AppendFormat("((@id{0}), (@created_at{0}), (@meta{0})),", i);
        }
        var s = sb.ToString();
        s = s.Remove(s.Length - 1);
        var sql = string.Concat("INSERT INTO MyTableNew (id, created_at, meta) VALUES ", s);
        var command = _dataSource.CreateCommand(sql);
        for (int i = 1; i <= rows; i++)
        {
            command.Parameters.Add(new NpgsqlParameter<Guid>($"id{i}", Uuid7.NewGuid()));
            command.Parameters.Add(new NpgsqlParameter<DateTime>($"created_at{i}", DateTime.Now));
            command.Parameters.Add(new NpgsqlParameter<string>($"meta{i}", _faker.Lorem.Paragraphs(2)));
        }

        command.ExecuteNonQuery();
    }

    public void InsertDataGuidLong(int rows)
    {
        var sb = new StringBuilder();
        for (int i = 1; i <= rows; i++)
        {
            sb.AppendFormat("((@id{0}), (@created_at{0}), (@meta{0})),", i);
        }
        var s = sb.ToString();
        s = s.Remove(s.Length - 1);
        var sql = string.Concat("INSERT INTO MyTableLong (id, created_at, meta) VALUES ", s);
        var command = _dataSource.CreateCommand(sql);
        for (int i = 1; i <= rows; i++)
        {
            command.Parameters.Add(new NpgsqlParameter<long>($"id{i}", Sequence.Next()));
            command.Parameters.Add(new NpgsqlParameter<DateTime>($"created_at{i}", DateTime.Now));
            command.Parameters.Add(new NpgsqlParameter<string>($"meta{i}", _faker.Lorem.Paragraphs(2)));
        }

        command.ExecuteNonQuery();
    }

    public void InsertDataNewId(int rows)
    {
        var sb = new StringBuilder();
        for (int i = 1; i <= rows; i++)
        {
            sb.AppendFormat("((@id{0}), (@created_at{0}), (@meta{0})),", i);
        }
        var s = sb.ToString();
        s = s.Remove(s.Length - 1);
        var sql = string.Concat("INSERT INTO MyTableNewId (id, created_at, meta) VALUES ", s);
        var command = _dataSource.CreateCommand(sql);
        for (int i = 1; i <= rows; i++)
        {
            command.Parameters.Add(new NpgsqlParameter<Guid>($"id{i}", NewId.NextSequentialGuid()));
            command.Parameters.Add(new NpgsqlParameter<DateTime>($"created_at{i}", DateTime.Now));
            command.Parameters.Add(new NpgsqlParameter<string>($"meta{i}", _faker.Lorem.Paragraphs(2)));
        }

        command.ExecuteNonQuery();
    }
}