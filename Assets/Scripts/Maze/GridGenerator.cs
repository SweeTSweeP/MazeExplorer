using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private int _gridWidth;
        [SerializeField] private int _gridHeight;
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _node;
        [SerializeField] private GameObject _player;
        [SerializeField] private Transform _parent;

        private List<Node> _freeCells;
        public List<Node> FreeCells => _freeCells;
        public int GridWidth => _gridWidth;
        public int GridHeight => _gridHeight;

        public void BuildGrid()
        {
            if (_gridWidth % 2 == 0)
            {
                _gridWidth++;
            }

            if (_gridHeight % 2 == 0)
            {
                _gridHeight++;
            }

            _freeCells = new List<Node>();

            for (var i = 0; i < _gridWidth; i++)
            {
                for (var j = 0; j < _gridHeight; j++)
                {
                    Instantiate(_node, new Vector3(i, j, 1), Quaternion.identity);

                    if ((i == _gridWidth - 1 || j == _gridHeight - 1) || (i == 0 && j != 0) || (i != 0 && j == 0) || (i == 0 && j == 0))
                    {
                        Instantiate(_wall, new Vector3(i, j, 0), Quaternion.identity);
                    }
                    else
                    {
                        if (i % 2 == 0 || j % 2 == 0)
                        {
                            Instantiate(_wall, new Vector3(i, j, 0), Quaternion.identity);
                        }
                        else
                        {
                            _freeCells.Add(new Node(i, j, false));
                        }
                    }
                }
            }
        }
    }
}
