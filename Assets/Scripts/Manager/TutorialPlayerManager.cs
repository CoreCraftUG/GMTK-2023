using JamCraft.GMTK2023.Code;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TutorialPlayerManager : PlayerManager
{
    protected bool _canPlace;
    protected bool _pauseGame;
    protected bool _stepDone;
    protected bool _timerStart;
    protected bool _tutorialInProgress = true;
    protected CardGrid _lastPlacedGrid;
    protected int _lastPlacedSlot;
    protected int _missingSlot;
    protected TutorialPlayer _currentPlayer;

    // Tutorial Message Text Boxes
    [SerializeField] protected GameObject _message1_SpaceToPlaceCard;
    [SerializeField] protected GameObject _message2_ADToTurnTable;
    [SerializeField] protected GameObject _message3_PlaceCardsNextTOEachOther;
    [SerializeField] protected GameObject _message4_PlaceThreeCardsInARow;
    [SerializeField] protected GameObject _message5_TimerIndicatesTheTime;
    [SerializeField] protected GameObject _message6_IfTheTimeRunsOutTheCardWillBePlacedAutomatically;

    protected override void Awake()
    {
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        StartCoroutine(BeginPlay());
        _currentDelay = LevelTime[0];
        _currentNextLevelTime = _nextLevelTime;
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
        _currentPlayer = (TutorialPlayer)Players[0];
        CardTimer.CardAnimator.speed = 0;

        // First Turn
        #region First Turn
        int playerIndex = 0;
        NextTutorialPlayer(playerIndex);
        // Show Place Card tutorial Message
        _message1_SpaceToPlaceCard.SetActive(true);
        CanTurn = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && !GameStateManager.Instance.IsGamePaused);
        _message1_SpaceToPlaceCard.SetActive(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Second Turn
        #region Second Turn
        playerIndex = 1;
        NextTutorialPlayer(playerIndex);
        // Can Turn Now show Turn Tutorial Message
        _message2_ADToTurnTable.SetActive(true);
        CanTurn = true;

        while (_lastPlacedGrid != _currentPlayer.GetFacingGrid())
        {
            yield return null;
        }
        _message2_ADToTurnTable.SetActive(false);
        CanTurn = false;

        _message3_PlaceCardsNextTOEachOther.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && !GameStateManager.Instance.IsGamePaused && _lastPlacedGrid == _currentPlayer.GetFacingGrid() && _lastPlacedSlot != _currentPlayer.GetCurrentSlot());
        _message3_PlaceCardsNextTOEachOther.SetActive(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion

        // Third Turn
        #region Third Turn
        playerIndex = 1;
        NextTutorialPlayer(playerIndex);

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
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);

        yield return new WaitUntil(() => _currentPlayer.GetCurrentSlot() == _missingSlot);

        yield return new WaitUntil(() => _currentPlayer.GetCurrentSlot() != _missingSlot);

        yield return new WaitUntil(() => _currentPlayer.GetCurrentSlot() == _missingSlot);

        // Pause Game
        _message4_PlaceThreeCardsInARow.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && !GameStateManager.Instance.IsGamePaused && _lastPlacedGrid == _currentPlayer.GetFacingGrid() && _missingSlot == _currentPlayer.GetCurrentSlot());

        _message4_PlaceThreeCardsInARow.SetActive(false);

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
        _timerStart = true;
        CardTimer.CardAnimator.speed = 1;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && !GameStateManager.Instance.IsGamePaused);

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
        CanTurn = true;

        _message5_TimerIndicatesTheTime.SetActive(true);
        // Press any Key or wait for seconds
        _message5_TimerIndicatesTheTime.SetActive(false);

        _message6_IfTheTimeRunsOutTheCardWillBePlacedAutomatically.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && !GameStateManager.Instance.IsGamePaused); // Time is Up
        _message6_IfTheTimeRunsOutTheCardWillBePlacedAutomatically.SetActive(false);

        _timePlaced = true;
        TutorialPlayerPlays(playerIndex);
        StartCoroutine(TimePlaceDelay());

        _lastPlacedGrid = _currentPlayer.GetFacingGrid();
        _lastPlacedSlot = _currentPlayer.GetCurrentSlot();

        yield return new WaitUntil(() => _timePlaced == false);
        #endregion
    }

    protected void NextTutorialPlayer(int index)
    {
        Players[index].IsSelected = true;
        Players[index].Level = _delayLevel;
        Players[index].TurnLight.SetActive(true);
        _currentPlayer = (TutorialPlayer)Players[index];

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

        if (_puppyProtection > _maxPuppyProtection)
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
        else if (Input.GetKeyDown(KeyCode.Space) && CanTurn && !GameStateManager.Instance.IsGamePaused && !GameStateManager.Instance.IsGameOver && !_timePlaced)
        {
            _timePlaced = true;
            SelectedPlayerPlays();

            StartCoroutine(TimePlaceDelay());
            //CancelInvoke();
        }
    }


    protected override IEnumerator TimePlaceDelay()
    {
        yield return new WaitForSeconds(_timeDelayTimePlace);
        _timePlaced = false;
    }

    public override void NextPlayer(bool first)
    {
        if (_puppyProtection <= _maxPuppyProtection)
            _puppyProtection++;

        _randomPlayer = Random.Range(0, Players.Count);
        Players[_randomPlayer].IsSelected = true;
        Players[_randomPlayer].Level = _delayLevel;
        Players[_randomPlayer].TurnLight.SetActive(true);

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
}
