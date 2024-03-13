using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using JamCraft.GMTK2023.Code;

public class SecondGameModeCardGrid : CardGrid
{
    [SerializeField] private SecondGameModeCardGrid _oppositeGrid;
    [SerializeField] private CentreGrid _centreGrid;

    protected bool _gotACard;

    public override void AddCard(CardHolder card, int slot)
    {
        card.gameObject.transform.SetParent(this.transform);
        bool couldPlace = false;
        #region handle ERROR
        if (slot > _gridWidth)
        {
            Debug.LogError("Slot dose not exist");
            return;
        }
        if (card == null)
        {
            Debug.LogError("Card is null");
            return;
        }
        #endregion
        int s = Random.Range(0, 3);
        SoundManager.Instance.PlaySFX(s);

        if (_cardField[slot - 1, 0] == null)
        {
            _cardField[slot - 1, 0] = card.Card;
            _cardObjects[slot - 1, 0] = card;
            _cardObjects[slot - 1, 0].MoveCard(_cardPositions[slot - 1, 0]);
            couldPlace = true;
        }
        else
        {
            for (int i = 1; i < _gridLength; i++)
            {
                if (_cardField[slot - 1, i] == null)
                {
                    for (int j = i; j >= 0; j--)
                    {
                        if (j > 0)
                        {
                            _cardField[slot - 1, j] = _cardField[slot - 1, j - 1];
                            _cardField[slot - 1, j - 1] = null;
                            _cardObjects[slot - 1, j] = _cardObjects[slot - 1, j - 1];
                            _cardObjects[slot - 1, j - 1].MoveCard(_cardPositions[slot - 1, j]);
                            _cardObjects[slot - 1, j - 1] = null;
                        }
                        else
                        {
                            _cardField[slot - 1, j] = card.Card;
                            _cardObjects[slot - 1, j] = card;
                            _cardObjects[slot - 1, j].MoveCard(_cardPositions[slot - 1, j]);
                        }
                    }
                    couldPlace = true;
                    break;
                }
            }
            if(!couldPlace && _cardObjects[slot - 1, _gridLength - 1] != null)
            {
                if (_oppositeGrid.AddCardFromTop(_cardObjects[slot - 1, _gridLength - 1], slot))
                {
                    _centreGrid.CardPathThrough(this, _cardObjects[slot - 1, _gridLength - 1], slot);
                    _cardObjects[slot - 1, _gridLength - 1] = null;
                    _cardField[slot - 1, _gridLength - 1] = null;
                    for (int j = _gridLength - 1; j >= 0; j--)
                    {
                        if (j > 0)
                        {
                            _cardField[slot - 1, j] = _cardField[slot - 1, j - 1];
                            _cardField[slot - 1, j - 1] = null;
                            _cardObjects[slot - 1, j] = _cardObjects[slot - 1, j - 1];
                            _cardObjects[slot - 1, j - 1].MoveCard(_cardPositions[slot - 1, j]);
                            _cardObjects[slot - 1, j - 1] = null;
                        }
                        else
                        {
                            _cardField[slot - 1, j] = card.Card;
                            _cardObjects[slot - 1, j] = card;
                            _cardObjects[slot - 1, j].MoveCard(_cardPositions[slot - 1, j]);
                        }
                    }
                    couldPlace = true;
                }
            }
        }
        SetLastCard(_cardObjects[slot - 1, 0]);
        if (!couldPlace)
        {
            EventManager.Instance.TimeStopEvent.Invoke();
            FailedToPlace();
            return;
        }

        EventManager.Instance.TimeStopEvent.Invoke();
        EventManager.Instance.TurnEvent.Invoke();
        CheckForMatch();

    }

    protected bool AddCardFromTop(CardHolder card, int slot) //TODO: check for match
    {
        _gotACard = true;
        card.gameObject.transform.SetParent(this.transform);
        slot = _gridLength - (slot - 1);

        if (_cardField[slot - 1, _gridLength - 1] == null)
        {
            _cardField[slot - 1, _gridLength - 1] = card.Card;
            _cardObjects[slot - 1, _gridLength - 1] = card;
            _cardObjects[slot - 1, _gridLength - 1].MoveCard(_cardPositions[slot - 1, _gridLength - 1]);
        }
        else
            return false;

        for (int i = _gridLength - 2; i >= 0; i--)
        {
            if (i >= 0 && _cardField[slot - 1, i] == null)
            {
                _cardField[slot - 1, i] = _cardField[slot - 1, i + 1];
                _cardField[slot - 1, i + 1] = null;
                _cardObjects[slot - 1, i] = _cardObjects[slot - 1, i + 1];
                _cardObjects[slot - 1, i + 1].MoveCard(_cardPositions[slot - 1, i]);
                _cardObjects[slot - 1, i + 1] = null;
            }
            else
                break;
        }

        EventManager.Instance.TimeStopEvent.Invoke();
        EventManager.Instance.TurnEvent.Invoke();
        CheckForMatch();
        return true;
    }

    protected override void CheckForMatch()
    {
        base.CheckForMatch();
        _gotACard = false;
    }

    protected override void StartRowMatch(int row)
    {
        base.StartRowMatch(row);
        EventManager.Instance.MatchFromNeighbourEvent.Invoke();
    }
}