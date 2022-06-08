using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Templates
{
    public class NewsEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Descr { get; set; }
        public NewsEntity(int id,string name,string image,string descr)
        {
            Id = id;
            Name = name;
            Image = image;
            Descr = descr;
        }
    }
}
