using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalSwitch : MonoBehaviour
{

    [SerializeField] private List<GameObject> _decals1;
    [SerializeField] private List<GameObject> _decals2;
    [SerializeField] private List<GameObject> _decals3;
    [SerializeField] private List<GameObject> _decals4;
    
    private int _currentDecal = 0;
    public bool IsTurning;
    private GameObject _lastLight;

    public void NextDecal(int i)
    {
        if(_lastLight != null)
            _lastLight.GetComponent<DecalProjector>().material.SetInt("_Filled", 0);

        switch (_currentDecal)
        {
            case 0:
                _decals1[i].GetComponent<DecalProjector>().material.SetInt("_Filled", 1);
                _lastLight = _decals1[i];
                break;
            case 1:
                _decals2[i].GetComponent<DecalProjector>().material.SetInt("_Filled", 1);
                _lastLight = _decals1[i];
                break;
            case 2:
                _decals3[i].GetComponent<DecalProjector>().material.SetInt("_Filled", 1);
                _lastLight = _decals1[i];
                break;
            case 3:
                _decals4[i].GetComponent<DecalProjector>().material.SetInt("_Filled", 1);
                _lastLight = _decals1[i];
                break;
        }
    }

    public void NextDecalSet(int i)
    {
        if(i > 0)
        {
            if (_currentDecal++ > 3)
                _currentDecal = 0;
            else
                _currentDecal++;
        }
        else if(i < 0)
        {
            if (_currentDecal-- < 0)
                _currentDecal = 3;
            else
                _currentDecal--;
        }

    }






}
