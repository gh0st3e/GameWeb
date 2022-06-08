using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Templates
{
    public class GameEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string Descr { get; set; }
        public string LogoImgPath { get; set; }
        public string BackImgPath { get; set; }

        public GameEntity(int id, string name, string category, int price, string descr, string logoimgpath, string backimgpath)
        {
            ID = id;
            Name = name;
            Category = category;
            Price = price;
            Descr = descr;
            LogoImgPath = logoimgpath;
            BackImgPath = backimgpath;
        }
    }
}
