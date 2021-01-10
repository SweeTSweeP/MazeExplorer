using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Maze;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _scoreText;

    private List<Node> _freeCells;
    private List<GameObject> _coins;
    private List<(int, int)> _spawnSpots;
    private int _timerTick;
    private int _seconds;
    private int _minutes;
    private int _spawnedCoins;
    private int _score;
    private int _coinValue;
    private int _timeToSpawnDelay;
    private int _countEnemiesToSpawn;
    private bool _isWaitingToSpawnCoin;

    private void Start()
    {
        _coins = new List<GameObject>();
        _timerTick = 5;
        _freeCells = _gridGenerator.FreeCells.ToList();
        _seconds = 0;
        _minutes = 0;
        _score = 0;
        _spawnedCoins = 1;
        _coinValue = 25;
        _timeToSpawnDelay = 5;
        _countEnemiesToSpawn = 6;
        _isWaitingToSpawnCoin = false;

        _spawnSpots = new List<(int, int)>
        {
            (_gridGenerator.GridWidth - 2, _gridGenerator.GridHeight - 2),
            (1, _gridGenerator.GridHeight - 2),
            (_gridGenerator.GridWidth - 2, 1)
        };

        SpawnEnemies(6);
    }
    
    private void Update()
    {
        Tick();
        if (!_isWaitingToSpawnCoin)
        {
            StartCoroutine(WaitToSpawnCoin());
        }
    }

    private void Tick()
    {
        _seconds = _minutes < 1 ? (int)(Time.time) : (int)(Time.time) - 60 * _minutes;

        var seconds = _seconds < 10 ? $"0{_seconds}" : $"{_seconds}";
        var minutes = _minutes < 10 ? $"0{_minutes}" : $"{_minutes}";

        _timer.text = $"{minutes}:{seconds}";

        if (_seconds >= 60)
        {
            _seconds -= 60;
            _minutes++;
        }
    }

    private IEnumerator WaitToSpawnEnemy(float timeToDelay, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(timeToDelay);
        Instantiate(_enemy, spawnPosition, Quaternion.identity);
    }

    private IEnumerator WaitToSpawnCoin()
    {
        _isWaitingToSpawnCoin = true;
        yield return new WaitForSeconds(_timerTick);
        SpawnCoin();
        _spawnedCoins++;
        _isWaitingToSpawnCoin = false;
    }

    private void SpawnCoin()
    {
        if (_freeCells.Count <= 0) return;
        var nodeToSpawn = _freeCells[UnityEngine.Random.Range(0, _freeCells.Count - 1)];
        _coins.Add(Instantiate(_coin, new Vector3(nodeToSpawn.XCoordinate, nodeToSpawn.YCoordinate, 0), Quaternion.identity));
        _freeCells.Remove(nodeToSpawn);
    }

    public void CoinPickup(Vector3 position)
    {
        foreach (var coin in _coins.ToList().Where(coin => coin.transform.position == position))
        {
            _score += _coinValue;
            _freeCells.Add(new Node((int)Math.Round(position.x), (int)Math.Round(position.y), false));

            if (_score >= 10)
            {
                _scoreText.text = $"00{_score}";
            }

            if (_score >= 100)
            {
                _scoreText.text = $"0{_score}";
            }

            if (_score >= 1000)
            {
                _scoreText.text = $"{_score}";
            }

            Destroy(coin);
            _coins.Remove(coin);
        }
    }

    private void SpawnEnemies(int enemiesCount)
    {
        var time = _timeToSpawnDelay;

        var spotsToSpawn = _spawnSpots.ToList();

        for (var i = 0; i < enemiesCount; i++)
        {
            if (spotsToSpawn.Count == 0)
            {
                spotsToSpawn = _spawnSpots.ToList();
            }

            var randomIndex = UnityEngine.Random.Range(0, spotsToSpawn.Count - 1);

            StartCoroutine(WaitToSpawnEnemy(_timeToSpawnDelay, new Vector3(spotsToSpawn[randomIndex].Item1, spotsToSpawn[randomIndex].Item2, 0)));

            spotsToSpawn.Remove(spotsToSpawn[randomIndex]);

            _timeToSpawnDelay += time;
        }
    }
}
