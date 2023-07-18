using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNewLineController : MonoBehaviour
{
    private InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputField.isFocused != GameManager.IsUIFocused)
        {
            Debug.Log("focus: " + inputField.isFocused);
            GameManager.IsUIFocused = inputField.isFocused;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputField.lineType = InputField.LineType.MultiLineNewline;
        }
        else
        {
            inputField.lineType = InputField.LineType.MultiLineSubmit;
        }
    }
}
