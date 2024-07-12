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

    public void Start()
    {
        createdOverlayObject = Instantiate(overlayImagePrefab, transform);
        var imageComponent = createdOverlayObject.GetComponent<Image>();
        imageComponent.sprite = overlayImage;
    }

    public void EnableOverlay()
    {
        if (overlayImage == null)
            return;

        createdOverlayObject.SetActive(true);
    }

    public void DisableOverlay()
    {
        if (overlayImage == null)
            return;

        createdOverlayObject.SetActive(false);
    }

}
