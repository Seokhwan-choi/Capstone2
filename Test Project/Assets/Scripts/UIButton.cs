using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

    GameObject player;
    Player playerScript;

    public void init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }
    public void LeftDown()
    {
        playerScript.inputLeft = true;
    }
    public void LeftUp()
    {
        playerScript.inputLeft = false;
    }
    public void RightDown()
    {
        playerScript.inputRight = true;
    }
    public void RightUp()
    {
        playerScript.inputRight = false;
    }
    public void Attack()
    {
        playerScript.inputAttack = true;
    }
    public void Jump()
    {
        playerScript.inputJump = true;
    }
    public void Dash()
    {
        playerScript.inputDash = true;
    }
}
