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

  public void Animate(Vector2 moveVector, Vector2 lastVector, bool dash, float moveSpeed) {
    var magnitude = moveVector.sqrMagnitude;
    animator.SetFloat("Horizontal", moveVector.x);
    animator.SetFloat("Vertical", moveVector.y);
    animator.SetFloat("Magnitude", magnitude);
    animator.SetBool("Dash", dash);
    animator.SetFloat("Speed", Mathf.Clamp(magnitude * moveSpeed, 0.5f, 2.5f));
    if (magnitude > 0) {
      animator.SetFloat("LastHorizontal", lastVector.x);
      animator.SetFloat("LastVertical", lastVector.y);
      spriteRenderer.flipX = (lastVector.x > 0);
    }
  }
}
