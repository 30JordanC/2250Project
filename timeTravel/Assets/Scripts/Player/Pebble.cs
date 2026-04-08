using System.Collections.Generic;
using UnityEngine;

public class Pebble : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer[] renderers;
    
    [Header("Prompt")]
    [SerializeField] private string interactText = "Press E to pick up pebble";

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider pebbleCollider;

    private bool isHeld = false;
    private readonly List<Collider> ignoredPlayerColliders = new List<Collider>();

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (pebbleCollider == null)
            pebbleCollider = GetComponent<Collider>();

        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        IgnorePlayerCollisionPermanently();
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        PlayerRockHandler playerRockHandler = FindFirstObjectByType<PlayerRockHandler>();
        if (playerRockHandler == null)
            return;

        playerRockHandler.PickUpPebble(this);
    }

    public bool CanInteract()
    {
        if (isHeld)
            return false;

        PlayerRockHandler playerRockHandler = FindFirstObjectByType<PlayerRockHandler>();
        if (playerRockHandler == null)
            return false;

        return !playerRockHandler.IsHoldingPebble();
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void SetHeld(Transform holdPoint)
    {
        isHeld = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (pebbleCollider != null)
            pebbleCollider.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        SetVisuals(false);
    }

    public void Release()
    {
        isHeld = false;
        transform.SetParent(null);

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (pebbleCollider != null)
            pebbleCollider.enabled = true;

        ReapplyIgnoredPlayerCollisions();
        
        SetVisuals(true);
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    private void IgnorePlayerCollisionPermanently()
    {
        if (pebbleCollider == null)
            return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
            return;

        Collider[] playerColliders = playerObject.GetComponentsInChildren<Collider>();

        ignoredPlayerColliders.Clear();

        foreach (Collider playerCol in playerColliders)
        {
            if (playerCol == null)
                continue;

            Physics.IgnoreCollision(pebbleCollider, playerCol, true);
            ignoredPlayerColliders.Add(playerCol);
        }
    }

    private void ReapplyIgnoredPlayerCollisions()
    {
        if (pebbleCollider == null)
            return;

        foreach (Collider playerCol in ignoredPlayerColliders)
        {
            if (playerCol == null)
                continue;

            Physics.IgnoreCollision(pebbleCollider, playerCol, true);
        }
    }
    
    private void SetVisuals(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            if (r != null)
                r.enabled = visible;
        }
    }
}