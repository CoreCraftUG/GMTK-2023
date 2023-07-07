using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermanager : MonoBehaviour
{
    public List<Player> Players = new List<Player>(); //List of all players
    public float CurrentDelay; //Time it takes for one player to play a card automatically atm
    private int _randomPlayer; //random player currently being selected(int)
    public List<CardGrid> Grids = new List<CardGrid>(); //List of all available Grids(Grid)
    public static Playermanager instance;
    public int turnCounter = 0; //Turn counter for possibly turning up current delay etc.

    private void Awake()
    {
        instance = this;
    }
    public void BeginPlay()
    {
        NextPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectedPlayerPlays();
            CancelInvoke();
        }
    }

    public void NextPlayer()
    {
        _randomPlayer = Random.Range(0, Players.Count - 1);
        Players[_randomPlayer].IsSelected = true;
        Invoke("SelectedPlayerPlays", CurrentDelay);
        Debug.Log(_randomPlayer);
    }

    

    public void SelectedPlayerPlays()
    {
        Players[_randomPlayer].PlayCard();
        Players[_randomPlayer].IsSelected = false;
        turnCounter++;
        NextPlayer();
    }

    public void TurnRight()
    {
        foreach (Player player in Players)
        {
            player.FacingArea--;
            if (player.FacingArea <= 0)
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
