using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
  [Tooltip("Movement speed multiplier for the player. Effects both the speed of movement and the speed of animations.")]
  public float moveSpeed = 1f;

  private PlayerMovement playerMovement;
  private PlayerAnimator playerAnimator;
  private Vector2 moveVector = new Vector2(0, 0);

  void Awake() {
    playerMovement = GetComponent<PlayerMovement>();
    playerAnimator = GetComponent<PlayerAnimator>();
  }

  void Update()
  {
    playerMovement.Move(moveVector, moveSpeed);
    playerAnimator.Animate(moveVector, moveSpeed);
  } 

  public void OnMove(InputValue input)
  {
    var vector = input.Get<Vector2>();
    if (vector.magnitude > 0.2) {
      moveVector = vector;
    } else {
      moveVector = Vector2.zero;
    }
  }

  public void OnInteract(InputValue input)
  {
  }
}
