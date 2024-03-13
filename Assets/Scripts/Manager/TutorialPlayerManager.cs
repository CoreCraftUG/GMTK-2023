using JamCraft.GMTK2023.Code;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TutorialPlayerManager : PlayerManager
{
    protected bool _canPlace;
    protected bool _pauseGame;
    protected bool _stepDone;
    protected bool _timerStart;
    protected bool _tutorialInProgress = true;
    [ShowInInspector, ReadOnly] protected bool _cardPlacePressed;
    [ShowInInspector, ReadOnly] protected bool _anyKeyPressed;
    protected CardGrid _lastPlacedGrid;
    protected int _lastPlacedSlot;
    protected int _missingSlot;
    protected TutorialPlayer _currentPlayer;



    public bool ContinuePlay;

    protected override void Awake()
    {
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        StartCoroutine(BeginPlay());
        _currentDelay = LevelTime[0];
        _currentNextLevelTime = _nextLevelTime;

        GameInputManager.Instance.OnPlaceCardAction += Instance_OnPlaceCardAction;
        InputSystem.onAnyButtonPress.Call(ctnr => StartCoroutine(SetAnyKeyPlacedPressBool()));
    }

    protected override void Instance_OnPlaceCardAction(object sender, System.EventArgs e)
    {
        if (GameStateManager.Instance.IsGamePaused || GameStateManager.Instance.IsGameOver || _timePlaced) return;

        StartCoroutine(SetCardPlacedPressBool());
    }

    private IEnumerator SetCardPlacedPressBool()
    {
        _cardPlacePressed = true;
        yield return null;
        _cardPlacePressed = false;
    }

    private IEnumerator SetAnyKeyPlacedPressBool()
    {
        _anyKeyPressed = true;
        yield return null;
        _anyKeyPressed = false;
    }

    public IEnumerator BeginPlay()
    {
        yield return new WaitUntil(() =>
        {
            while (Players.Any(p => !p.Ready))
            {
                return false;
            }
            return true;
        });

        StartCoroutine(TutorialGame());
    }

    protected IEnumerator TutorialGame()
    {
        CardTimer.gameObject.SetActive(_timerStart);
        _currentPlayer = (TutorialPlayer)Players[0];
        CardTimer.CardAnimator.speed = 0;

        // First Turn
        #region First Turn
        int playerIndex = 0;
        NextTutorialPlayer(playerIndex);
        // Show Place Card tutorial Message
        EventManager.Instance.TutorialMessage01Event.Invoke(true);
        CanTurn = false;
        yield return new WaitUntil(() => _cardPlacePressed && !GameStateManager.Instance.IsGamePaused);
        EventManager.Instance.TutorialMessage01Event.Invoke(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();
        Debug.Log($"Last placed Slot: {_lastPlacedSlot}");

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Second Turn
        #region Second Turn
        playerIndex = 1;
        NextTutorialPlayer(playerIndex);
        // Can Turn Now show Turn Tutorial Message
        EventManager.Instance.TutorialMessage02Event.Invoke(true);
        CanTurn = true;

        while (_lastPlacedGrid != _currentPlayer.GetFacingGrid())
        {
            yield return null;
        }
        EventManager.Instance.TutorialMessage02Event.Invoke(true);
        CanTurn = false;

        EventManager.Instance.TutorialMessage03Event.Invoke(true);
        yield return new WaitUntil(() => _cardPlacePressed && !GameStateManager.Instance.IsGamePaused && _lastPlacedGrid == _currentPlayer.GetFacingGrid() && _lastPlacedSlot != _currentPlayer.GetCurrentSlot());
        EventManager.Instance.TutorialMessage03Event.Invoke(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        switch (_lastPlacedSlot)
        {
            case 1:
                if (_currentPlayer.GetCurrentSlot() == 2)
                    _missingSlot = 3;
                else
                    _missingSlot = 2;
                break;
            case 2:
                if (_currentPlayer.GetCurrentSlot() == 1)
                    _missingSlot = 3;
                else
                    _missingSlot = 1;
                break;
            case 3:
                if (_currentPlayer.GetCurrentSlot() == 1)
                    _missingSlot = 2;
                else
                    _missingSlot = 1;
                break;
            default:
                throw new System.Exception($"Last placed Slot {_lastPlacedSlot} can not exist");
        }
        Debug.Log($"Missing Slot: {_missingSlot}");
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();
        Debug.Log($"Last placed Slot: {_lastPlacedSlot}");

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Third Turn
        #region Third Turn
        playerIndex = 1;
        NextTutorialPlayer(playerIndex);

        yield return new WaitUntil(() => _timePlaced == false);

        yield return new WaitUntil(() => _currentPlayer.GetCurrentSlot() == _missingSlot);

        yield return new WaitUntil(() => _currentPlayer.GetCurrentSlot() != _missingSlot);

        yield return new WaitUntil(() => _currentPlayer.GetCurrentSlot() == _missingSlot);

        // Pause Game
        EventManager.Instance.TutorialMessage04Event.Invoke(true);

        yield return new WaitUntil(() => _cardPlacePressed && !GameStateManager.Instance.IsGamePaused && _lastPlacedGrid == _currentPlayer.GetFacingGrid() && _missingSlot == _currentPlayer.GetCurrentSlot());

        EventManager.Instance.TutorialMessage04Event.Invoke(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Fourth Turn
        #region Fourth Turn
        playerIndex = 2;
        NextTutorialPlayer(playerIndex);
        CanTurn = false;

        yield return new WaitUntil(() => _cardPlacePressed && !GameStateManager.Instance.IsGamePaused);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Fifth Turn
        #region Fifth Turn
        playerIndex = 3;
        NextTutorialPlayer(playerIndex);

        EventManager.Instance.TutorialMessage05Event.Invoke(true);
        yield return new WaitUntil(() => (_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);
        EventManager.Instance.TutorialMessage05Event.Invoke(false);
        yield return new WaitUntil(() => (!_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        CanTurn = true;
        _timerStart = true;
        CardTimer.gameObject.SetActive(_timerStart);
        CardTimer.CardAnimator.speed = 1;

        EventManager.Instance.TutorialMessage06Event.Invoke(true);
        yield return new WaitUntil(() => Timer >= _currentDelay && !GameStateManager.Instance.IsGamePaused); // Time is Up
        EventManager.Instance.TutorialMessage06Event.Invoke(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Sixth Turn
        #region Sixth Turn
        playerIndex = 1;
        NextTutorialPlayer(playerIndex);
        CanTurn = true;

        yield return new WaitUntil(() => (_cardPlacePressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Seventh Turn
        #region Secenth Turn
        playerIndex = 3;
        NextTutorialPlayer(playerIndex);

        yield return new WaitUntil(() => (_cardPlacePressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // eighth Turn
        #region eighth Turn
        playerIndex = 1;
        NextTutorialPlayer(playerIndex);
        //CanTurn = false;

        _timerStart = false;
        CardTimer.gameObject.SetActive(_timerStart);
        CardTimer.CardAnimator.speed = 0;
        EventManager.Instance.TutorialMessage07Event.Invoke(true);
        yield return new WaitUntil(() => (_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);
        EventManager.Instance.TutorialMessage07Event.Invoke(false);
        yield return new WaitUntil(() => (!_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);
        _timerStart = true;
        CardTimer.gameObject.SetActive(_timerStart);
        CardTimer.CardAnimator.speed = 1;

        yield return new WaitUntil(() => (_cardPlacePressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Ninth Turn
        #region Ninth Turn
        playerIndex = 0;
        NextTutorialPlayer(playerIndex);
        CanTurn = false;

        _delayTimer = 0;
        _delayLevel++;
        if (_delayLevel >= _hardCoreLevel)
            _currentNextLevelTime = _hardCoreNextLevelTime;
        EventManager.Instance.TutorialMessage08Event.Invoke(true);
        yield return new WaitUntil(() => (_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);
        EventManager.Instance.LevelUpEvent.Invoke(_delayLevel);
        EventManager.Instance.TutorialMessage08Event.Invoke(false);
        yield return new WaitUntil(() => (!_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        _currentDelay = _delayLevel >= LevelTime.Length ? LevelTime[LevelTime.Length - 1] : LevelTime[_delayLevel - 1];

        yield return new WaitUntil(() => (_cardPlacePressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Tenth Turn
        #region Tenth Turn
        playerIndex = 2;
        NextTutorialPlayer(playerIndex);
        CanTurn = false;
        _timerStart = false;
        CardTimer.gameObject.SetActive(_timerStart);
        CardTimer.CardAnimator.speed = 0;

        EventManager.Instance.TutorialMessage09Event.Invoke(true);
        yield return new WaitUntil(() => (_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);
        EventManager.Instance.TutorialMessage09Event.Invoke(false);
        yield return new WaitUntil(() => (!_anyKeyPressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        EventManager.Instance.TutorialMessage10Event.Invoke(true);

        yield return new WaitUntil(() => (_cardPlacePressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Eleventh Turn
        #region Eleventh Turn
        playerIndex = 2;
        NextTutorialPlayer(playerIndex);

        yield return new WaitUntil(() => _cardPlacePressed && !GameStateManager.Instance.IsGamePaused && _lastPlacedGrid == _currentPlayer.GetFacingGrid() && _lastPlacedSlot != _currentPlayer.GetCurrentSlot());

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        switch (_lastPlacedSlot)
        {
            case 1:
                if (_currentPlayer.GetCurrentSlot() == 2)
                    _missingSlot = 3;
                else
                    _missingSlot = 2;
                break;
            case 2:
                if (_currentPlayer.GetCurrentSlot() == 1)
                    _missingSlot = 3;
                else
                    _missingSlot = 1;
                break;
            case 3:
                if (_currentPlayer.GetCurrentSlot() == 1)
                    _missingSlot = 2;
                else
                    _missingSlot = 1;
                break;
            default:
                throw new System.Exception($"Last placed Slot {_lastPlacedSlot} can not exist");
        }
        Debug.Log($"Missing Slot: {_missingSlot}");
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();
        Debug.Log($"Last placed Slot: {_lastPlacedSlot}");

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Twelfth Turn
        #region Twelfth Turn
        playerIndex = 2;
        NextTutorialPlayer(playerIndex);

        yield return new WaitUntil(() => (_cardPlacePressed || Timer >= _currentDelay) && !GameStateManager.Instance.IsGamePaused && _lastPlacedGrid == _currentPlayer.GetFacingGrid() && _missingSlot == _currentPlayer.GetCurrentSlot());
        EventManager.Instance.TutorialMessage10Event.Invoke(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        EventManager.Instance.TutorialClearedEvent.Invoke();
        #endregion

        //Wait Untill player procedes to play or returns to title
        yield return new WaitUntil(() => ContinuePlay);
        CanTurn = true;
        _timerStart = true;
        CardTimer.gameObject.SetActive(_timerStart);
        CardTimer.CardAnimator.speed = 0;
        _tutorialInProgress = false;
    }

    protected void NextTutorialPlayer(int index)
    {
        Players[index].IsSelected = true;
        Players[index].Level = _delayLevel;
        Players[index].TurnLight.SetActive(true);
        _currentPlayer = (TutorialPlayer)Players[index];
        _virtualCamera.Follow = Players[_randomPlayer].CameraFocusPoint;
        GameStateManager.Instance.LastPlayerFocusPoint = Players[_randomPlayer].CameraFocusPoint;

        {
            Vector3 CardPos = Players[index].GetPresentedCard().transform.position;
            CardTimer.gameObject.transform.position = new Vector3(CardPos.x, CardPos.y - .03f, CardPos.z);

            CardTimer.gameObject.transform.parent = Players[index].GetPresentedCard().transform;
            CardTimer.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void TutorialPlayerPlays(int index)
    {
        Players[index].PlayCard();
        Players[index].IsSelected = false;
        Players[index].ResetSlots();
        Players[index].TurnLight.SetActive(false);
    }

    public override Transform ReturnLookTarget()
    {
        return (Players[_randomPlayer].GetComponent<Player>().LookTarget);
    }

    protected override void Update()
    {
        if (_gameover)
            return;

        if (TimeManager.Instance.TimeStop)
            return;

        if (!_timerStart)
            return;

        if (!_tutorialInProgress)
        {
            if (_delayLevel < EndLevel && _delayTimer >= _currentNextLevelTime)
            {
                _delayTimer = 0;
                _delayLevel++;
                if (_delayLevel >= _hardCoreLevel)
                    _currentNextLevelTime = _hardCoreNextLevelTime;
                EventManager.Instance.LevelUpEvent.Invoke(_delayLevel);
                _currentDelay = _delayLevel >= LevelTime.Length ? LevelTime[LevelTime.Length - 1] : LevelTime[_delayLevel - 1];
            }
            if (_delayTimer < _currentNextLevelTime)
            {
                _delayTimer += Time.deltaTime;
            }
        }

        Timer += Time.deltaTime;

        CardTimer.SetTimerProgress(Timer / _currentDelay);

        if (_tutorialInProgress)
            return;
        if (Timer >= _currentDelay && CanTurn && _puppyProtection > _maxPuppyProtection)
        {
            _timePlaced = true;
            SelectedPlayerPlays();
            StartCoroutine(TimePlaceDelay());
        }
        else if (_cardPlacePressed && CanTurn && !GameStateManager.Instance.IsGamePaused && !GameStateManager.Instance.IsGameOver && !_timePlaced)
        {
            _timePlaced = true;
            SelectedPlayerPlays();

            StartCoroutine(TimePlaceDelay());
            //CancelInvoke();
        }
    }

    public override void NextPlayer(bool first)
    {
        if (_puppyProtection <= _maxPuppyProtection)
            _puppyProtection++;

        _randomPlayer = UnityEngine.Random.Range(0, Players.Count);
        Players[_randomPlayer].IsSelected = true;
        Players[_randomPlayer].Level = _delayLevel;
        Players[_randomPlayer].TurnLight.SetActive(true);
        _virtualCamera.Follow = Players[_randomPlayer].CameraFocusPoint;
        GameStateManager.Instance.LastPlayerFocusPoint = Players[_randomPlayer].CameraFocusPoint;

        {
            Vector3 CardPos = Players[_randomPlayer].GetPresentedCard().transform.position;
            CardTimer.gameObject.transform.position = new Vector3(CardPos.x, CardPos.y - .03f, CardPos.z);

            CardTimer.gameObject.transform.parent = Players[_randomPlayer].GetPresentedCard().transform;
            CardTimer.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public override void SelectedPlayerPlays()
    {

        Players[_randomPlayer].PlayCard();
        Players[_randomPlayer].IsSelected = false;
        Players[_randomPlayer].ResetSlots();
        Players[_randomPlayer].TurnLight.SetActive(false);
        NextPlayer(false);
    }

    public override void TurnRight()
    {
        foreach (Player player in Players)
        {
            player.FacingArea--;
            if (player.FacingArea < 0)
                player.FacingArea = Players.Count - 1;
        }
    }

    public override void TurnLeft()
    {
        foreach (Player player in Players)
        {
            player.FacingArea++;
            if (player.FacingArea >= Players.Count)
                player.FacingArea = 0;
        }
    }

    private void OnDestroy()
    {
        if (GameInputManager.Instance != null)
        {
            GameInputManager.Instance.OnPlaceCardAction -= Instance_OnPlaceCardAction;
        }
    }

    private void OnApplicationQuit()
    {
        if (GameInputManager.Instance != null)
        {
            GameInputManager.Instance.OnPlaceCardAction -= Instance_OnPlaceCardAction;
        }
    }
}
