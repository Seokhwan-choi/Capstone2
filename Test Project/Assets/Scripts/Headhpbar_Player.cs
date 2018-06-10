using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Headhpbar_Player : MonoBehaviour {

    public GameObject Player;
    public Player PlayerScript;
    public GameObject HeadUp;
    public Slider HpBar;


    // Use this for initialization
    public void init()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HpBar.value = PlayerScript.Health_Power;
        HpBar.transform.position = HeadUp.transform.position;
    }
}
