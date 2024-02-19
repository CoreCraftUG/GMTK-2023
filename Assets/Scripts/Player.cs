using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [BoxGroup("Visual"), SerializeField] protected Material _off;
    [BoxGroup("Visual"), SerializeField] protected Material _on;
    [BoxGroup("Visual"), SerializeField] protected List<GameObject> _slotIndicators = new List<GameObject>();
    [BoxGroup("Visual"), SerializeField] public GameObject TurnLight;
    [BoxGroup("Visual"), SerializeField] public List<GameObject> Models;


    [BoxGroup("Gameplay"), SerializeField] protected int _randomCardAmount; //Amount of random Cards being added to the Base Deck amount
    [BoxGroup("Gameplay"), SerializeField] protected int _deckSize => 16 + _randomCardAmount; //Cards
    [BoxGroup("Gameplay"), SerializeField] protected float SwapDelay; //Timer between slots changing
    [BoxGroup("Gameplay"), SerializeField] protected int SwapsPerTurn; //Times the position Swaps per Turn
    [BoxGroup("Gameplay"), SerializeField] protected float SwapDelayIncrement; //Timer between slots changing
    [BoxGroup("Gameplay"), SerializeField] public int FacingArea; //Number of the Grid that is currently being faced
    [BoxGroup("Gameplay"), SerializeField] protected GameObject _card;
    [BoxGroup("Gameplay"), SerializeField] protected Transform _cardPreview;
    [BoxGroup("Gameplay"), SerializeField] protected List<CardBase> _cards = new List<CardBase>(); //Cards

    protected int _currentLevel; //Is this player currently selected?
    protected int _possibleSpots = 3;
    protected int SelectedSpot; //Currently selected Grid spot
    protected float timer; //timer
    protected CardHolder _presentedCard;
    protected Stack<CardBase> _deck = new Stack<CardBase>(); //Card
    [SerializeField] protected float _cardHeight;

    [HideInInspector] public bool IsSelected; //Is this player currently selected?
    [HideInInspector] public int Level; //Is this player currently selected?
    public CardGrid FacingGrid => PlayerManager.Instance.Grids[FacingArea]; //Grid
    public CardBase CurrentCard => (_deck.Peek().Equals(null)) ? null : _deck.Peek(); //Card
    [SerializeField] protected Transform _lookTarget;
    public Transform LookTarget;

    [SerializeField] protected Transform _cameraFocusPoint;

    public Transform CameraFocusPoint => _cameraFocusPoint;

    protected bool _ready;
    public bool Ready { get => _ready; protected set => _ready = value; }

    protected virtual void Awake()
    {
        SelectedSpot = Random.Range(1, 4);
        NewDeck();

        _ready = true;
    }

    public virtual Transform ReturnPresentedCard()
    {
        return _slotIndicators[SelectedSpot - 1].transform.GetChild(0).transform;
    }

    protected virtual void Update()
    {
        if (TimeManager.Instance.TimeStop)
            return;

        _lookTarget.position = PlayerManager.Instance.ReturnLookTarget().position;

        if (_currentLevel < Level)
        {
            _currentLevel = Level;
            SwapDelay = (PlayerManager.Instance.LevelTime.Length >= _currentLevel?   PlayerManager.Instance.LevelTime[_currentLevel - 1] : PlayerManager.Instance.LevelTime[PlayerManager.Instance.LevelTime.Length - 1]) / SwapsPerTurn;
        }

        if (IsSelected)
        {
            timer += Time.deltaTime;
            if (timer > SwapDelay)
                NextSpot();
        }
    }

    protected virtual void NewDeck()
    {
        List<CardBase> tempcards = new List<CardBase>();
        foreach (CardBase x in _cards)
            tempcards.Add(x);
        for(int i = 0; i<_randomCardAmount; i++)
        {
            int j = Random.Range(1, _cards.Count + 1);
            tempcards.Add(_cards[i]);
        }

        for(int z = tempcards.Count - 1; z > 0; z--)
        {
            CardBase temp = tempcards[z];
            int index = Random.Range(0, tempcards.Count);
            tempcards[z] = tempcards[index];
            tempcards[index] = temp;
        }
        foreach (CardBase card in tempcards)
            _deck.Push(card);
        _cards = tempcards;
        PreviewNextCard();
    }

    public virtual void NextSpot()
    {
        _slotIndicators[SelectedSpot - 1].transform.GetComponent<MeshRenderer>().material = _off;
        SelectedSpot++;
        if (SelectedSpot > _possibleSpots)
            SelectedSpot = 1;
        _slotIndicators[SelectedSpot - 1].transform.GetComponent<MeshRenderer>().material = _on;
        _presentedCard.transform.position =  new Vector3(_slotIndicators[SelectedSpot - 1].transform.position.x, _slotIndicators[SelectedSpot - 1].transform.position.y + _cardHeight, _slotIndicators[SelectedSpot - 1].transform.position.z);
        
        timer = 0;
    }

    public virtual void ResetSlots()
    {
        foreach (GameObject obj in _slotIndicators)
            obj.transform.GetComponent<MeshRenderer>().material = _off;
    }

    public virtual void PreviewNextCard()
    {
        GameObject NewCard = Instantiate(_card, _cardPreview);
        _presentedCard = NewCard.GetComponent<CardHolder>();
        _presentedCard.transform.position = new Vector3(_slotIndicators[SelectedSpot - 1].transform.position.x, _slotIndicators[SelectedSpot - 1].transform.position.y + _cardHeight, _slotIndicators[SelectedSpot - 1].transform.position.z);
        _presentedCard.SetCard(_deck.Pop());
    }

    public virtual CardHolder GetPresentedCard()
    {
        return _presentedCard;
    }
    public virtual void PlayCard()
    {
        //PlayerManager.Instance.CanTurn = false;
        FacingGrid.AddCard(_presentedCard, SelectedSpot);
        int i = Random.Range(0, 3);
        EventManager.Instance.PlayAudio.Invoke(i, 4f);
        if (_deck.Count == 0)
            NewDeck();
        else
            PreviewNextCard();
    }
}