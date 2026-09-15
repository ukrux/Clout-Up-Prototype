using UnityEngine;
using UnityEngine.UI;

public class RandomImageChanger : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite[] images;

    public void ChangeRandomImage()
    {
        if (images == null || images.Length == 0)
        {
            Debug.LogWarning("No images assigned!");
            return;
        }

        int randomIndex = Random.Range(0, images.Length);
        targetImage.sprite = images[randomIndex];
    }
}