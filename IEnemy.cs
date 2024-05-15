using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConfinementDemo.Enemy;

namespace ConfinementDemo
{
    internal interface IEnemy
    {
        public MoveResult MakeMove();
    }
}
