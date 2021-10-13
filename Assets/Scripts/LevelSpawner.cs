using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _chankPrefab;
    [SerializeField] private GameObject _foodPrefab;
    [SerializeField] private GameObject _bonusPrefab;
    [SerializeField] private GameObject _deathCubePrefab;
    [Header("Level settings")]
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _startChank;
    [SerializeField] private int _chankCount;
    [Header("Cunk settings")]
    [SerializeField] private float _distanceBetwElements;
    [SerializeField] private int _countOfElements;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _leftBorder;
    [SerializeField] private float _rightBorder;
    [SerializeField] private SpawnSettings[] _spawnSettings;

    private Transform _currentChunk;
    private bool _leftSpawn = true;
    private bool _chunkWithFood = true;

    private void Start()
    {
        _currentChunk = _startChank;

        for (int i = 0; i < _chankCount; i++)
        {
            GameObject chank = Instantiate(_chankPrefab, _parent);
            chank.transform.position = new Vector3(_currentChunk.position.x, _currentChunk.position.y, _currentChunk.position.z + _currentChunk.localScale.z / 2);

            float PosZInChank = chank.transform.position.z - chank.transform.localScale.z / 2 + _distanceBetwElements;

            for (int j = 0; j < _countOfElements; j++)
            {
                if (_chunkWithFood)
                {
                    GameObject food = Instantiate(_foodPrefab, _parent);
                    float PosX;
                    if (_leftSpawn)
                    {
                        PosX = Random.Range(_leftBorder, 0);
                    }
                    else
                    {
                        PosX = Random.Range(0, _rightBorder);
                    }
                    food.transform.position = new Vector3(PosX, _offsetY, PosZInChank);                    
                    _leftSpawn = !_leftSpawn;                    
                }
                else
                {
                    int rowIndex = Random.Range(0, _spawnSettings.Length-1);

                    SpawnSettings spawnSettings = _spawnSettings[rowIndex];

                    for (int BonusIndex = 0; BonusIndex < spawnSettings.PosXBonus.Length; BonusIndex++)
                    {
                        GameObject bonus = Instantiate(_bonusPrefab, _parent);

                        bonus.transform.position = new Vector3(spawnSettings.PosXBonus[BonusIndex], _offsetY, PosZInChank);
                    }

                    for (int DeathCubeIndex = 0; DeathCubeIndex < spawnSettings.PosXDeathCube.Length; DeathCubeIndex++)
                    {
                        GameObject deathCube = Instantiate(_deathCubePrefab, _parent);

                        deathCube.transform.position = new Vector3(spawnSettings.PosXDeathCube[DeathCubeIndex], _offsetY, PosZInChank);                        
                    }
                }

                PosZInChank += _distanceBetwElements;

                _chunkWithFood = !_chunkWithFood;
            }

            _currentChunk = chank.transform;
        }        
    }
}
