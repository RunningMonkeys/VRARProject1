using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dateTracker : MonoBehaviour
{
    public GameObject dateTextObject;

    // Start is called before the first frame update
    void Start()
    {
		InvokeRepeating("UpdateDate", 0f, 30f);   
    }

    // Update is called once per frame
    void UpdateDate()
    {
		dateTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("MM/dd/yy");
    }
}
