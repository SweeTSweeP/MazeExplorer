public class Node
{
    private int _xCoordinate;
    private int _yCoordinate;
    private bool _isVisited;
    private bool _isNoWayOut;

    public int XCoordinate
    {
        get
        {
            return _xCoordinate;
        }
        set
        {
            _xCoordinate = value;
        }
    }

    public int YCoordinate
    {
        get
        {
            return _yCoordinate;
        }
        set
        {
            _yCoordinate = value;
        }
    }

    public bool IsVisited
    {
        get
        {
            return _isVisited;
        }
        set
        {
            _isVisited = value;
        }
    }

    public bool IsNoWayOut
    {
        get
        {
            return _isNoWayOut;
        }
        set
        {
            _isNoWayOut = value;
        }
    }

    public Node(int x, int y, bool isVisited, bool isNoWayOut)
    {
        _xCoordinate = x;
        _yCoordinate = y;
        _isVisited = isVisited;
        _isNoWayOut = isNoWayOut;
    }
}
