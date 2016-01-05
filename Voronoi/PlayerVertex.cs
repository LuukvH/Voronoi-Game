using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;

namespace Voronoi
{
    public class PlayerVertex : Vertex
    {

        public PlayerVertex(Player player, float x, float y) : base(x, y)
        {
         this.Player = player;
        }

        public Player Player { get; private set; }

    }
}
