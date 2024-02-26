using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : Player
{
    [BoxGroup("Gameplay"), SerializeField] protected List<CardBase> _tutorialCards = new List<CardBase>(); //Cards

    protected override void Awake()
    {
        SelectedSpot = Random.Range(1, 4);
        //NewDeck();

        foreach (CardBase card in _tutorialCards)
            _deck.Push(card);

        PreviewNextCard();
        Debug.Log($"Player {gameObject.name} is set up. First card is: Face: {_presentedCard.Card.Face} Colour: {_presentedCard.Card.Colour}");


        _ready = true;
    }

    public override Transform ReturnPresentedCard()
    {
        return _slotIndicators[SelectedSpot - 1].transform.GetChild(0).transform;
    }

    protected override void Update()
    {
        if (TimeManager.Instance.TimeStop)
            return;

        _lookTarget.position = PlayerManager.Instance.ReturnLookTarget().position;

        if (_currentLevel < Level)
        {
            _currentLevel = Level;
            SwapDelay = (PlayerManager.Instance.LevelTime.Length >= _currentLevel ? PlayerManager.Instance.LevelTime[_currentLevel - 1] : PlayerManager.Instance.LevelTime[PlayerManager.Instance.LevelTime.Length - 1]) / SwapsPerTurn;
        }

        if (IsSelected)
        {
            timer += Time.deltaTime;
            if (timer > SwapDelay)
                NextSpot();
        }
    }

    public CardGrid GetFacingGrid() => FacingGrid;

    public int GetCurrentSlot() => SelectedSpot;

    protected override void NewDeck()
    {
        List<CardBase> tempcards = new List<CardBase>();
        foreach (CardBase x in _cards)
            tempcards.Add(x);
        for (int i = 0; i < _randomCardAmount; i++)
        {
            int j = Random.Range(1, _cards.Count + 1);
            tempcards.Add(_cards[i]);
        }

        for (int z = tempcards.Count - 1; z > 0; z--)
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

    public override void NextSpot()
    {
        _slotIndicators[SelectedSpot - 1].transform.GetComponent<MeshRenderer>().material = _off;
        SelectedSpot++;
        if (SelectedSpot > _possibleSpots)
            SelectedSpot = 1;
        _slotIndicators[SelectedSpot - 1].transform.GetComponent<MeshRenderer>().material = _on;
        _presentedCard.transform.position = new Vector3(_slotIndicators[SelectedSpot - 1].transform.position.x, _slotIndicators[SelectedSpot - 1].transform.position.y + _cardHeight, _slotIndicators[SelectedSpot - 1].transform.position.z);

        timer = 0;
    }

    public override void ResetSlots()
    {
        foreach (GameObject obj in _slotIndicators)
            obj.transform.GetComponent<MeshRenderer>().material = _off;
    }

    public override void PreviewNextCard()
    {
        GameObject NewCard = Instantiate(_card, _cardPreview);
        _presentedCard = NewCard.GetComponent<CardHolder>();
        _presentedCard.transform.position = new Vector3(_slotIndicators[SelectedSpot - 1].transform.position.x, _slotIndicators[SelectedSpot - 1].transform.position.y + _cardHeight, _slotIndicators[SelectedSpot - 1].transform.position.z);
        _presentedCard.SetCard(_deck.Pop());
    }

    public override CardHolder GetPresentedCard()
    {
        return _presentedCard;
    }
    public override void PlayCard()
    {
        PlayerManager.Instance.CanTurn = false;
        FacingGrid.AddCard(_presentedCard, SelectedSpot);
        int i = Random.Range(0, 3);
        EventManager.Instance.PlayAudio.Invoke(i, .7f);
        if (_deck.Count == 0)
            NewDeck();
        else
            PreviewNextCard();
    }
}