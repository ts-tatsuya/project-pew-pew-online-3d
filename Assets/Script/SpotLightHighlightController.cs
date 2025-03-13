using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightHighlightController : MonoBehaviour
{
    public List<Transform> highlightPivot = new List<Transform>();
    public int currentHighlightId = 0;
    private Coroutine currentMoveSession;

    private void Awake()
    {
        if (highlightPivot.Count > 0)
        {
            ChangeHighlightPosition(GameManager.PlayerAvatarId);
        }
    }

    public void ChangeHighlightPosition(int highlightId)
    {
        if (currentMoveSession != null)
        {
            StopCoroutine(currentMoveSession);
        }
        currentHighlightId = highlightId;
        Debug.Log(currentHighlightId);
        Vector3 startPosition = transform.position;
        Vector3 destination = new Vector3(
            highlightPivot[highlightId].position.x,
            transform.position.y,
            highlightPivot[highlightId].position.z
        );

        currentMoveSession = StartCoroutine(MoveHighlight(startPosition, destination));
    }

    public IEnumerator MoveHighlight(Vector3 startPosition, Vector3 destination)
    {
        yield return new WaitUntil(() =>
        {

            transform.position = Vector3.Lerp(transform.position, destination, 3 * Time.deltaTime);

            if (transform.position != destination)
            {
                return false;
            }
            else
            {
                return true;
            }
        });
        currentMoveSession = null;
    }
}
