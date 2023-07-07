using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject PlayTable;
    public List<GameObject> TestCubes = new List<GameObject>(); //test
    public Material TestOff; //test
    public Material TestOn; //test
    public float delay; //test
    public bool isselected; //test
    float timer; //test
    int SelectedSpot = 2; //test

    private void Awake()
    {
        foreach (GameObject obj in TestCubes)
            obj.transform.GetComponent<MeshRenderer>().material = TestOff;
    }
    private void Update()
    {
        if (isselected) //TEST
        {
            timer += Time.deltaTime;
            if (timer > delay)
                NextSpot();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayTable.transform.Rotate(0, 90, 0);
            Playermanager.instance.TurnLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayTable.transform.Rotate(0, -90, 0);
            Playermanager.instance.TurnRight();
        }

        if (Input.GetKeyDown(KeyCode.Space)) //TEST
        {
            isselected = true;
            Invoke("TestInvoke", 5);
 
        }
        if(Input.GetKeyDown(KeyCode.Backspace)) //TEST
            CancelInvoke();

        
    }
    public void NextSpot() //TEST
    {
        TestCubes[SelectedSpot].transform.GetComponent<MeshRenderer>().material = TestOff;
        SelectedSpot++;
        if (SelectedSpot >= TestCubes.Count)
            SelectedSpot = 0;
        TestCubes[SelectedSpot].transform.GetComponent<MeshRenderer>().material = TestOn;
        timer = 0;
    }
    public void TestInvoke()
    {
        
        Debug.Log("InvokeWorkedLOL");
    }
}
