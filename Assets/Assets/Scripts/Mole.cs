using UnityEngine;
using UnityEngine.EventSystems;

public class Mole : MonoBehaviour, IPointerClickHandler
{
    public MoleGameManager gameManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.MoleClicked(gameObject);
    }
}