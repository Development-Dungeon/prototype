using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItemInHand : MonoBehaviour
{
    public Transform parentTransform;

    private GameObject currentlyHeld;

    void Start()
    {
        InventoryManagerNew.SelectedItemChanged += SelectedItemChanged;
    }

    private void SelectedItemChanged(Item selectedItem)
    {
        if (currentlyHeld != null) Destroy(currentlyHeld);

        if (selectedItem == null) return;

        if (selectedItem.leftHandPrefab != null)
            currentlyHeld = Instantiate(selectedItem.leftHandPrefab, parentTransform.position, parentTransform.rotation, parentTransform);

    }

    private void OnDestroy()
    {
        InventoryManagerNew.SelectedItemChanged -= SelectedItemChanged;
    }
}
