namespace Maze
{
    public class Node
    {
        public int XCoordinate { get; private set; }

        public int YCoordinate { get; private set; }

        public bool IsVisited { get; set; }

        public Node(int x, int y, bool isVisited)
        {
            XCoordinate = x;
            YCoordinate = y;
            IsVisited = isVisited;
        }
    }
}
