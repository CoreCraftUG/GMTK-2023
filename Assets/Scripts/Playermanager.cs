using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CoreCraft.Core;
using JamCraft.GMTK2023.Code;
using Sirenix.OdinInspector;
using System.Linq;

public class Playermanager : Singleton<Playermanager>
{
    [BoxGroup("Visual"), SerializeField] private Material On;
    [BoxGroup("Visual"), SerializeField] private Material Off;
    [BoxGroup("Visual"), SerializeField] private CardAnimation CardTimer;

    [BoxGroup("Gameplay"), SerializeField] public int EndLevel;
    [BoxGroup("Gameplay"), SerializeField] public float[] LevelTime;
    [BoxGroup("Gameplay"), SerializeField] private int _maxPuppyProtection;
    [BoxGroup("Gameplay"), SerializeField] private int _hardCoreLevel;
    [BoxGroup("Gameplay"), SerializeField] private float _timeDelayTimePlace;
    [BoxGroup("Gameplay"), SerializeField] private float _nextLevelTime;
    [BoxGroup("Gameplay"), SerializeField] private float _hardCoreNextLevelTime;
    [BoxGroup("Gameplay"), SerializeField] public List<CardGrid> Grids = new List<CardGrid>(); //List of all available Grids(Grid)
    [BoxGroup("Gameplay"), SerializeField] private List<Player> Players = new List<Player>(); //List of all players

    private bool _gameover;
    private bool _timePlaced;
    private int _randomPlayer = 0; //random player currently being selected(int)
    private int _delayLevel;
    private int _puppyProtection;
    private float _currentNextLevelTime;
    private float _currentDelay; //Time it takes for one player to play a card automatically atm
    private float _delayTimer;

    [HideInInspector] public bool CanTurn = true;
    [HideInInspector] public float Timer;

    private void Awake()
    {
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        StartCoroutine(BeginPlay());
        _currentDelay = LevelTime[0];
        _currentNextLevelTime = _nextLevelTime;
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.GameOverEvent.RemoveAllListeners();
        }
    }

    private void OnApplicationQuit()
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

    public Transform ReturnLookTarget()
    {
            return (Players[_randomPlayer].GetComponent<Player>().LookTarget);
    }

    private void Update()
    {
        if (_gameover)
            return;

        if (TimeManager.Instance.TimeStop)
            return;

        if (_puppyProtection > _maxPuppyProtection)
        {
            if (_delayLevel < EndLevel && _delayTimer >= _nextLevelTime)
            {
                _delayTimer = 0;
                _delayLevel++;
                if (_delayLevel >= _hardCoreLevel)
                    _currentNextLevelTime = _hardCoreNextLevelTime;
                EventManager.Instance.LevelUpEvent.Invoke(_delayLevel);
                _currentDelay = _delayLevel >= LevelTime.Length ? LevelTime[LevelTime.Length - 1] : LevelTime[_delayLevel - 1];
            }
            if (_delayTimer < _nextLevelTime)
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


    private IEnumerator TimePlaceDelay()
    {
        yield return new WaitForSeconds(_timeDelayTimePlace);
        _timePlaced = false;
    }

    public void NextPlayer(bool first)
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

    

    public void SelectedPlayerPlays()
    {
        
        Players[_randomPlayer].PlayCard();
        Players[_randomPlayer].IsSelected = false;
        Players[_randomPlayer].ResetSlots();
        Players[_randomPlayer].TurnLight.SetActive(false);
        NextPlayer(false);
    }

    public void TurnRight()
    {
        foreach (Player player in Players)
        {
            player.FacingArea--;
            if (player.FacingArea < 0)
                player.FacingArea = Players.Count - 1;
        }
    }

    public void TurnLeft()
    {
        foreach (Player player in Players)
        {
            player.FacingArea++;
            if (player.FacingArea >= Players.Count)
                player.FacingArea = 0;
        }
    }

}
