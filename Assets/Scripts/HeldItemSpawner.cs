//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Original Purpose: Handles the spawning and returning of the ItemPackage
// 
// Modified by Dylan Gedig.
//  New Purpose: Create a zone that the user can reach into and pull out an object attached to their hand.
//
//=============================================================================

using UnityEngine;
using Valve.VR.InteractionSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

//------------------------------------------------------------------
[RequireComponent(typeof(Interactable))]
public class HeldItemSpawner : MonoBehaviour {
    public ItemPackage itemPackage {
        get {
            return _itemPackage;
        }
        set {
            CreatePreviewObject();
        }
    }

    public ItemPackage _itemPackage;

    private bool useItemPackagePreview = true;
    private GameObject previewObject;

    public bool requireGrabActionToTake = false;
    public bool showTriggerHint = false;

    [EnumFlags]
    public Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags;

    private GameObject spawnedItem;

    public bool justPickedUpItem = false;


    //-------------------------------------------------
    private void CreatePreviewObject() {
        if (!useItemPackagePreview) {
            return;
        }

        ClearPreview();

        if (useItemPackagePreview) {
            if (itemPackage == null) {
                return;
            }


            if (itemPackage.previewPrefab != null) {
                previewObject = Instantiate(itemPackage.previewPrefab, transform.position, Quaternion.identity) as GameObject;
                previewObject.transform.parent = transform;
                previewObject.transform.localRotation = Quaternion.identity;
            }
        }
    }


    //-------------------------------------------------
    void Start()
    {
        VerifyItemPackage();
    }


    //-------------------------------------------------
    private void VerifyItemPackage() {
        if (itemPackage == null) {
            ItemPackageNotValid();
        }

        if (itemPackage.itemPrefab == null) {
            ItemPackageNotValid();
        }
    }


    //-------------------------------------------------
    private void ItemPackageNotValid() {
        Debug.LogError("<b>[SteamVR Interaction (Modifed)]</b> ItemPackage assigned to " + gameObject.name + " is not valid. Destroying this game object.");
        Destroy(gameObject);
    }


    //-------------------------------------------------
    private void ClearPreview() {
        foreach (Transform child in transform) {
            if (Time.time > 0) {
                GameObject.Destroy(child.gameObject);
            } else {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }


    //-------------------------------------------------
    private void OnHandHoverBegin(Hand hand) {

        if (!requireGrabActionToTake) // we don't require trigger press for pickup. Spawn and attach object.
        {
            SpawnAndAttachObject(hand, GrabTypes.Scripted);
        }

        if (requireGrabActionToTake && showTriggerHint) {
            hand.ShowGrabHint("PickUp");
        }
    }


    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand) {
        if (requireGrabActionToTake) {
            GrabTypes startingGrab = hand.GetGrabStarting();

            if (startingGrab != GrabTypes.None) {
                SpawnAndAttachObject(hand, startingGrab);
            }
        }
    }


    //-------------------------------------------------
    private void OnHandHoverEnd(Hand hand) {
        if (!justPickedUpItem && requireGrabActionToTake && showTriggerHint) {
            hand.HideGrabHint();
        }

        justPickedUpItem = false;
    }


    //-------------------------------------------------
    private void RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType packageType, Hand hand) {
        for (int i = 0; i < hand.AttachedObjects.Count; i++) {
            ItemPackageReference packageReference = hand.AttachedObjects[i].attachedObject.GetComponent<ItemPackageReference>();
            if (packageReference != null) {
                if (packageReference.itemPackage.packageType == packageType) {
                    GameObject detachedItem = hand.AttachedObjects[i].attachedObject;
                    hand.DetachObject(detachedItem);
                }
            }
        }
    }


    //-------------------------------------------------
    private void SpawnAndAttachObject(Hand hand, GrabTypes grabType) {

        if (showTriggerHint) {
            hand.HideGrabHint();
        }

        if (itemPackage.otherHandItemPrefab != null) {
            if (hand.otherHand.hoverLocked) {
                Debug.Log("<b>[SteamVR Interaction]</b> Not attaching objects because other hand is hoverlocked and we can't deliver both items.");
                return;
            }
        }

        // if we're trying to spawn a one-handed item, remove one and two-handed items from this hand and two-handed items from both hands
        if (itemPackage.packageType == ItemPackage.ItemPackageType.OneHanded) {
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand);
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand);
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand.otherHand);
        }

        // if we're trying to spawn a two-handed item, remove one and two-handed items from both hands
        if (itemPackage.packageType == ItemPackage.ItemPackageType.TwoHanded) {
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand);
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.OneHanded, hand.otherHand);
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand);
            RemoveMatchingItemTypesFromHand(ItemPackage.ItemPackageType.TwoHanded, hand.otherHand);
        }

        justPickedUpItem = true;

        spawnedItem = GameObject.Instantiate(itemPackage.itemPrefab);
        spawnedItem.SetActive(true);
        hand.AttachObject(spawnedItem, grabType, attachmentFlags);

        if ((itemPackage.otherHandItemPrefab != null) && (hand.otherHand.isActive)) {
            GameObject otherHandObjectToAttach = GameObject.Instantiate(itemPackage.otherHandItemPrefab);
            otherHandObjectToAttach.SetActive(true);
            hand.otherHand.AttachObject(otherHandObjectToAttach, grabType, attachmentFlags);
        }

    }
}
