using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] public float MoveTime;

    [SerializeField] private GameObject[] _vanishVFX = new GameObject[4];
    [SerializeField] private GameObject[] _cards = new GameObject[20];

    private CardBase _card;
    public CardBase Card
    {
        get { return _card; }
        private set { _card = value; }
    }

    public void SetCard(CardBase card)
    {
        Card = card;
        VisualizeCard();
    }

    public void MoveCard(Vector3 position)
    {
        Debug.Log($"Moving Card: {this} from {transform.position} to {position}");
        transform.localRotation = Quaternion.identity;
        EventManager.Instance.TimeStopEvent.Invoke();
        transform.DOLocalMove(position, MoveTime).OnComplete(() => EventManager.Instance.TimeStartEvent.Invoke());
    }

    [Tooltip("Club, Diamond, Heart, Spade")]
    public void VanishCard()
    {
        switch (_card.Face)
        {
            case ECardFace.Club:
                Instantiate(_vanishVFX[0], transform.position, Quaternion.identity);
                break;
            case ECardFace.Diamond:
                Instantiate(_vanishVFX[1], transform.position, Quaternion.identity);
                break;
            case ECardFace.Heart:
                Instantiate(_vanishVFX[2], transform.position, Quaternion.identity);
                break;
            case ECardFace.Spade:
                Instantiate(_vanishVFX[3], transform.position, Quaternion.identity);
                break;
        }
        Destroy(this.gameObject);
    }

    public void VisualizeCard()
    {
        if (_card == null)
            return;

        switch (_card.Colour)
        {
            case ECardColour.Blue:
                switch (_card.Face)
                {
                    case ECardFace.Club:
                        _cards[0].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Diamond:
                        _cards[1].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Heart:
                        _cards[2].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Spade:
                        _cards[3].transform.gameObject.SetActive(true);
                        break;
                }
                break;
            case ECardColour.Green:
                switch (_card.Face)
                {
                    case ECardFace.Club:
                        _cards[4].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Diamond:
                        _cards[5].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Heart:
                        _cards[6].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Spade:
                        _cards[7].transform.gameObject.SetActive(true);
                        break;
                }
                break;
            case ECardColour.Red:
                switch (_card.Face)
                {
                    case ECardFace.Club:
                        _cards[8].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Diamond:
                        _cards[9].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Heart:
                        _cards[10].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Spade:
                        _cards[11].transform.gameObject.SetActive(true);
                        break;
                }
                break;
            case ECardColour.White:
                switch (_card.Face)
                {
                    case ECardFace.Club:
                        _cards[12].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Diamond:
                        _cards[13].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Heart:
                        _cards[14].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Spade:
                        _cards[15].transform.gameObject.SetActive(true);
                        break;
                }
                break;
            case ECardColour.Yellow:
                switch (_card.Face)
                {
                    case ECardFace.Club:
                        _cards[16].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Diamond:
                        _cards[17].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Heart:
                        _cards[18].transform.gameObject.SetActive(true);
                        break;
                    case ECardFace.Spade:
                        _cards[19].transform.gameObject.SetActive(true);
                        break;
                }
                break;
        }
    }
}
