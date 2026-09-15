using UnityEngine;

public class DisableOnKeyPress : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            objectToDisable.SetActive(false);
        }
    }
}