using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class PathGenerator : MonoBehaviour
    {
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private AstarPath _astarPath;

        private Stack<Node> _visitedNodes;
        private List<Node> _freeCells;
        private List<GameObject> _walls; 
        private int _gridHeight;
        private int _gridWidth;
        private List<PathDirection> _directions;

        private Node _currentNode;

        private void Start()
        {
            _directions = Enum.GetValues(typeof(PathDirection)).OfType<PathDirection>().ToList();
            UnityEngine.Random.InitState(DateTime.Now.Millisecond * 100);
            _gridGenerator.BuildGrid();

            _visitedNodes = new Stack<Node>();
            _walls = GameObject.FindGameObjectsWithTag("Wall").ToList();
            _freeCells = _gridGenerator.FreeCells.ToList();
            _gridHeight = _gridGenerator.GridHeight;
            _gridWidth = _gridGenerator.GridWidth;

            _currentNode = _freeCells[UnityEngine.Random.Range(0, _freeCells.Count - 1)];
            _currentNode.IsVisited = true;
            _visitedNodes.Push(_currentNode);

            BuildPath();
            GraphsConfiguring();

            _astarPath.Scan();
        }

        private void BuildPath()
        {
            while (_visitedNodes.Count != 0)
            {
                var step = PathStep();

                if (step.Item1 != null)
                {
                    _currentNode = step.Item1;
                    _currentNode.IsVisited = true;
                    _visitedNodes.Push(_currentNode);

                    switch (step.Item2)
                    {
                        case PathDirection.North:
                            RemoveWall(_currentNode.XCoordinate - 1, _currentNode.YCoordinate);
                            break;
                        case PathDirection.East:
                            RemoveWall(_currentNode.XCoordinate, _currentNode.YCoordinate - 1);
                            break;
                        case PathDirection.South:
                            RemoveWall(_currentNode.XCoordinate + 1, _currentNode.YCoordinate);
                            break;
                        case PathDirection.West:
                            RemoveWall(_currentNode.XCoordinate, _currentNode.YCoordinate + 1);
                            break;
                    }
                }
                else
                {
                    Backtrack();
                }
            }
        }

        private (Node, PathDirection) PathStep()
        {
            var directions = _directions.OrderBy(x => Guid.NewGuid()).ToList();

            do
            {
                var randomDirection = directions[UnityEngine.Random.Range(0, directions.Count - 1)];
                var node = CheckDirection(randomDirection, _currentNode);

                if (node != null)
                {
                    return (node, randomDirection);
                }

                directions.Remove(randomDirection);
            } 
            while (directions.Count > 0);

            return (null, PathDirection.North);
        }

        private Node CheckDirection(PathDirection pathDirection, Node node)
        {
            switch (pathDirection)
            {
                case PathDirection.North:
                    if (node.XCoordinate + 2 < _gridWidth - 1)
                    {
                        return FindNode(node.XCoordinate + 2, node.YCoordinate);
                    }
                    break;
                case PathDirection.East:
                    if (node.YCoordinate + 2 < _gridHeight - 1)
                    {
                        return FindNode(node.XCoordinate, node.YCoordinate + 2);
                    }
                    break;
                case PathDirection.South:
                    if (node.XCoordinate - 2 > 0)
                    {
                        return FindNode(node.XCoordinate - 2, node.YCoordinate);
                    }
                    break;
                case PathDirection.West:
                    if (node.YCoordinate - 2 > 0)
                    {
                        return FindNode(node.XCoordinate, node.YCoordinate - 2);
                    }
                    break;
                default:
                    return null;
            }

            return null;
        }

        private Node FindNode(int x, int y)
        {
            foreach (var node in _freeCells)
            {
                if (node.XCoordinate == x && node.YCoordinate == y && !node.IsVisited)
                {
                    return node;
                }
            }

            return null;
        }

        private void RemoveWall(int x, int y)
        {
            GameObject wallToRemove = null;

            foreach (var wall in _walls)
            {
                if (wall.transform.position.x == x && wall.transform.position.y == y)
                {
                    wallToRemove = wall;
                    break;
                }
            }

            if (wallToRemove != null)
            {
                var position = wallToRemove.transform.position;
                _gridGenerator.FreeCells.Add(new Node((int)position.x, (int)position.y, false));
                Destroy(wallToRemove);
                _walls.Remove(wallToRemove);
            }
        }

        private void Backtrack()
        {
            var backtrackNode = _visitedNodes.Peek();

            _freeCells.Remove(backtrackNode);
            _visitedNodes.Pop();

            if (_visitedNodes.Count > 0)
            {
                _currentNode = _visitedNodes.Peek();
            }
        }

        private void GraphsConfiguring()
        {
            var nodeSize = 0.25f;
            var gridSizeMultiplication = 1 / 0.25f;

            _astarPath.data.gridGraph.SetDimensions((int)(_gridWidth * gridSizeMultiplication), (int)(_gridHeight * gridSizeMultiplication), nodeSize);
            _astarPath.data.gridGraph.center = new Vector3(_gridWidth / 2, _gridHeight / 2, 0);
            _astarPath.Scan();
        }
    }
}