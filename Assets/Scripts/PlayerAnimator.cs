using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
  private Animator animator;
  private SpriteRenderer spriteRenderer;

  void Awake() {
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void Animate(Vector2 moveVector, float moveSpeed) {
    var magnitude = moveVector.sqrMagnitude;
    animator.SetFloat("Horizontal", moveVector.x);
    animator.SetFloat("Vertical", moveVector.y);
    animator.SetFloat("Magnitude", magnitude);
    animator.SetFloat("Speed", Mathf.Clamp(magnitude * moveSpeed, 0.5f, 2.5f));
    if (magnitude > 0) {
      animator.SetFloat("LastHorizontal", moveVector.x);
      animator.SetFloat("LastVertical", moveVector.y);
      spriteRenderer.flipX = (moveVector.x > 0);
    }
  }
}
