using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Login
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Admin { get; set; }
        public int Online { get; set; }
        public string DateFrom { get; set; }
        public string LastDate { get; set; }
        public string AvatarImg { get; set; }
        public User(int id, string login, string password, string name, int admin, int online, string datefrom, string lastdate, string avatarimg)
        {
            Id = id;
            Login = login;
            Password = password;
            Name = name;
            Admin = admin;
            Online = online;
            DateFrom = datefrom;
            LastDate = lastdate;
            AvatarImg = avatarimg;
        }
    }
}
