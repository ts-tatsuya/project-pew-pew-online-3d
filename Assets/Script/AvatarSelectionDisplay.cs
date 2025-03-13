using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelectionDisplay : MonoBehaviour
{
    [SerializeField]
    private AvatarSelectionMenuDisplay menuDisplay;
    private void OnEnable()
    {
        StartCoroutine(menuDisplay.LoadAvatarView());
    }

    private void OnDisable()
    {
        menuDisplay.UnloadAvatarView();
    }
}
