using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    private void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 positionToLookAt = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.LookAt(new Vector3(positionToLookAt.x, transform.position.y, positionToLookAt.z));
    }
}
