using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CoreCraft.Core;
using Sirenix.OdinInspector;

public class Playermanager : Singleton<Playermanager>
{
    [BoxGroup("Visual"), SerializeField] private Material On;
    [BoxGroup("Visual"), SerializeField] private Material Off;
    [BoxGroup("Visual"), SerializeField] private Image timerCountdown;

    [BoxGroup("Gameplay"), SerializeField] int _endLevel;
    [BoxGroup("Gameplay"), SerializeField] float _startDelay;
    [BoxGroup("Gameplay"), SerializeField] float _delayIncrement;
    [BoxGroup("Gameplay"), SerializeField] float _delayIncreaseTime;
    [BoxGroup("Gameplay"), SerializeField] public List<CardGrid> Grids = new List<CardGrid>(); //List of all available Grids(Grid)
    [BoxGroup("Gameplay"), SerializeField] private List<Player> Players = new List<Player>(); //List of all players

    private bool _gameover;
    private int _randomPlayer = 0; //random player currently being selected(int)
    private int _delayLevel;
    private float _currentDelay; //Time it takes for one player to play a card automatically atm
    private float _delayTimer;

    [HideInInspector] public bool CanTurn = true;
    [HideInInspector] public float Timer;

    private void Awake()
    {
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        BeginPlay();
        _currentDelay = _startDelay;

    }

    private void OnDestroy()
    {
        EventManager.Instance.GameOverEvent.RemoveAllListeners();
    }

    private void OnApplicationQuit()
    {
        EventManager.Instance.GameOverEvent.RemoveAllListeners();
    }

    public void BeginPlay()
    {
        CanTurn = true;
        NextPlayer();
    }

    public Transform ReturnLookTarget()
    {
            return (Players[_randomPlayer].transform);
    }

    private void Update()
    {
        if (_gameover)
            return;

        if (TimeManager.Instance.TimeStop)
            return;

        if(_delayLevel < _endLevel && _delayTimer >= _delayIncreaseTime)
        {
            _delayTimer = 0;
            _delayLevel++;
            _currentDelay -= _delayIncrement;
        }
        if(_delayTimer < _delayIncreaseTime)
        {
            _delayTimer += Time.deltaTime;
        }

        Timer += Time.deltaTime;
        timerCountdown.fillAmount = (_currentDelay - Timer)/_currentDelay;
        if(Timer >= _currentDelay && CanTurn)
        {
            SelectedPlayerPlays();
        }
        else if (Input.GetKeyDown(KeyCode.Space)&&CanTurn)
        {
            SelectedPlayerPlays();
            //CancelInvoke();
        }

    }

    public void NextPlayer()
    {
        _randomPlayer = Random.Range(0, Players.Count);
        Players[_randomPlayer].IsSelected = true;
        Players[_randomPlayer].Level = _delayLevel;
        //Players[_randomPlayer].transform.GetComponent<MeshRenderer>().material = On;
        Players[_randomPlayer].TurnLight.SetActive(true);
        //Invoke("SelectedPlayerPlays", CurrentDelay);
        //Debug.Log(_randomPlayer);
    }

    

    public void SelectedPlayerPlays()
    {
        //Debug.Log(_randomPlayer);
        Players[_randomPlayer].PlayCard();
        Players[_randomPlayer].IsSelected = false;
        Players[_randomPlayer].ResetSlots();
        Players[_randomPlayer].TurnLight.SetActive(false);
        //Players[_randomPlayer].transform.GetComponent<MeshRenderer>().material = Off;
        NextPlayer();
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
