using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Playermanager : MonoBehaviour
{
    [SerializeField] float _startDelay;
    [SerializeField] int _endLevel;
    [SerializeField] float _delayIncrement;
    [SerializeField] float _delayIncreaseTime;
    private float _delayTimer;

    private int _delayLevel;

    public List<Player> Players = new List<Player>(); //List of all players
    public float CurrentDelay; //Time it takes for one player to play a card automatically atm
    private int _randomPlayer; //random player currently being selected(int)
    public List<CardGrid> Grids = new List<CardGrid>(); //List of all available Grids(Grid)
    public static Playermanager instance;
    public int turnCounter = 0; //Turn counter for possibly turning up current delay etc.
    public bool CanTurn = true;
    public Material On;
    public Material Off;
    public float timer;
    [SerializeField] private Image timerCountdown;
    private bool _gameover;

    private void Awake()
    {
        instance = this;
        EventManager.Instance.GameOverEvent.AddListener(() => _gameover = true);
        BeginPlay();
        CurrentDelay = _startDelay;

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
            CurrentDelay -= _delayIncrement;
        }
        if(_delayTimer < _delayIncreaseTime)
        {
            _delayTimer += Time.deltaTime;
        }

        if (_gameover)
            return;
        timer += Time.deltaTime;
        timerCountdown.fillAmount = (CurrentDelay - timer)/CurrentDelay;
        if(timer >= CurrentDelay && CanTurn)
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
        turnCounter++;
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
