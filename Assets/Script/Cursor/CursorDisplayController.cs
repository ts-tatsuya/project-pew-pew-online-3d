using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDisplayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Cursor.visible == false)
        {
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt) && Cursor.visible == true)
        {
            Cursor.visible = false;
        }
    }
}
