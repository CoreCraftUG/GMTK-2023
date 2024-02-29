using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using Codice.Client.Common;
using HeathenEngineering.SteamworksIntegration;

public class CardGrid : MonoBehaviour
{
    [SerializeField] protected int _gridLength;
    [SerializeField] protected int _gridWidth;
    [SerializeField] protected float _fieldLengthSpacing;
    [SerializeField] protected float _fieldWidthSpacing;
    [SerializeField] protected Transform _gridStartTransform;
    [SerializeField] protected float cardTime;

    [SerializeField] protected GameObject _cardObject;
    [SerializeField] protected GameObject _gridSlot;
    [SerializeField] protected Transform _gridVisualHolder;
    [SerializeField] protected Transform _gridDebugHolder;

    protected CardBase[,] _cardField;
    public CardHolder[,] _cardObjects;
    protected Vector3[,] _cardPositions;
    protected CardHolder _lastCard;

    [SerializeField] protected bool _wait;

    protected void Start()
    {
        _cardField = new CardBase[_gridWidth, _gridLength];
        _cardObjects = new CardHolder[_gridWidth, _gridLength];
        _cardPositions = new Vector3[_gridWidth, _gridLength];

        for (int x = 0; x < _gridWidth; x++)
        {
            for(int z  = 0; z < _gridLength; z++)
            {
                _cardPositions[x, z] = new Vector3(x * _fieldWidthSpacing, 0, z * _fieldLengthSpacing);
            }
        }

        EventManager.Instance.RimExplosionEvent.AddListener((grid, i)=>
        {
            if (grid == this)
                return;
            StartCoroutine(RimExplosion(i));
        });
    }

    public void SetLastCard(CardHolder card)
    {
        _lastCard = card;
    }

    public virtual void AddCard(CardHolder card, int slot)
    {
        card.gameObject.transform.SetParent(this.transform);
        bool couldPlace = false;
        #region handle ERROR
        if (slot > _gridWidth)
        {
            Debug.LogError("Slot dose not exist");
            return;
        }
        if(card == null)
        {
            Debug.LogError("Card is null");
            return;
        }
        #endregion

        if (_cardField[slot - 1, 0] == null)
        {
            _cardField[slot - 1, 0] = card.Card;
            _cardObjects[slot - 1, 0] = card;
            _cardObjects[slot - 1, 0].MoveCard(_cardPositions[slot - 1, 0]);
            couldPlace = true;
            
        }
        else
        {
            for(int i = 1; i< _gridLength; i++)
            {
                if (_cardField[slot - 1, i] == null)
                {
                    for(int j = i; j >= 0; j--)
                    {
                        if(j > 0)
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

    protected virtual void CheckForMatch()
    {
        int red = 0;
        int blue = 0;
        int green = 0;
        int yellow = 0;
        int white = 0;

        bool gridFull = true;
        bool match = false;

        for (int i = 0; i < _gridLength; i++)
        {
            for (int k = 0; k < _gridWidth; k++)
            {
                if (_cardField[k, i] == null)
                {
                    gridFull = false;
                    continue;
                }

                _cardObjects[k, i].HidePrimedExplosion();


                switch (_cardField[k, i].Colour)
                {
                    case ECardColour.Red:
                        red++;
                        break;
                    case ECardColour.Blue:
                        blue++;
                        break;
                    case ECardColour.Green:
                        green++;
                        break;
                    case ECardColour.Yellow:
                        yellow++;
                        break;
                    case ECardColour.Purple:
                        white++;
                        break;
                    default:
                        Debug.LogError($"Card on position {_cardField[k, i]} has an invalid colour");
                        break;
                }
            }
            if (red == _gridWidth - 1 || blue == _gridWidth - 1 || green == _gridWidth - 1 || yellow == _gridWidth - 1 || white == _gridWidth - 1)
            {
                StartPrimedExplosion(i);
            }

            if (red == _gridWidth || blue == _gridWidth || green == _gridWidth || yellow == _gridWidth || white == _gridWidth)
            {
                match = true;
                StartRowMatch(i);
                break;
            }
            else
                TurnOffPrimedExplosion(i);

            red = 0;
            blue = 0;
            green = 0;
            yellow = 0;
            white = 0;
        }

        if (gridFull)
            EventManager.Instance.GridFullEvent.Invoke(this);
        else
            EventManager.Instance.GridNoLongerFullEvent.Invoke(this);

        if (!match)
        {
            //PlayerManager.Instance.CanTurn = true;
            EventManager.Instance.TimeStartEvent.Invoke();

            PlayerManager.Instance.Timer = 0;
        }
    }
    protected void TurnOffPrimedExplosion(int row)
    {
        for(int i = _gridWidth - 1; i >= 0; i--)
        {
            if (_cardObjects[i, row] == null)
                continue;
            _cardObjects[i, row].HidePrimedExplosion();
        }
    }
    protected virtual void StartRowMatch(int row)
    {
        StartCoroutine(RowMatch(row));
    }

    protected virtual void StartPrimedExplosion(int row)
    {
        StartCoroutine(PrimedExplosion(row));
    }

    protected IEnumerator PrimedExplosion(int row)
    {
        float time = cardTime;
        ECardFace face = new ECardFace();
        ECardColour color = new ECardColour();
        bool isfirstrow = false;
        for(int i = _gridWidth - 1; i >= 0; i--)
        {
            if (_cardObjects[i, row] == _lastCard)
                isfirstrow = true;
        }

        if (isfirstrow)
        {
            face = _lastCard.Card.Face;
            color = _lastCard.Card.Colour;
        }
        if (!isfirstrow)
        {
            if (_cardObjects[_gridWidth - 1, row] != null)
            {
                face = _cardObjects[_gridWidth - 1, row].Card.Face;
                color = _cardObjects[_gridWidth - 1, row].Card.Colour;
            }
            else
            {
                face = _cardObjects[_gridWidth - 2, row].Card.Face;
                color = _cardObjects[_gridWidth - 2, row].Card.Colour;
            }
        }


        yield return new WaitForSeconds(time);

        for (int i = _gridWidth - 1; i >= 0; i--)
        {
            if (_cardObjects[i, row] == _lastCard)
                continue;
            if (_cardObjects[i, row] == null)
                continue;
            else if (_cardObjects[i, row].Card.Colour == color && _cardObjects[i, row].Card.Face == face)
            {
                _cardObjects[i, row].ShowPrimedExplosion();
                if(isfirstrow)
                    _lastCard.ShowPrimedExplosion();
            }
        }
        //if (_cardObjects[_gridWidth - 1, row] != null)
        //{
        //    //face = _cardObjects[_gridWidth - 1, row].Card.Face;
        //    //color = _cardObjects[_gridWidth - 1, row].Card.Colour;
        //    for(int i = _gridWidth -2; i>= 0; i--) 
        //    {
        //        if (_cardObjects[i, row] == null)
        //            continue;
        //        if(_cardObjects[i,row].Card.Colour == color && _cardObjects[i,row].Card.Face == face)
        //        {
        //            _cardObjects[i, row].ShowPrimedExplosion();
        //            _cardObjects[_gridWidth - 1, row].ShowPrimedExplosion();
        //        }
        //    }

        //}
        //else
        //{
        //    face = _cardObjects[0, row].Card.Face;
        //    color = _cardObjects[0, row].Card.Colour;
        //    for (int i = _gridWidth - 1; i > 0; i--)
        //    {
        //        if (_cardObjects[i, row] == null)
        //            continue;
        //        if (_cardObjects[i, row].Card.Colour == color && _cardObjects[i, row].Card.Face == face)
        //        {
        //            _cardObjects[i, row].ShowPrimedExplosion();
        //            _cardObjects[0, row].ShowPrimedExplosion();
        //        }
        //    }

        //}

    }

    protected IEnumerator RowMatch(int i)
    {

        float time = _cardObjects[_gridWidth - 1, i].MoveTime;
        yield return new WaitForSeconds(time);
        ECardFace face = _cardObjects[_gridWidth - 1, i].Card.Face;
        ECardColour color = _cardObjects[_gridWidth - 1, i].Card.Colour;
        bool faceMatch = true;
        EventManager.Instance.PlayAudio.Invoke(3, 0);
        List<CardHolder> holder = new List<CardHolder>();
        for (int j  = _gridWidth - 1; j >= 0; j--)
        {
            if(face != _cardObjects[j, i].Card.Face)
                faceMatch = false;
            _cardObjects[j, i].FlipCard();
            //int k = Random.Range(0, 3);
            EventManager.Instance.PlayAudio.Invoke(1, 4);
            holder.Add(_cardObjects[j, i]);
            if (PlayerManager.Instance.WigglingCards.Contains(_cardObjects[j, i]))
                PlayerManager.Instance.WigglingCards.Remove(_cardObjects[j, i]);
            _cardField[j, i] = null;
        }
        yield return new WaitForSeconds(.5f);
        foreach (CardHolder holders in holder)
            holders.VanishCard();
        if (faceMatch)
            EventManager.Instance.RimExplosionEvent.Invoke(this, i);

        EventManager.Instance.MatchingCardsEvent.Invoke(faceMatch);
        EventManager.Instance.RowStreakAchievementEvent.Invoke(color);

        yield return new WaitForSeconds(time);
        StartCoroutine(ArrangeField());

        CheckForMatch();
    }

    protected IEnumerator ArrangeField()
    {
        float time = 0;
        for (int i  = 0; i < _gridLength; i++)
        {
            for(int j =0; j < _gridWidth; j++)
            {
                if (i + 1 < _gridLength && _cardField[j,i] == null)
                {
                    if (_cardField[j, i + 1] != null)
                    {
                        _cardField[j, i] = _cardField[j, i + 1];
                        _cardField[j, i + 1] = null;
                        _cardObjects[j, i + 1].MoveCard(_cardPositions[j, i]);
                        time = _cardObjects[j, i + 1].MoveTime;
                        _cardObjects[j, i] = _cardObjects[j, i + 1];
                        _cardObjects[j, i + 1] = null;
                    }
                }
            }
        }
        yield return new WaitForSeconds(time);
    }

    protected void FailedToPlace()
    {
        EventManager.Instance.GameOverEvent.Invoke();
    }

    protected IEnumerator RimExplosion(int i)
    {
        float time = cardTime;
        int explodedCardsCounter = 0;
        yield return new WaitForSeconds(time);
        EventManager.Instance.PlayAudio.Invoke(3, 0);
        for (int j = _gridWidth - 1; j >= i; j--)
        {
            if (_cardObjects[j, i] != null && _cardField[j, i] != null)
            {
                _cardObjects[j, i].VanishCard();
                _cardField[j, i] = null;
                explodedCardsCounter++;
            }
        }

        EventManager.Instance.RimExplosionCardDeletedEvent.Invoke();
        EventManager.Instance.RimExplosionExplodedCardsEvent.Invoke(explodedCardsCounter);

        yield return new WaitForSeconds(time);
        StartCoroutine(ArrangeField());
    }

    [Button("Test Card Slot 1")]
    protected void TestAt1()
    {
        TestCard(1);
    }

    [Button("Test Card Slot 2")]
    protected void TestAt2()
    {
        TestCard(2);
    }

    [Button("Test Card Slot 3")]
    protected void TestAt3()
    {
        TestCard(3);
    }

    [SerializeField] protected ECardColour _testColour;
    [SerializeField] protected ECardFace _testFace;
    List<GameObject> _gridDebugObjects = new List<GameObject>();
    [Button("DebugGrid")]
    protected void DebugGrid()
    {
        for(int l = _gridDebugObjects.Count - 1; l >= 0; l--)
        {
            Destroy(_gridDebugObjects[l]);
        }

        _gridDebugObjects = new List<GameObject>();

        for (int i = 0; i < _gridWidth; i++)
        {
            for(int j =0; j < _gridLength; j++)
            {
                CardHolder holder = _cardObjects[i, j];
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.SetParent(_gridDebugHolder.transform);
                obj.transform.localPosition = _cardPositions[i, j];
                obj.transform.rotation = Quaternion.identity;
                obj.transform.localScale = new Vector3(_fieldWidthSpacing / 2, _fieldWidthSpacing / 2, _fieldWidthSpacing / 2);
                if (holder == null)
                    obj.GetComponent<MeshRenderer>().material.color = Color.black;
                else
                    obj.GetComponent<MeshRenderer>().material.color = Color.white;
                _gridDebugObjects.Add(obj);
            }
        }
    }

    [SerializeField] List<GameObject> _gridVisualizeObjects = new List<GameObject>();
    [Button("Visualize Grid")]
    protected void VisualizeGrid()
    {
        _cardPositions = new Vector3[_gridWidth, _gridLength];

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridLength; z++)
            {
                _cardPositions[x, z] = new Vector3(x * _fieldWidthSpacing, 0, z * _fieldLengthSpacing);
            }
        }

        foreach (Vector3 vec in _cardPositions)
        {
            GameObject obj = Instantiate(_gridSlot, _gridVisualHolder);
            obj.transform.localPosition = vec;
            _gridVisualizeObjects.Add(obj);
        }
    }

    [Button("Devisualize Grid")]
    protected void DevisualizeGrid()
    {
        foreach (GameObject obj in _gridVisualizeObjects)
        {
            DestroyImmediate(obj);
        }
        _gridVisualizeObjects = new List<GameObject>();
    }

    protected void TestCard(int slot)
    {
        GameObject obj = Instantiate(_cardObject, transform);
        CardHolder holder = obj.GetComponent<CardHolder>();
        int colour = Random.Range(0, 3);
        int face = Random.Range(0, 3);
        CardBase card = new CardBase();
        card.Colour = _testColour;
        card.Face = _testFace;
        holder.SetCard(card);
        obj.name = $"{card.Colour} {card.Face}";

        //int i = Random.Range(1, _gridLength);

        Debug.Log($"Card created Colour: {card.Colour}\nFace: {card.Face} at Slot: {slot}");

        AddCard(holder, slot);
    }
}