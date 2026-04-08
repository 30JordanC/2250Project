using System.Collections;
using UnityEngine;

public class PebbleNoise : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float rayStartHeight = 0.5f;
    [SerializeField] private float rayDistance = 1.5f;

    [Header("Landing")]
    [SerializeField] private float minAirTime = 0.1f; // prevents instant detection

    private Coroutine landingRoutine;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    public void BeginTrackingLanding()
    {
        if (landingRoutine != null)
        {
            StopCoroutine(landingRoutine);
        }

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        landingRoutine = StartCoroutine(TrackLandingRoutine());
    }

    private IEnumerator TrackLandingRoutine()
    {
        // Small delay so it doesn't instantly detect ground when thrown
        yield return new WaitForSeconds(minAirTime);

        while (true)
        {
            if (rb == null)
                yield break;

            // Only check when falling or slow
            if (rb.linearVelocity.y <= 0.1f)
            {
                Vector3 rayOrigin = transform.position + Vector3.up * rayStartHeight;

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, whatIsGround))
                {
                    FinalizeLanding(hit.point);
                    yield break;
                }
            }

            yield return null;
        }
    }

    private void FinalizeLanding(Vector3 groundPoint)
    {
        if (rb != null)
        {
            // Snap to ground
            Vector3 pos = transform.position;
            pos.y = groundPoint.y;
            transform.position = pos;

            // Kill all movement
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Freeze completely
            rb.constraints = RigidbodyConstraints.FreezeAll;
            
            Debug.Log("Pebble landed at: " + groundPoint);
        }

        NotifyTRex();
    }

    private void NotifyTRex()
    {
        Rex[] allTRex = FindObjectsByType<Rex>(FindObjectsSortMode.None);

        foreach (Rex trex in allTRex)
        {
            trex.NotifyRockLanded(transform.position);
        }
    }
}