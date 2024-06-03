using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Obvious.Soap;
using TMPro;

public class DungeonAllySpawn : MonoBehaviour
{
    [SerializeField] DungeonCameraMovement _cameraMovement;

    [SerializeField] FloatVariable _elixirData;
    [SerializeField] IntVariable _maxElixirData;
    [SerializeField] TMP_Text _textElixir;
    [SerializeField] ScriptableListGameObject allys;
    [SerializeField] BoolVariable _gameStarted;
    [SerializeField] LayerMask _fireBallLayer;
    public GameObject[] elixirImages;
    public bool isSpawning = false;
    public bool castingFireBall = false;

    [SerializeField] GameObject _fireBallPrefab;
    private bool canSpawn = true;
    private SpawnButton currentSpawnButton;

    private Coroutine GenerateElixirCoA;

    private void Awake()
    {
        _elixirData.Value = 2;
    }
    private void OnEnable()
    {
        _gameStarted.Value = false;
        _gameStarted.OnValueChanged += StartGeneratingElixir;
    }

    private void OnDisable()
    {
        _gameStarted.OnValueChanged -= StartGeneratingElixir;
    }

    public void ToggleSpawn(SpawnButton newSpawnButton)
    {
        if(_gameStarted && canSpawn)
        {
            if (isSpawning)
            {
                if(newSpawnButton == currentSpawnButton)
                {
                    newSpawnButton.GetComponent<Image>().color = newSpawnButton.myColor;
                    StopSpawn();
                }
                else
                {
                    currentSpawnButton.GetComponent<Image>().color = currentSpawnButton.myColor;
                    currentSpawnButton = newSpawnButton;
                    newSpawnButton.GetComponent<Image>().color = Color.green;
                }
            }
            else
            {
                currentSpawnButton = newSpawnButton;
                StartSpawn();
                newSpawnButton.GetComponent<Image>().color = Color.green;
            }
        }
    }

    void Update()
    {
        
        if (isSpawning)
        {
            if (Input.touchCount > 0)
            {
 
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended && currentSpawnButton != null)
                {
                    if (PointerIsOverUI(touch.position)) return;

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if(hit.transform.gameObject.CompareTag("SpawnArea") || currentSpawnButton.myUnitData.SpawnAnyWhere)
                            {
                                if(_elixirData.Value >= currentSpawnButton.myUnitData.SpawnCost)
                                {
                                    _elixirData.Value -= currentSpawnButton.myUnitData.SpawnCost;
                                    GameObject newAlly = Instantiate(currentSpawnButton.myUnitData.UnitPrefab, hit.point, currentSpawnButton.myUnitData.UnitPrefab.transform.rotation);
                                    hit.transform.gameObject.TryGetComponent<SetAllyFirstTargets>(out SetAllyFirstTargets saft);
                                    if(saft != null)
                                    newAlly.GetComponent<UnitBehabiourBase>().SetFirstTargets(saft.allysFirstTargets);
                                    currentSpawnButton.GetComponent<Image>().color = currentSpawnButton.myColor;
                                    currentSpawnButton.UpdateGameData();
                                    StopSpawn();
                                    UpdateEnergyUI();
                                }
                            }
                        }
                    
                }
            }
        }
        if (castingFireBall)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    if (PointerIsOverUI(touch.position)) return;

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, _fireBallLayer);
                    RaycastHit hit;

                    if(hits.Length > 0)
                    {
                        hit = hits[0];

                        GameObject instanciatedFireBall = Instantiate(_fireBallPrefab, new Vector3(0, 8, 0), Quaternion.identity);

                        instanciatedFireBall.TryGetComponent<FireBallSpell>(out FireBallSpell spell);

                        spell.InitializeFireBall(hit.point, false);
                        _cameraMovement.canMoveCamera = true;
                        castingFireBall = false;
                        canSpawn = true;
                        Time.timeScale = 1;
                    }

                }
            }
        }
    }

    IEnumerator GenerateElixirCo(bool gameStarted)
    {
        while (gameStarted)
        {
            yield return new WaitForSeconds(2);
            if (_elixirData.Value < _maxElixirData.Value)
            {
                _elixirData.Value += 1;
                UpdateEnergyUI();
            }
            Mathf.Clamp(_elixirData.Value, 0, _maxElixirData.Value);

        }
    }

    public void StartGeneratingElixir(bool gameStarted)
    {
        if(gameStarted)
        {
            GenerateElixirCoA = StartCoroutine(GenerateElixirCo(gameStarted));
        }
        else
        {
            if(GenerateElixirCoA != null)
            {
                StopCoroutine(GenerateElixirCoA);
            }
        }
    }

    private void StartSpawn()
    {
        isSpawning = true;
        //_cameraMovement.canMoveCamera = false;
    }

    private void StopSpawn()
    {
        isSpawning = false;
        //_cameraMovement.canMoveCamera = true;
        currentSpawnButton = null;
    }

    public void UpdateEnergyUI()
    {
        foreach(GameObject image in elixirImages)
        {
            image.SetActive(false);
        }
        for(int i = 0; i <= _elixirData - 1; i++)
        {
            elixirImages[i].SetActive(true);
        }
        _textElixir.text = _elixirData.Value.ToString();
    }

    public void CombatSpell()
    {
        foreach(GameObject ally in allys)
        {
            if(ally.TryGetComponent<UnitBehabiourBase>(out  UnitBehabiourBase unitBehabiour));
            {
                unitBehabiour.SetArmorBuff();
                unitBehabiour.SetRegenerationBuff();
                unitBehabiour.SetDamageBuff();
            }

        }
    }

    public void FireBallSpell()
    {
        _cameraMovement.canMoveCamera = false;
            canSpawn = false;
            StopSpawn();
            castingFireBall = true;
        Time.timeScale = 0.3f;
    }

    public void StartGame()
    {
        _gameStarted.Value = true;
    }

    public bool PointerIsOverUI(Vector2 screenPos)
    {
        var hitObject = UIRaycast(ScreenPosToPointerData(screenPos));
        return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI");
    }

    GameObject UIRaycast(PointerEventData pointerData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count < 1 ? null : results[0].gameObject;
    }

    PointerEventData ScreenPosToPointerData(Vector2 screenPos)
        => new PointerEventData(EventSystem.current) { position = screenPos };
}
