using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWeb.Patterns
{
    public class UnitOfWork
    {
        public UserRepo UserRepo = new UserRepo();
        public GameRepo GameRepo = new GameRepo();
        public ReviewRepo ReviewRepo = new ReviewRepo();
    }
}
