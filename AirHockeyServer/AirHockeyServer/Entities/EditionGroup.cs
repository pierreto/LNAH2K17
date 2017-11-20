using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHockeyServer.Entities.EditionCommand;

namespace AirHockeyServer.Entities
{
    public class EditionGroup
    {
        public static readonly string[] Colors = { "007bc2", "c2009e", "c25700", "6bc200" };

        public List<OnlineUser> users { get; set; }
        public bool[] colorUsed { get; set; }

        public EditionGroup()
        {
            users=new List<OnlineUser>();
            colorUsed = new bool[4];
        }

        public void AddUser(OnlineUser user)
        {
            this.users.Add(user);
        }
        public void RemoveUser(OnlineUser user)
        {
            releaseColor(user.HexColor);
            this.users.Remove(user);
        }
        public string lockNextColor()
        {
            for(int i =0; i<Colors.Length;i++)       {
                if (!colorUsed[i])
                {
                    colorUsed[i] = true;
                    return Colors[i];
                }
            }
            return null;
        }

        public void releaseColor(string color)
        {
            for (int i = 0; i < Colors.Length; i++)
            {
                if (color.Equals(Colors[i]))
                {
                    colorUsed[i]=false;
                }
            }
        }
    }
}
