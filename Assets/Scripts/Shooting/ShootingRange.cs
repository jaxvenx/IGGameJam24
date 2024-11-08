using System.Collections;
using UnityEngine;

public class ShootingRange : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 3;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private float moveDistance = 5f;

    private GameObject[] _activeEnemies;
    private Vector3[] _initialPositions;
    private bool[] _movingRight;
    private Coroutine[] _respawnCoroutines;

    private void Start()
    {
        _activeEnemies = new GameObject[enemyCount];
        _initialPositions = new Vector3[enemyCount];
        _respawnCoroutines = new Coroutine[enemyCount];
        _movingRight = new bool[enemyCount];

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(i * 2f, 0, 0);
            _initialPositions[i] = spawnPosition;
            _activeEnemies[i] = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            _movingRight[i] = true;
        }
    }

    private void Update()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (_activeEnemies[i] != null)
            {
                MoveEnemy(i);
            }
            else if(_respawnCoroutines[i] == null)
            {
                _respawnCoroutines[i] = StartCoroutine(RespawnEnemy(i));
            }
        }
    }

    private void MoveEnemy(int index)
    {
        if (_movingRight[index])
        {
            _activeEnemies[index].transform.Translate(Vector3.right * (speed * Time.deltaTime));
            if (_activeEnemies[index].transform.position.x >= _initialPositions[index].x + moveDistance)
            {
                _movingRight[index] = false;
            }
        }
        else
        {
            _activeEnemies[index].transform.Translate(Vector3.left * (speed * Time.deltaTime));
            if (_activeEnemies[index].transform.position.x <= _initialPositions[index].x - moveDistance)
            {
                _movingRight[index] = true;
            }
        }
    }

    private IEnumerator RespawnEnemy(int index)
    {
        yield return new WaitForSeconds(respawnTime);
        _activeEnemies[index] = Instantiate(enemyPrefab, _initialPositions[index], Quaternion.identity);
        _movingRight[index] = true;
        _respawnCoroutines[index] = null;
    }
}