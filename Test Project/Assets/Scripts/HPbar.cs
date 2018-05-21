using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour {

    public GameObject player;
    public Player playerScript;
    public Slider hpbar;

	// Use this for initialization
	public void init () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        hpbar.value = playerScript.Health_Power;
	}
}
