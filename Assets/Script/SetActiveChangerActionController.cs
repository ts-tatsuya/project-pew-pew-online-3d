using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetActiveChangerActionController : MonoBehaviour
{
    [Header("Object To Set")]
    [SerializeField]
    private List<ObjectToSet> objectToSetList = new List<ObjectToSet>();
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach (ObjectToSet objectToSet in objectToSetList)
            {
                objectToSet.objectToSet.SetActive(objectToSet.isSetToActive);
            }
        });
    }
}
