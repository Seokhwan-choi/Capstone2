using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject Inventory;

    private bool is_iv = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Inventory"))
        {
            if (!is_iv)
            {
                is_iv = true;
                Inventory.SetActive(true);
            }
            else
            {
                Inventory.SetActive(false);
                is_iv = false;
            }
        }
	}
}
