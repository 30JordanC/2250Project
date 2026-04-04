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

    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    
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
            
            animator1.SetTrigger("Interact");
            animator2.SetTrigger("Interact");
            animator3.SetTrigger("Interact");
        }
    }

    void CheckInteractable()
    {
        currentInteractable = null;
        
        Vector3 boxCenter = interactOrigin.position + orientation.forward * forwardOffset;
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfDimensions, orientation.rotation,  interactableMask);

        if (hits.Length > 0)
        {
            IInteractable interactable = hits[0].GetComponent<IInteractable>();

            if (interactable != null && interactable.CanInteract())
            {
                currentInteractable = interactable;

                interactText.gameObject.SetActive(true);
                interactText.text = interactable.GetInteractText();

                return;
            }
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
