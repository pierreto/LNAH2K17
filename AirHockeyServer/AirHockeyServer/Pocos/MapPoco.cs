using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "test_maps")]
    public class MapPoco : Poco
    {
        private string _Creator;
        private string _Name;
        private DateTime _CreationDate;
        private string _Json;
        private bool _Private;
        private string _Password;

        public override int? Id { get; set; }

        [Column(Name="creator", DbType="varchar(128) NOT NULL", IsPrimaryKey=true, CanBeNull=false, Storage="_Creator")]
        public string Creator
        {
            get
            {
                return this._Creator;
            }
            set
            {
                this._Creator = value;
            }
        }

        [Column(Name="name", DbType="varchar(128) NOT NULL", IsPrimaryKey=true, CanBeNull=false, Storage="_Name")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        [Column(Name="creationDate", DbType="date NOT NULL", CanBeNull=false, Storage="_CreationDate")]
        public DateTime CreationDate
        {
            get
            {
                return this._CreationDate;
            }
            set
            {
                this._CreationDate = value;
            }
        }

        [Column(Name="json", DbType="varchar(4000) NOT NULL", CanBeNull=false, Storage="_Json", UpdateCheck=UpdateCheck.Always)]
        public string Json
        {
            get
            {
                return this._Json;
            }
            set
            {
                this._Json = value;
            }
        }

        [Column(Name="private", DbType="tinyint(1)", CanBeNull=false, Storage="_Private")]
        public bool Private
        {
            get
            {
                return this._Private;
            }
            set
            {
                this._Private = value;
            }
        }

        [Column(Name="password", DbType="varchar(255)", CanBeNull=true, Storage="_Password")]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }
    }
}