using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CoreCraft.Core;

public class Playermanager : Singleton<Playermanager>
{
    [SerializeField] int _endLevel;
    [SerializeField] float _startDelay;
    [SerializeField] float _delayIncrement;
    [SerializeField] float _delayIncreaseTime;
    [SerializeField] private Material On;
    [SerializeField] private Material Off;
    [SerializeField] private Image timerCountdown;
    [SerializeField] public List<CardGrid> Grids = new List<CardGrid>(); //List of all available Grids(Grid)
    [SerializeField] private List<Player> Players = new List<Player>(); //List of all players

    private float _currentDelay; //Time it takes for one player to play a card automatically atm
    private int _randomPlayer; //random player currently being selected(int)
    private float timer;
    private bool _gameover;
    private float _delayTimer;
    private int _delayLevel;

    [HideInInspector] public bool CanTurn = true;

    private void Awake()
    {
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        BeginPlay();
        _currentDelay = _startDelay;

    }
    public void BeginPlay()
    {
        CanTurn = true;
        NextPlayer();
    }

    private void Update()
    {
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

        if (_gameover)
            return;
        timer += Time.deltaTime;
        timerCountdown.fillAmount = (_currentDelay - timer)/_currentDelay;
        if(timer >= _currentDelay && CanTurn)
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
        Players[_randomPlayer].transform.GetComponent<MeshRenderer>().material = On;
        //Invoke("SelectedPlayerPlays", CurrentDelay);
        //Debug.Log(_randomPlayer);
    }

    

    public void SelectedPlayerPlays()
    {
        //Debug.Log(_randomPlayer);
        Players[_randomPlayer].PlayCard();
        Players[_randomPlayer].IsSelected = false;
        Players[_randomPlayer].ResetSlots();
        Players[_randomPlayer].transform.GetComponent<MeshRenderer>().material = Off;
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
