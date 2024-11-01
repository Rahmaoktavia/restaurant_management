using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace RM
{
    internal class MysqlConnection
    {
        public ConnectionState State { get; internal set; }

        internal void Close()
        {
            throw new NotImplementedException();
        }

        internal void Open()
        {
            throw new NotImplementedException();
        }

        public static implicit operator MysqlConnection(MySqlConnection v)
        {
            throw new NotImplementedException();
        }
    }
}