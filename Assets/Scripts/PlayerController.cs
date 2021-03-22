using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
  [Tooltip("Movement speed multiplier for the player. Effects both the speed of movement and the speed of animations.")]
  public float moveSpeed = 1f;
  public float dashSpeed = 5f;
  public float dashTime = 0.1f;
  public float minimumMagnitude = 0.2f;

  private PlayerMovement playerMovement;
  private PlayerAnimator playerAnimator;
  private Vector2 moveVector = new Vector2(0, 0);
  private Vector2 lastVector = new Vector2(0, -1);
  private Vector2 dashVector = new Vector2(0, 0);
  private bool move;
  private bool dash;
  private float dashTimer;

  void Awake()
  {
    playerMovement = GetComponent<PlayerMovement>();
    playerAnimator = GetComponent<PlayerAnimator>();
  }

  void Update()
  {
    if (dash)
    {
      playerMovement.Move(dashVector, dashSpeed);
      dashTimer -= Time.deltaTime;
      if (dashTimer <= 0)
      {
        dash = false;
      }
    }
    else if (move)
    {
      playerMovement.Move(moveVector, moveSpeed);
    }
    playerAnimator.Animate(moveVector, lastVector, dash, moveSpeed);
  }

  public void OnMove(InputValue input)
  {
    var vector = input.Get<Vector2>();
    if (vector.sqrMagnitude > minimumMagnitude)
    {
      moveVector = vector;
      lastVector = vector;
      move = true;
    }
    else
    {
      moveVector = Vector2.zero;

    }
  }

  public void OnDash(InputValue input)
  {
    if (!dash)
    {
      dashVector = lastVector.normalized;
      dashTimer = dashTime;
      dash = true;
    }
  }

  public void OnInteract(InputValue input)
  {
  }
}
