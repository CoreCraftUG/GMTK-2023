using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CoreCraft.Core;
using JamCraft.GMTK2023.Code;
using Sirenix.OdinInspector;
using System.Linq;

public class PlayerManager : Singleton<PlayerManager>
{
    [BoxGroup("Visual"), SerializeField] protected Material On;
    [BoxGroup("Visual"), SerializeField] protected Material Off;
    [BoxGroup("Visual"), SerializeField] protected CardAnimation CardTimer;

    [BoxGroup("Gameplay"), SerializeField] public int EndLevel;
    [BoxGroup("Gameplay"), SerializeField] public float[] LevelTime;
    [BoxGroup("Gameplay"), SerializeField] protected int _maxPuppyProtection;
    [BoxGroup("Gameplay"), SerializeField] protected int _hardCoreLevel;
    [BoxGroup("Gameplay"), SerializeField] protected float _timeDelayTimePlace;
    [BoxGroup("Gameplay"), SerializeField] protected float _nextLevelTime;
    [BoxGroup("Gameplay"), SerializeField] protected float _hardCoreNextLevelTime;
    [BoxGroup("Gameplay"), SerializeField] public List<CardGrid> Grids = new List<CardGrid>(); //List of all available Grids(Grid)
    [BoxGroup("Gameplay"), SerializeField] protected List<Player> Players = new List<Player>(); //List of all players

    protected bool _gameover;
    protected bool _timePlaced;
    protected int _randomPlayer = 0; //random player currently being selected(int)
    protected int _delayLevel;
    protected int _puppyProtection;
    protected float _currentNextLevelTime;
    protected float _currentDelay; //Time it takes for one player to play a card automatically atm
    protected float _delayTimer;

    [HideInInspector] public bool CanTurn = true;
    [HideInInspector] public float Timer;

    protected virtual void Awake()
    {
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        StartCoroutine(BeginPlay());
        _currentDelay = LevelTime[0];
        _currentNextLevelTime = _nextLevelTime;
    }

    protected virtual void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.GameOverEvent.RemoveAllListeners();
        }
    }

    protected virtual void OnApplicationQuit()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.GameOverEvent.RemoveAllListeners();
        }
    }

    public IEnumerator BeginPlay()
    {
        yield return new WaitUntil(() =>
        {
            while(Players.Any(p => !p.Ready))
            {
                return false;
            }
            return true;
        });
        CanTurn = true;
        NextPlayer(true);
    }

    public virtual Transform ReturnLookTarget()
    {
            return (Players[_randomPlayer].GetComponent<Player>().LookTarget);
    }

    protected virtual void Update()
    {
        if (_gameover)
            return;

        if (TimeManager.Instance.TimeStop)
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


    protected virtual IEnumerator TimePlaceDelay()
    {
        yield return new WaitForSeconds(_timeDelayTimePlace);
        _timePlaced = false;
    }

    public virtual void NextPlayer(bool first)
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

    

    public virtual void SelectedPlayerPlays()
    {
        
        Players[_randomPlayer].PlayCard();
        Players[_randomPlayer].IsSelected = false;
        Players[_randomPlayer].ResetSlots();
        Players[_randomPlayer].TurnLight.SetActive(false);
        NextPlayer(false);
    }

    public virtual void TurnRight()
    {
        foreach (Player player in Players)
        {
            player.FacingArea--;
            if (player.FacingArea < 0)
                player.FacingArea = Players.Count - 1;
        }
    }

    public virtual void TurnLeft()
    {
        foreach (Player player in Players)
        {
            player.FacingArea++;
            if (player.FacingArea >= Players.Count)
                player.FacingArea = 0;
        }
    }

}