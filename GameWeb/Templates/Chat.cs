using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Templates
{
    public class Chat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AvatarImg { get; set; }
        public string Text { get; set; }

        public Chat(int id, int userid, string avatarimg,string text)
        {
            Id = id;
            UserId = userid;
            AvatarImg = avatarimg;
            Text = text;
        }
    }
}
