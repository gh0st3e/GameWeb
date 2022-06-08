using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Templates
{
    public class ReviewEntity
    {
        public int Id { get; set; }
        public string AvatarImg { get; set; }
        public string Name { get; set; }
        public string Review { get; set; }
        public ReviewEntity(int id, string avatarimg,string name,string review)
        {
            Id = id;
            AvatarImg = avatarimg;
            Name = name;
            Review = review;
        }
    }
}
