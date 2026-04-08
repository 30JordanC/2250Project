using UnityEngine;

public class PlayerRockHandler : MonoBehaviour
{
    [Header("Holding")]
    [SerializeField] private Transform pebbleHoldPoint;
    [SerializeField] private Pebble heldPebble;

    [Header("Direction")]
    [SerializeField] private Transform orientation;

    [Header("Throwing")]
    [SerializeField] private float throwForce = 18f;
    [SerializeField] private float upwardForce = 5f;
    [SerializeField] private float throwSpawnForwardOffset = 1.2f;
    [SerializeField] private float throwSpawnUpOffset = 0.5f;

    [Header("Dropping")]
    [SerializeField] private float dropForwardOffset = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayHeight = 3f;
    [SerializeField] private float groundRayDistance = 10f;

    private void Update()
    {
        if (heldPebble == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ThrowPebble();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DropPebble();
        }
    }

    public bool IsHoldingPebble()
    {
        return heldPebble != null;
    }

    public void PickUpPebble(Pebble pebble)
    {
        if (heldPebble != null || pebble == null || pebbleHoldPoint == null)
            return;

        heldPebble = pebble;
        heldPebble.SetHeld(pebbleHoldPoint);
    }

    private void ThrowPebble()
    {
        if (heldPebble == null || orientation == null)
            return;

        Pebble pebbleToThrow = heldPebble;
        heldPebble = null;

        pebbleToThrow.Release();

        Vector3 flatForward = orientation.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        pebbleToThrow.transform.position =
            transform.position +
            flatForward * throwSpawnForwardOffset +
            Vector3.up * throwSpawnUpOffset;

        Rigidbody rb = pebbleToThrow.GetRigidbody();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(flatForward * throwForce + Vector3.up * upwardForce, ForceMode.Impulse);
        }

        PebbleNoise pebbleNoise = pebbleToThrow.GetComponent<PebbleNoise>();
        if (pebbleNoise != null)
        {
            pebbleNoise.BeginTrackingLanding();
        }
    }

    private void DropPebble()
    {
        if (heldPebble == null || orientation == null)
            return;

        Pebble pebbleToDrop = heldPebble;
        heldPebble = null;

        pebbleToDrop.Release();

        Vector3 flatForward = orientation.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        Vector3 targetPosition = transform.position + flatForward * dropForwardOffset;

        if (Physics.Raycast(transform.position + Vector3.up * groundRayHeight, Vector3.down, out RaycastHit hit, groundRayDistance, groundLayer))
        {
            targetPosition.y = hit.point.y;
        }
        else
        {
            targetPosition.y = 0f;
        }

        pebbleToDrop.transform.position = targetPosition;

        Rigidbody rb = pebbleToDrop.GetRigidbody();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}