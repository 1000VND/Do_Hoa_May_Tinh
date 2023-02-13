using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int MedicalItems;
    public int MedicalItemUsed = 0;
    private GameObject[] getCount;
    private GameObject[] getCountLeft;
    // Start is called before the first frame update
    void Start()
    {
        getCount = GameObject.FindGameObjectsWithTag("Medical");
    }

    // Update is called once per frame
    void Update()
    {
        getCountLeft = GameObject.FindGameObjectsWithTag("Medical");
        MedicalItems = getCount.Length - getCountLeft.Length - MedicalItemUsed + 2;

    }
}
