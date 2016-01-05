using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;

namespace Voronoi
{
    public class PlayerFace : Face
    {
        public PlayerFace(Player player, HalfEdge halfEdge) : base(halfEdge)
        {
            this.Player = player;
        }

        public Player Player { get; private set; }
    }
}
