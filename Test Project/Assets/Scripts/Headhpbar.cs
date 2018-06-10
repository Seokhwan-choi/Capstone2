using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Headhpbar : MonoBehaviour {

    public GameObject Monster;
    public Mon_Move2 MonScript;
    public GameObject HeadUp;
    public Slider HpBar;


    // Use this for initialization
    public void init () {
        Monster = GameObject.FindGameObjectWithTag("Monster");
        MonScript = Monster.GetComponent<Mon_Move2>();
	}
	
	// Update is called once per frame
	void Update () {
        HpBar.value = MonScript.M_Health;
        HpBar.transform.position = HeadUp.transform.position;
	}
}
