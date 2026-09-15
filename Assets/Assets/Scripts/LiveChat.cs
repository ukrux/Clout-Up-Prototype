using UnityEngine;
using System.Collections;

public class LiveChat : MonoBehaviour
{
    [SerializeField] private float messageDelay = 2f;

    private void Start()
    {
        StartCoroutine(ActivateMessages());
    }

    private IEnumerator ActivateMessages()
    {
        while (true)
        {
            // Activate messages from top to bottom
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);

                yield return new WaitForSeconds(messageDelay);
            }

            // Hide all messages
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}