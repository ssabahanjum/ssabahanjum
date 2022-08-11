﻿
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Hotel;

public class DapperContext
{

    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("UnipassSqlServerConnection");
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}

