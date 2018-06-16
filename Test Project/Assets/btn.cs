using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btn : MonoBehaviour
{

    public Sprite[] mouse = new Sprite[2];
    // Use this for initialization
    void Start()
    {

        StartCoroutine("ChangeImage");

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(-10, 0, 0);
    }

    IEnumerator ChangeImage()
    {
        for (int i = 0; i < 2; i++)
        {
            gameObject.GetComponent<Image>().sprite = mouse[i];
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine("ChangeImage");
    }
}