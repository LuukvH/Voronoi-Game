using Objects;

namespace TreeStructure
{
    public class Tree
    {
        public Tree(Face root)
        {
            Root = new Node(root);
        }

        public Node Root { get; private set; }

        public void Add(Vertex vertex, params Face[] faces )
        {
            Node node = Search(Root, vertex);

            foreach (Face face in faces)
            {
                node.Children.Add(new Node(face));
            }
        }

        public Face Search(Vertex vertex)
        {
            if (!Root.Face.Inside(vertex))
                return null;

            return Search(Root, vertex).Face;
        }

        private Node Search(Node node, Vertex vertex)
        {
            foreach (Node childNode in node.Children)
            {
                if (childNode.Face.Inside(vertex))
                {
                    return Search(childNode, vertex);
                }
            }

            return node;
        }
    }
}
