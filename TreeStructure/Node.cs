using System.Collections.Generic;
using Objects;

namespace TreeStructure
{
    public class Node
    {
        public Node(Face face)
        {
            Face = face;
        }

        public Face Face { get; private set; }

        public List<Node> Children { get; private set; } = new List<Node>();
    }
}
