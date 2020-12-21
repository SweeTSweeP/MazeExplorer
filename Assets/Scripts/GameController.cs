using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _coin;
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _scoreText;

    private List<Node> _freeCells;
    private List<GameObject> _coins;
    private int _timerTick;
    private int _seconds;
    private int _minutes;
    private bool _isCoinSpawned;
    private int _spawnedCoins;
    private int _score;
    private int _coinValue;

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
        _isCoinSpawned = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Tick();
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

        if (_freeCells.Count > 0 && _seconds * _spawnedCoins % _timerTick == 0 && _seconds > 0 && !_isCoinSpawned)
        {
            SpawnCoin();
            _isCoinSpawned = !_isCoinSpawned;
            StartCoroutine(WaitForCoin());
            _spawnedCoins++;
        }
    }

    private void SpawnCoin()
    {
        var nodeToSpawn = _freeCells[UnityEngine.Random.Range(0, _freeCells.Count - 1)];
        _coins.Add(Instantiate(_coin, new Vector3(nodeToSpawn.XCoordinate, nodeToSpawn.YCoordinate, 0), Quaternion.identity));
        _freeCells.Remove(nodeToSpawn);
    }

    private IEnumerator WaitForCoin()
    {
        yield return new WaitForSeconds(1f);
        _isCoinSpawned = !_isCoinSpawned;
    }
    public void CoinPickup(Vector3 position)
    {
        foreach (var coin in _coins)
        {
            if (coin.transform.position == position)
            {
                _score += _coinValue;
                _freeCells.Add(new Node((int)position.x, (int)position.y, false, false));

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
    }
}
