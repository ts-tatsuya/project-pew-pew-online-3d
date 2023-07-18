using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChatDisplay : MonoBehaviour
{
    [Header("Necessary Gameobject")]
    [SerializeField]
    private GameObject chatWindow;
    [SerializeField]
    private GameObject chatWindowAnchorDefault;
    [SerializeField]
    private GameObject chatWindowAnchorCollapsed;
    [SerializeField]
    private GameObject arrowUI;
    [Header("Config")]
    [SerializeField]
    private float animateSpeed;
    private bool isAnimating;
    private Vector3 target;
    private bool isCollapsed;
    // Start is called before the first frame update
    void Start()
    {
        isAnimating = false;
        isCollapsed = true;
        GetComponent<Button>().onClick.AddListener(ChatDisplayToggle);
        chatWindow.transform.position = chatWindowAnchorCollapsed.transform.position;
    }

    void Update()
    {
        if (isAnimating)
        {
            chatWindow.transform.position =
            Vector3.MoveTowards(chatWindow.transform.position, target, animateSpeed * Time.deltaTime);
            if (chatWindow.transform.position == target)
            {
                isAnimating = false;
            }
        }
    }

    public void ChatDisplayToggle()
    {
        if (isAnimating == false && Cursor.visible)
        {
            isAnimating = true;
            if (isCollapsed)
            {
                target = chatWindowAnchorDefault.transform.position;
                isCollapsed = false;
            }
            else
            {
                target = chatWindowAnchorCollapsed.transform.position;
                isCollapsed = true;
            }
        }
    }
}
