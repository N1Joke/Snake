using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _chankPrefab;
    [SerializeField] private GameObject _foodPrefab;
    [SerializeField] private GameObject _bonusPrefab;
    [SerializeField] private GameObject _deathCubePrefab;
    [SerializeField] private GameObject _changerColorPrefab;
    [SerializeField] private GameObject _finishPrefab;
    [Header("Level settings")]
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _startChank;
    [SerializeField] private int _chankCount;
    [SerializeField] private int _changingColor = 2;
    [SerializeField] private Material _startMat;
    [Header("Cunk settings")]
    [SerializeField] private float _distanceBetwElements;
    [SerializeField] private int _countOfElements;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _leftBorder;
    [SerializeField] private float _rightBorder;
    [SerializeField] private SpawnSettings[] _spawnSettings;
    [Header("Other")]
    [SerializeField] private RandomMaterial _randomMaterial;
    
    private Transform _currentChunk;
    private bool _leftSpawn = true;
    private bool _chunkWithFood = true;
    private Color _currentChankColor;
    private Color _secondChankColor;

    private void Start()
    {
        _currentChankColor = _startMat.color;
        _secondChankColor = _currentChankColor;

        _currentChunk = _startChank;

        int countOfChanks = 0;

        for (int i = 0; i < _chankCount; i++)
        {
            GameObject chank = null;

            if (countOfChanks == _changingColor)
            {
                countOfChanks = 0;
                chank = Instantiate(_changerColorPrefab, _parent);
                chank.transform.position = new Vector3(_currentChunk.position.x, _currentChunk.position.y, _currentChunk.position.z + _currentChunk.localScale.z / 2 + _changerColorPrefab.transform.localScale.z / 2);

                Material material = _randomMaterial.GetRandomMaterial();

                _currentChankColor = material.color;
                chank.GetComponent<ColorSwitcher>().SetColor(_currentChankColor);

                material = _randomMaterial.GetRandomMaterial();
                _secondChankColor = material.color;
            }
            else
            {
                countOfChanks++;
                chank = Instantiate(_chankPrefab, _parent);
                chank.transform.position = new Vector3(_currentChunk.position.x, _currentChunk.position.y, _currentChunk.position.z + _currentChunk.localScale.z / 2 + chank.transform.localScale.z / 2);

                float PosZInChank = chank.transform.position.z - chank.transform.localScale.z / 2 + _distanceBetwElements / 2;

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

                        int RandomColor = Random.Range(0,100);

                        if (RandomColor <= 50)
                            food.GetComponent<Renderer>().material.color = _currentChankColor;
                        else
                            food.GetComponent<Renderer>().material.color = _secondChankColor;

                        _leftSpawn = !_leftSpawn;
                    }
                    else
                    {
                        int rowIndex = Random.Range(0, _spawnSettings.Length - 1);

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
                }
                _chunkWithFood = !_chunkWithFood;                
            }
            _currentChunk = chank.transform;

            if (_chankCount - 1 == i)
            {
                GameObject finish = Instantiate(_finishPrefab, _parent);
                finish.transform.position = new Vector3(_currentChunk.position.x, _currentChunk.position.y, _currentChunk.position.z + _currentChunk.localScale.z / 2 + _finishPrefab.transform.localScale.z / 2);
            }
        }
    }
}
