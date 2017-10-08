using System;

namespace AirHockeyServer.Entities
{
    public class UserEntity : Entity
    {
        public int Id { get; set; }
        
        public string Username { get; set; }

        public UserEntity()
        {

        }
    }

    //public class UserComparer : IComparable<UserEntity>
    //{
    //    public int CompareTo(UserEntity other)
    //    {
    //        return other.Id.GetHashCode();
    //    }
    //}
}