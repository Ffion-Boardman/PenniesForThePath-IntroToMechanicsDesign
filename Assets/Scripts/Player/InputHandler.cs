using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;
    private PlayerControls m_ActionMap;
    [SerializeField] private LayerMask m_InteractLayer;
    private void Awake()
    {
        m_ActionMap = new PlayerControls();
    }
    
    #region Bindings
    private void OnEnable()
    {
        m_ActionMap.Enable();

        m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled += Handle_JumpCanceled;
        m_ActionMap.Default.Interact.performed += Handle_InteractionPerformed;
    }

    private void Handle_JumpCanceled(InputAction.CallbackContext obj)
    {
        m_CharacterMovement.JumpCancelled();
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled -= Handle_JumpCanceled;
        m_ActionMap.Default.Interact.performed -= Handle_InteractionPerformed;
    }
    #endregion
    
    #region InputFunctions

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        m_CharacterMovement.SetInMove(context.ReadValue<float>());
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        m_CharacterMovement.SetInMove(0);
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        m_CharacterMovement.JumpPerformed();
    }

    private void Handle_InteractionPerformed(InputAction.CallbackContext context)
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 1, m_InteractLayer);

        if (col != null && col.transform.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact();
        }
    }
    #endregion
}
