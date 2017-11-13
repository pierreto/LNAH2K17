using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "test_maps")]
    public class MapPoco
    {
        [Column(Name = "id", DbType = "int(11) NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public int? Id { get; set; }

        [Column(Name = "creator", DbType = "varchar(128) NOT NULL", CanBeNull = false)]
        public string Creator;

        [Column(Name = "name", DbType = "varchar(128) NOT NULL", CanBeNull = false)]
        public string Name;

        [Column(Name = "creationDate", DbType = "datetime NOT NULL", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public DateTime CreationDate;

        [Column(Name = "json", DbType = "text NOT NULL", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public string Json;

        [Column(Name = "private", DbType = "tinyint(1)", CanBeNull = false)]
        public bool Private;

        [Column(Name = "password", DbType = "varchar(255)", CanBeNull = true)]
        public string Password;

        [Column(Name = "lastBackup", DbType = "datetime NOT NULL", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public DateTime LastBackup;
    }
}