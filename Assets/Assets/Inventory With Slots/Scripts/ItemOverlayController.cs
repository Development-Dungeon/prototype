using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOverlayController : MonoBehaviour
{
    public GameObject overlayImagePrefab;
    public Sprite overlayImage;

    [HideInInspector]
    public GameObject createdOverlayObject;

    public void Awake()
    {
        createdOverlayObject = Instantiate(overlayImagePrefab, transform);
        var imageComponent = createdOverlayObject.GetComponent<Image>();
        imageComponent.sprite = overlayImage;


        var itemInSlot = transform.parent.GetComponentInChildren<InventoryItem>();

        if (itemInSlot == null)
            EnableOverlay();
        else
            DisableOverlay();
    }

    public void EnableOverlay()
    {
        if (overlayImage == null)
            return;
        if (createdOverlayObject == null)
            return;

        createdOverlayObject.SetActive(true);
    }

    public void DisableOverlay()
    {
        if (overlayImage == null)
            return;
        if (createdOverlayObject == null)
            return;

        createdOverlayObject.SetActive(false);
    }

}
