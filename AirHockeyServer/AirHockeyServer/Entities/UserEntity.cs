using System;

namespace AirHockeyServer.Entities
{
    public class UserEntity : Entity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }

    //public class UserComparer : IComparable<UserEntity>
    //{
    //    public int CompareTo(UserEntity other)
    //    {
    //        return other.Id.GetHashCode();
    //    }
    //}
}