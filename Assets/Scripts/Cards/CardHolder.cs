using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] public float MoveTime;

    [SerializeField] private GameObject _cardObject;
    [SerializeField] private TMP_Text _cardText;

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

    public void VanishCard()
    {
        Destroy(this.gameObject);
    }

    public void VisualizeCard()
    {
        if (_card == null)
            return;

        GameObject card = Instantiate(_cardObject, transform);

        switch (_card.Colour)
        {
            case ECardColour.Red:
                card.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case ECardColour.Blue:
                card.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case ECardColour.Green:
                card.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case ECardColour.Yellow:
                card.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
        }

        _cardText.text = _card.Face.ToString();
    }
}
