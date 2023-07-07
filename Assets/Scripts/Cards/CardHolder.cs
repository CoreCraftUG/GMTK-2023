using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] private float _moveTime;

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
        transform.DOMove(position, _moveTime);
    }

    public void VanishCard()
    {
        Destroy(this);
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
