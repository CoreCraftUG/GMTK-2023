using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _randomCardAmount; //Amount of random Cards being added to the Base Deck amount
    [SerializeField] private int _deckSize => 16 + _randomCardAmount; //Cards
    [SerializeField] private GameObject _card;
    [SerializeField] private Transform _cardPreview;
    [SerializeField] private List<CardBase> _cards = new List<CardBase>(); //Cards

    [SerializeField] private Stack<CardBase> _deck = new Stack<CardBase>(); //Cards
    public int FacingArea; //Number of the Grid that is currently being faced
    public bool IsSelected; //Is this player currently selected?
    public float SwapDelay; //Timer between slots changing
    private int _possibleSpots = 3;
    public int SelectedSpot; //Currently selected Grid spot
    float timer; //timer
    public CardGrid FacingGrid => Playermanager.instance.Grids[FacingArea]; //Grid
    public CardBase CurrentCard => (_deck.Peek().Equals(null)) ? null : _deck.Peek(); //Card

    private CardHolder _presentedCard;
    [SerializeField] private Material _off;
    [SerializeField] private Material _on;
    [SerializeField] private List<GameObject> _slotIndicators = new List<GameObject>();

    

    private void Awake()
    {
        SelectedSpot = Random.Range(1, 4);
        NewDeck();
    }

    private void Update()
    {
        if (IsSelected)
        {
            timer += Time.deltaTime;
            if (timer > SwapDelay)
                NextSpot();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //    NewDeck();
    }

    private void NewDeck()
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

    public void NextSpot()
    {
        _slotIndicators[SelectedSpot - 1].transform.GetComponent<MeshRenderer>().material = _off;
        SelectedSpot++;
        if (SelectedSpot > _possibleSpots)
            SelectedSpot = 1;
        _slotIndicators[SelectedSpot - 1].transform.GetComponent<MeshRenderer>().material = _on;
        timer = 0;
    }

    public void ResetSlots()
    {
        foreach (GameObject obj in _slotIndicators)
            obj.transform.GetComponent<MeshRenderer>().material = _off;
    }

    public void PreviewNextCard()
    {
        GameObject NewCard = Instantiate(_card, _cardPreview);
        _presentedCard = NewCard.GetComponent<CardHolder>();
        _presentedCard.SetCard(_deck.Pop());
    }

    public void PlayCard()
     {

        Debug.Log($"{_presentedCard == null}" + SelectedSpot + _presentedCard);
        
        Playermanager.instance.CanTurn = false;
        FacingGrid.AddCard(_presentedCard, SelectedSpot);
        EventManager.Instance.TurnEvent.Invoke();
        if (_deck.Count == 0)
            NewDeck();
        else
            PreviewNextCard();
    }
}
