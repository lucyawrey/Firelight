using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
  [Tooltip("Layers to test for collisions. The Player will walk through anything not on these layers.")]
  public LayerMask collisionLayers;

  private CircleCollider2D circleCollider2D;
  private RaycastHit2D[] hits2D = new RaycastHit2D[2];

  void Awake()
  {
    circleCollider2D = GetComponent<CircleCollider2D>();
  }

  /// <summary>Attempts to move the controller by <paramref name="motion"/>, the motion will only be constained by collisions. It will slide along colliders.</summary>
  /// <param name="motion">The desired motion from the current position.</param>
  public void Move(Vector2 moveVector, float speed)
  {
    Vector3 movePosition = GetValidPosition(moveVector * speed * Time.deltaTime * 5, 10f, 20f, 40f, 60f, 80f) - circleCollider2D.offset;
    movePosition.z = transform.position.z;
    transform.position = movePosition;
  }

  /// <summary>Returns a valid Vector2 position based on <paramref name="motion"/> from the current position. Collisions are tested for in both <paramref name="motion"/> + and - <paramref name="testAngles"/>.</summary>
  /// <returns>The valid Vector2 position to move to.</returns>
  /// <param name="motion">The desired motion from the current position.</param>
  /// <param name="testAngles">Each angle to test in both the positive and negative z rotation, relative to <paramref name="motion"/>.</param>
  public Vector2 GetValidPosition(Vector2 motion, params float[] testAngles)
  {
    var movementDirection = motion.normalized * circleCollider2D.radius;
    var targetPosition = (Vector2)transform.position + motion + circleCollider2D.offset;

    // Test the desired direction for a collision and update targetPosition if any is found
    targetPosition += GetValidDirectionAdjustment(targetPosition, movementDirection);

    // Test movementDirection + and - each testAngle for a collision and update targetPosition if any is found
    for (int i = 0; i < testAngles.Length; i++)
    {
      targetPosition += GetValidDirectionAdjustment(targetPosition, Quaternion.Euler(0, 0, testAngles[i]) * movementDirection);
      targetPosition += GetValidDirectionAdjustment(targetPosition, Quaternion.Euler(0, 0, -testAngles[i]) * movementDirection);
    }
    return targetPosition;
  }

  /// <summary>Tests <paramref name="direction"/> from <paramref name="targetPosition"/> + <paramref name="radius"/> for a collision. If one is found, returns a Vector2 adjustment to the closest valid position. Otherwise returns Vector2.zero.</summary>
  /// <returns>The closest valid position to <paramref name="targetPosition"/>.</returns>
  /// <param name="targetPosition">The desired position to move to.</param>
  /// <param name="direction">The direction to test for a collision.</param>
  private Vector2 GetValidDirectionAdjustment(Vector2 targetPosition, Vector2 direction)
  {
    var validPositionAdjustment = Vector2.zero;

    int amountOfHits = Physics2D.RaycastNonAlloc(targetPosition, direction, hits2D, circleCollider2D.radius, collisionLayers);
    RaycastHit2D hit2D;

    /// Because the character has a collider, to ensure we can collide with other characters if desired
    /// we need to allow for up to two hit detections. One for the character's collider and the other 
    /// for our actual collision.
    if (amountOfHits == 0 || (amountOfHits == 1 && hits2D[0].fraction < 0.5f))
    {
      // We hit nothing, or we only hit ourselves
      return validPositionAdjustment;
    }
    else if (amountOfHits == 1 || (amountOfHits > 1 && hits2D[0].fraction > 0.5f))
    {
      // We hit one of more colliders, but none of them was ours
      hit2D = hits2D[0];
    }
    else
    {
      // We hit ourselves, but we also hit something else.
      hit2D = hits2D[1];
    }

    validPositionAdjustment = hit2D.normal.normalized * ((1.0f - hit2D.fraction) * circleCollider2D.radius);

    return validPositionAdjustment;
  }
}
