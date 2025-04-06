using UnityEngine;
using UnityEngine.UI;

public class AssignDiamondText : MonoBehaviour
{
    void Start()
    {
        // Assegna questo Text al DiamondManager
        DiamondManager.Instance.AssignUIText(GetComponent<Text>());
    }
}