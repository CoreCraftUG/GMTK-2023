using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class CardGrid : MonoBehaviour
{
    [SerializeField] private int _gridLength;
    [SerializeField] private int _gridWidth;
    [SerializeField] private float _fieldLengthSpacing;
    [SerializeField] private float _fieldWidthSpacing;
    [SerializeField] private Transform _gridStartTransform;

    [SerializeField] private GameObject _cardObject;
    [SerializeField] private GameObject _gridSlot;
    [SerializeField] private Transform _gridVisualHolder;
    [SerializeField] private Transform _gridDebugHolder;

    private CardBase[,] _cardField;
    private CardHolder[,] _cardObjects;
    private Vector3[,] _cardPositions;

    [SerializeField] private bool _wait;

    void Start()
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
    }

    public void AddCard(CardHolder card, int slot)
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

    private void CheckForMatch()
    {
        for (int i = 0; i < _gridLength; i++)
        {
            int red = 0;
            int blue = 0;
            int green = 0;
            int yellow = 0;
            int white = 0;
            for (int k = 0; k < _gridWidth; k++)
            {
                if (_cardField[k, i] == null)
                    continue;

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
                    case ECardColour.White:
                        white++;
                        break;
                    default:
                        Debug.LogError($"Card on position {_cardField[k, i]} has an invalid colour");
                        break;
                }
            }

            if(red == _gridWidth || blue == _gridWidth || green == _gridWidth || yellow == _gridWidth || white == _gridWidth)
            {
                StartCoroutine(RowMatch(i));
                break;
            }
        }
        EventManager.Instance.TimeStartEvent.Invoke();
        Playermanager.Instance.CanTurn = true;
        Playermanager.Instance.Timer = 0;
    }

    private IEnumerator RowMatch(int i)
    {

        float time = _cardObjects[_gridWidth - 1, i].MoveTime;
        yield return new WaitForSeconds(time);
        ECardFace face = _cardObjects[_gridWidth - 1, i].Card.Face;
        bool faceMatch = true;
        EventManager.Instance.PlayAudio.Invoke(3, 0);
        for (int j  = _gridWidth - 1; j >= 0; j--)
        {
            if(face != _cardObjects[j, i].Card.Face)
                faceMatch = false;
            _cardObjects[j, i].VanishCard();
            _cardField[j, i] = null;
        }

        EventManager.Instance.MatchingCardsEvent.Invoke(faceMatch);

        yield return new WaitForSeconds(time);
        StartCoroutine(ArrangeField());

        CheckForMatch();

    }

    private IEnumerator ArrangeField()
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

    private void FailedToPlace()
    {
        EventManager.Instance.GameOverEvent.Invoke();
    }

    [Button("Test Card Slot 1")]
    private void TestAt1()
    {
        TestCard(1);
    }

    [Button("Test Card Slot 2")]
    private void TestAt2()
    {
        TestCard(2);
    }

    [Button("Test Card Slot 3")]
    private void TestAt3()
    {
        TestCard(3);
    }

    [SerializeField] private ECardColour _testColour;
    [SerializeField] private ECardFace _testFace;
    List<GameObject> _gridDebugObjects = new List<GameObject>();
    [Button("DebugGrid")]
    private void DebugGrid()
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
    private void VisualizeGrid()
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
    private void DevisualizeGrid()
    {
        foreach (GameObject obj in _gridVisualizeObjects)
        {
            DestroyImmediate(obj);
        }
        _gridVisualizeObjects = new List<GameObject>();
    }

    private void TestCard(int slot)
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