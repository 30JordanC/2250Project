using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public Transform interactOrigin;
    public Transform orientation;

    public Vector3 boxHalfDimensions;
    public float forwardOffset;

    public LayerMask interactableMask;
    private IInteractable currentInteractable;

    public TMPro.TMP_Text interactText;

    public Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractable();
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null) 
        {
            currentInteractable.Interact();
            
            animator.SetTrigger("Interact");
        }
    }

    void CheckInteractable()
    {
        currentInteractable = null;
        
        Vector3 boxCenter = interactOrigin.position + orientation.forward * forwardOffset;
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfDimensions, orientation.rotation,  interactableMask);

        if (hits.Length > 0)
        {
            interactText.gameObject.SetActive(true);
            currentInteractable = hits[0].GetComponent<IInteractable>();
            return;
        }
        
        interactText.gameObject.SetActive(false);
    }
    void OnDrawGizmosSelected()
    {
        if (interactOrigin == null || orientation == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(
            interactOrigin.position + orientation.forward * forwardOffset,
            orientation.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, boxHalfDimensions * 2f);
    }
}
