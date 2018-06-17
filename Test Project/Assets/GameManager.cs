using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject Inventory;

    private bool is_iv = false;
	void Start () {
        Vector3 pos;
        pos = Inventory.transform.position;
        pos.x = 5000;
        Inventory.transform.position = pos;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Inventory"))
        {
            if (!is_iv)
            {
                is_iv = true;
                Vector3 pos;
                pos = Inventory.transform.position;
                pos.x = 350;
                Inventory.transform.position = pos;
            }
            else
            {
                is_iv = false;
                Vector3 pos;
                pos = Inventory.transform.position;
                pos.x = 5000;
                Inventory.transform.position = pos;
            }
        }
	}
}
