using System;

[System.Serializable]
public class CardBase
{
    public ECardColour Colour;
    public ECardFace Face;
}

public enum ECardFace
{
    Club,
    Diamond,
    Heart,
    Spade
}

public enum ECardColour
{
    Red,
    Green,
    Blue,
    Yellow,
    White
}