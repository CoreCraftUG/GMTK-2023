using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTopSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _centerField;

    public void CenterfieldSwitch(bool i)
    {
        if (_centerField == null)
            return;

        _centerField.SetActive(i);
   }

}
