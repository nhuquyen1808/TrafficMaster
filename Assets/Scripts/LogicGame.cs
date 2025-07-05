using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicGame : MonoBehaviour
{
    [SerializeField] LayerMask carLayer;
    Camera _cam;
    RaycastHit _hit;
    public bool isCanClick = true;
    [SerializeField] private Car _currentCar;
    private Vector3 _directionInsCoin;
    [SerializeField] ObjectPool poolCoin;
    [SerializeField] PoolSmokeTrigger poolSmoke;
    [SerializeField] PoolHint poolHint;
    [SerializeField] List<Car> cars = new List<Car>();
    private int coin;
    public LogicUI logicUI;
    public Helicopter helicopter;
    bool isUsingHelicopter;
    [SerializeField] int hintAmount, helicopterAmount, movesAmount, currentLevel;
    [SerializeField] float coinsAmount;
    public int carAmount;
    [SerializeField] Tutorial tutorial;
    public int coinsGet { get; private set; }
    public GameObject coinIconUI;
    Vector3 coinIconPosition;

    public static LogicGame instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ManagerGame.TIME_SCALE = 1;
        _cam = Camera.main;
        Observer.AddObserver(EventAction.EVENT_CAR_DONE_ACTION, CheckCanPlayGame);
        Observer.AddObserver(EventAction.EVENT_GET_COIN_CAR, HandleGetCoinCar);
        Observer.AddObserver(EventAction.EVENT_CAR_TRIGGER, HandleCarTrigger);
        Observer.AddObserver(EventAction.EVENT_CAR_DISABLE, HandleCarDisable);
        Observer.AddObserver(EventAction.EVENT_CAR_HIT_REDLIGHT, HandleHitRedLight);


        SetData();
        tutorial.SetTutAndShow();
        logicUI.SetLockBooster();
        coinIconPosition = _cam.ScreenToWorldPoint(coinIconUI.GetComponent<RectTransform>().position);
    }

    private void HandleHitRedLight(object obj)
    {
        int minusMoveAmount = (int)obj;
        movesAmount -= minusMoveAmount;
        logicUI.UpdateMovesText(movesAmount);
        AudioManager.instance.PlaySound("ThroughRedLine");
    }

    private void OnDestroy()
    {
        Observer.RemoveObserver(EventAction.EVENT_CAR_DONE_ACTION, CheckCanPlayGame);
        Observer.RemoveObserver(EventAction.EVENT_GET_COIN_CAR, HandleGetCoinCar);
        Observer.RemoveObserver(EventAction.EVENT_CAR_TRIGGER, HandleCarTrigger);
        Observer.RemoveObserver(EventAction.EVENT_CAR_DISABLE, HandleCarDisable);
        Observer.RemoveObserver(EventAction.EVENT_CAR_HIT_REDLIGHT, HandleHitRedLight);
    }

    private async void SetData()
    {
        GlobalData.isInGame = true;
        poolCoin.SetupPool();
        poolSmoke.SetupPool();
        poolHint.SetupPool();
        if (PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock) == 0 ||
            PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock) == 21)
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.LevelUnlock, 1);
        }

        currentLevel = PlayerPrefs.GetInt(PlayerPrefsManager.LevelUnlock);
        hintAmount = PlayerPrefs.GetInt(PlayerPrefsManager.hintAmount);
        helicopterAmount = PlayerPrefs.GetInt(PlayerPrefsManager.helicopterAmount);
        coinsAmount = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);

        // LevelGame currentLevelGameLoad = Resources.Load<LevelGame>($"Levels/Level_{currentLevel}");
        if (AddressableLoader.IsAssetExist($"Level_{currentLevel}", typeof(GameObject)))
        {
            GameObject currentLevelGameLoad = await AddressableLoader.LoadAsset<GameObject>($"Level_{currentLevel}");
            LevelGame currentLevelGameSave =
                Instantiate(currentLevelGameLoad.GetComponent<LevelGame>(), this.transform.position,
                    Quaternion.identity);
            currentLevelGameSave.transform.localScale = Vector3.one;
            cars = currentLevelGameSave.cars;
            movesAmount = currentLevelGameSave.moves;
            carAmount = cars.Count;
            coinsGet = carAmount * 10;
            SetupText();
        }
    }

    private void HandleCarDisable(object obj)
    {
        Car car = (Car)obj;
        car.gameObject.layer = LayerMask.NameToLayer("CarDisable");
        cars.Remove(car);
        isCanClick = true;
        CheckAnyCarRunning();
    }

    private bool isShowWin;

    public void CheckWinLose()
    {
        if (cars.Count == 0 && movesAmount >= 0)
        {
            currentLevel++;
            GlobalData.isInGame = false;
            PlayerPrefs.SetInt(PlayerPrefsManager.LevelUnlock, currentLevel);
            DOVirtual.DelayedCall(1, (() =>
            {
                if (!isShowWin)
                {
                    isShowWin = true;
                    logicUI.ShowWinPopup(coinsGet);
                }
            }));
        }
        if (movesAmount <= 0 && carAmount > 0)
        {
            Debug.Log("Lose");
            GlobalData.isInGame = false;
            logicUI.ShowLosePopup(LOSETYPE.OUT_OF_MOVES);
        }
    }

   

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isCanClick && GlobalData.isInGame)
        {
            Ray mousePos = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mousePos, out _hit, Mathf.Infinity, carLayer))
            {
                _currentCar = _hit.collider.gameObject.GetComponent<Car>();
                if (_currentCar == null) return;
                if (!isUsingHelicopter)
                {
                    isCanClick = false;
                    movesAmount--;
                    _currentCar.CarMovement();
                    _directionInsCoin = _currentCar.GetLastDirection();
                    logicUI.UpdateMovesText(movesAmount);
                }
                else
                {
                    carAmount--;
                    AudioManager.instance.PlaySound("Helicopter");
                    isUsingHelicopter = false;
                    GlobalData.isInGame = false;
                    helicopter.MoveToCar(_currentCar);
                    logicUI.PerformBubbleHelicopter(false);
                    if (cars.Contains(_currentCar))
                    {
                        cars.Remove(_currentCar);
                        DOVirtual.DelayedCall(2, (() => { CheckWinLose(); }));
                    }

                    helicopterAmount--;
                    logicUI.UpdateHelicopterAmountText(helicopterAmount);
                }
            }
        }
    }

    public void Hint()
    {
        if (hintAmount > 0)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].CheckCanMove())
                {
                    PooledObject hintPlace = poolHint.GetPooledObject(
                        cars[i].transform.position + new Vector3(0, 0.3f, 0),
                        Quaternion.identity);
                    hintPlace.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    Duck.PlayParticle(hintPlace.GetComponent<ParticleSystem>());
                    break;
                }
            }

            hintAmount--;
            logicUI.UpdateHintAmountText(hintAmount);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public void CallHelicopter()
    {
        if (helicopterAmount > 0)
        {
            if (isUsingHelicopter)
            {
                isUsingHelicopter = false;
                logicUI.PerformBubbleHelicopter(false);
            }
            else
            {
                isUsingHelicopter = true;
                logicUI.PerformBubbleHelicopter(true);
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }


    private void HandleGetCoinCar(object obj)
    {
        Vector3 position = (Vector3)obj;
        PooledObject coinAnim =
            poolCoin.GetPooledObject(position + new Vector3(0, 1f, 0), Quaternion.Euler(new Vector3(0, 90, 0)));
        coinAnim.transform.DOLocalJump(position - 7 * _directionInsCoin, 1, 3, 1)
            .OnComplete(() => { coinAnim.ReturnToPool(); });
        _currentCar = null;
        _directionInsCoin = Vector3.zero;
        coin += 10;
    }

    private void HandleCarTrigger(object obj)
    {
        Vector3 position = (Vector3)obj;
        PooledObject smoke = poolSmoke.GetPooledObject(position + new Vector3(0, 0, -1), Quaternion.identity);
        Duck.PlayParticle(smoke.GetComponent<ParticleSystem>());
        StartCoroutine(ReturnSmokeToPool(smoke));
    }

    IEnumerator ReturnSmokeToPool(PooledObject pooledObject)
    {
        yield return new WaitForSeconds(1f);
        pooledObject.ReturnToPool();
    }

    private void CheckCanPlayGame(object obj)
    {
        isCanClick = (bool)obj;

        /*if (isCanClick)
        {
            carAmount--;
            Debug.Log("car : " + carAmount);
        }*/
    }

    private void CheckAnyCarRunning()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].isOnRunning)
            {
                isCanClick = false;
                break;
            }
        }
    }

    private void SetupText()
    {
        logicUI.SetupLevelText(currentLevel);
        logicUI.UpdateCoinText(coin);
        logicUI.UpdateMovesText(movesAmount);
        logicUI.UpdateHelicopterAmountText(helicopterAmount);
        logicUI.UpdateHintAmountText(hintAmount);
    }
}