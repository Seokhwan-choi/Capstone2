using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class power_ani : MonoBehaviour {

    public Sprite[] pow = new Sprite[7];

	// Use this for initialization
	void Start () {
        StartCoroutine("ChangeImage");
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    IEnumerator ChangeImage() // 이미지 코루틴
    {
        for (int i = 0; i < 7; i++)
        {
            gameObject.GetComponent<Image>().sprite = pow[i];
            yield return new WaitForSeconds(0.05f);
        }
        //yield return new WaitForSeconds(0.05f);
        StartCoroutine("ChangeImage");
    }
}
