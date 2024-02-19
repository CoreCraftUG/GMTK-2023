using CoreCraft.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CentreGrid : MonoBehaviour
{
    [SerializeField] private int _gridLength;
    [SerializeField] private int _gridWidth;
    [SerializeField] private SpriteDictionary _cardFaceSprites = new SpriteDictionary();
    [SerializeField] private List<Image> _images = new List<Image>();
    [SerializeField] private Color _bronzeColour;
    [SerializeField] private Color _silverColour;
    [SerializeField] private Color _goldColour;

    private List<CardGrid> Grids => PlayerManager.Instance.Grids;
    private CentreGridSlot[,] _grid;
    private Image[,] _spriteGrid;

    private void Start()
    {
        _grid = new CentreGridSlot[_gridLength, _gridWidth];
        _spriteGrid = new Image[_gridLength, _gridWidth];
        int count = 0;
        for(int i  = 0; i < _gridLength; i++)
        {
            for(int ii  = 0; ii < _gridWidth; ii++)
            {
                _spriteGrid[i, ii] = _images[count];
                count++;
            }
        }
    }

    public void CardPathThrough(CardGrid grid, CardHolder card, int slot)
    {
        if(grid == null || !Grids.Contains(grid))
            throw new System.Exception($"Grid is null or not listed {grid}!");
        if(card == null)
            throw new System.Exception($"Card is null!");
        if (slot < 0 || slot > _gridLength || slot > _gridWidth)
            throw new System.Exception($"Slot {slot} does not exist!");

        EDirections direction = (EDirections)Grids.IndexOf(grid);

        switch (direction)
        {
            case EDirections.West:
                {
                    slot = _gridLength - (slot - 1);

                    for (int i = _gridLength - 1; i >= 0; i--)
                    {
                        if (_grid[slot - 1, i].Level == ECentreGridLevel.Gold && _grid[slot - 1, i].Face == card.Card.Face)
                        {
                            EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Gold);
                            _grid[slot - 1, i] = new CentreGridSlot { Level = ECentreGridLevel.None };
                            _spriteGrid[slot - 1, i].sprite = null;
                            _spriteGrid[slot - 1, i].color = Color.white;
                            _spriteGrid[slot - 1, i].gameObject.SetActive(false);
                            continue;
                        }

                        int level = 0;
                        if (_grid[slot - 1, i].Face == card.Card.Face)
                            level = (int)_grid[slot - 1, i].Level;
                        level++;
                        _grid[slot - 1, i] = new CentreGridSlot { Face = card.Card.Face, Level = (ECentreGridLevel)level };

                        _spriteGrid[slot - 1, i].sprite = _cardFaceSprites[_grid[slot - 1, i].Face];
                        switch (_grid[slot - 1, i].Level)
                        {
                            case ECentreGridLevel.None:
                                _spriteGrid[slot - 1, i].color = Color.white;
                                break;
                            case ECentreGridLevel.Bronze:
                                _spriteGrid[slot - 1, i].color = _bronzeColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.None);
                                break;
                            case ECentreGridLevel.Silver:
                                _spriteGrid[slot - 1, i].color = _silverColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Bronze);
                                break;
                            case ECentreGridLevel.Gold:
                                _spriteGrid[slot - 1, i].color = _goldColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Silver);
                                break;
                        }
                        _spriteGrid[slot - 1, i].gameObject.SetActive(true);
                    }
                }
                break;
            case EDirections.North:
                {
                    slot = _gridWidth - (slot - 1);

                    for (int i = _gridWidth - 1; i >= 0; i--)
                    {
                        if (_grid[i, slot - 1].Level == ECentreGridLevel.Gold && _grid[i, slot - 1].Face == card.Card.Face)
                        {
                            EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Gold);
                            _grid[i, slot - 1] = new CentreGridSlot { Level = ECentreGridLevel.None };
                            _spriteGrid[i, slot - 1].sprite = null;
                            _spriteGrid[i, slot - 1].color = Color.white;
                            _spriteGrid[i, slot - 1].gameObject.SetActive(false);
                            continue;
                        }

                        int level = 0;
                        if (_grid[i, slot - 1].Face == card.Card.Face)
                            level = (int)_grid[i, slot - 1].Level;
                        level++;
                        _grid[i, slot - 1] = new CentreGridSlot { Face = card.Card.Face, Level = (ECentreGridLevel)level };

                        _spriteGrid[i, slot - 1].sprite = _cardFaceSprites[_grid[i, slot - 1].Face];
                        switch (_grid[i, slot - 1].Level)
                        {
                            case ECentreGridLevel.None:
                                _spriteGrid[i, slot - 1].color = Color.white;
                                break;
                            case ECentreGridLevel.Bronze:
                                _spriteGrid[i, slot - 1].color = _bronzeColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.None);
                                break;
                            case ECentreGridLevel.Silver:
                                _spriteGrid[i, slot - 1].color = _silverColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Bronze);
                                break;
                            case ECentreGridLevel.Gold:
                                _spriteGrid[i, slot - 1].color = _goldColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Silver);
                                break;
                        }
                        _spriteGrid[i, slot - 1].gameObject.SetActive(true);
                    }
                }
                break;
            case EDirections.East:
                {
                    for (int i = _gridLength - 1; i >= 0; i--)
                    {
                        if (_grid[slot - 1, i].Level == ECentreGridLevel.Gold && _grid[slot - 1, i].Face == card.Card.Face)
                        {
                            EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Gold);
                            _grid[slot - 1, i] = new CentreGridSlot { Level = ECentreGridLevel.None };
                            _spriteGrid[slot - 1, i].sprite = null;
                            _spriteGrid[slot - 1, i].color = Color.white;
                            _spriteGrid[slot - 1, i].gameObject.SetActive(false);
                            continue;
                        }

                        int level = 0;
                        if (_grid[slot - 1, i].Face == card.Card.Face)
                            level = (int)_grid[slot - 1, i].Level;
                        level++;
                        _grid[slot - 1, i] = new CentreGridSlot { Face = card.Card.Face, Level = (ECentreGridLevel)level };

                        _spriteGrid[slot - 1, i].sprite = _cardFaceSprites[_grid[slot - 1, i].Face];
                        switch (_grid[slot - 1, i].Level)
                        {
                            case ECentreGridLevel.None:
                                _spriteGrid[slot - 1, i].color = Color.white;
                                break;
                            case ECentreGridLevel.Bronze:
                                _spriteGrid[slot - 1, i].color = _bronzeColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.None);
                                break;
                            case ECentreGridLevel.Silver:
                                _spriteGrid[slot - 1, i].color = _silverColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Bronze);
                                break;
                            case ECentreGridLevel.Gold:
                                _spriteGrid[slot - 1, i].color = _goldColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Silver);
                                break;
                        }
                        _spriteGrid[slot - 1, i].gameObject.SetActive(true);
                    }
                }
                break;
            case EDirections.South:
                {
                    for (int i = _gridWidth - 1; i >= 0; i--)
                    {
                        if (_grid[i, slot - 1].Level == ECentreGridLevel.Gold && _grid[i, slot - 1].Face == card.Card.Face)
                        {
                            EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Gold);
                            _grid[i, slot - 1] = new CentreGridSlot { Level = ECentreGridLevel.None };
                            _spriteGrid[i, slot - 1].sprite = null;
                            _spriteGrid[i, slot - 1].color = Color.white;
                            _spriteGrid[i, slot - 1].gameObject.SetActive(false);
                            continue;
                        }

                        int level = 0;
                        if (_grid[i, slot - 1].Face == card.Card.Face)
                            level = (int)_grid[i, slot - 1].Level;
                        level++;
                        _grid[i, slot - 1] = new CentreGridSlot { Face = card.Card.Face, Level = (ECentreGridLevel)level };

                        _spriteGrid[i, slot - 1].sprite = _cardFaceSprites[_grid[i, slot - 1].Face];
                        switch (_grid[i, slot - 1].Level)
                        {
                            case ECentreGridLevel.None:
                                _spriteGrid[i, slot - 1].color = Color.white;
                                break;
                            case ECentreGridLevel.Bronze:
                                _spriteGrid[i, slot - 1].color = _bronzeColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.None);
                                break;
                            case ECentreGridLevel.Silver:
                                _spriteGrid[i, slot - 1].color = _silverColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Bronze);
                                break;
                            case ECentreGridLevel.Gold:
                                _spriteGrid[i, slot - 1].color = _goldColour;
                                EventManager.Instance.CentreLevelUpEvent.Invoke(ECentreGridLevel.Silver);
                                break;
                        }
                        _spriteGrid[i, slot - 1].gameObject.SetActive(true);
                    }
                }
                break;
            default:
                throw new System.Exception("How the fuck did we get here?");
        }
    }

    private struct CentreGridSlot
    {
        public ECardFace Face;
        public ECentreGridLevel Level;
    }

    private enum EDirections
    {
        North,
        East,
        South,
        West
    }
}

public enum ECentreGridLevel
{
    None,
    Bronze,
    Silver,
    Gold
}

[Serializable]
public class SpriteDictionary : SerializableDictionary<ECardFace, Sprite> { }