using UnityEngine;

public class CardBase
{
    public ECardColour Colour;
    public ECardFace Face;
}

public enum ECardFace
{
    Star,
    Heart,
    Circle,
    Square
}

public enum ECardColour
{
    Red,
    Green,
    Blue,
    Yellow
}