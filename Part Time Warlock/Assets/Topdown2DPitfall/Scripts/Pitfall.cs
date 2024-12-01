using UnityEngine;
namespace Nevelson.Topdown2DPitfall.Assets.Scripts.Utils
{
    public class Pitfall : MonoBehaviour
    {
        [Tooltip("If checked, all bounds will move with the parent object. Useful for moving pitfalls like sinkholes")]
        public bool parentBoundsToPitfallManager = false;
        public Bounds[] bounds;
        private LayerMask whatIsPitfall;
        private Collider2D[] results = new Collider2D[1];
        private int pitfallObjCount = 0;
        private Vector3 currentPos;

        private void Start()
        {
            currentPos = transform.position;
            whatIsPitfall = LayerMask.GetMask(Constants.PITFALL_COLLIDER);
            //CreateBoundaries();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (var bound in bounds)
            {
                // Draw the bounds in the local space transformed to world space
                Gizmos.DrawCube(transform.TransformPoint(bound.center), bound.size);
            }
        }

        private void Update()
        {
            if (parentBoundsToPitfallManager)
                CheckPitfallPositionsSynced();
            TriggerPitfallOnPitfallColliders();
        }

        private void CheckPitfallPositionsSynced()
        {
            if (transform.position != currentPos)
            {
                for (int i = 0; i < bounds.Length; i++)
                {
                    bounds[i].center += (transform.position - currentPos);
                }
                currentPos = transform.position;
            }
        }

        private void TriggerPitfallOnPitfallColliders()
        {
            foreach (var bound in bounds)
            {
                Vector3 worldCenter = transform.TransformPoint(bound.center);  // Convert local bound center to world space
                pitfallObjCount = Physics2D.OverlapBoxNonAlloc(worldCenter, bound.extents * 2, 0, results, whatIsPitfall);
                if (pitfallObjCount > 0)
                {
                    Collider2D[] objsInPit = Physics2D.OverlapBoxAll(worldCenter, bound.extents * 2, 0, whatIsPitfall);
                    foreach (var collider in objsInPit)
                    {
                        IPitfall pitfall = GetIPitfall(collider);
                        if (pitfall != null)
                        {
                            pitfall.TriggerPitfall();
                        }
                        else Debug.LogError("Attempting to trigger pitfall that doesn't exist, check GetIPitfall method");
                    }
                }
            }
        }

        private IPitfall GetIPitfall(Collider2D collision)
        {
            return collision.GetComponentInParent<IPitfall>();
        }

        private void CreateBoundaries()
        {
            foreach (var bound in bounds)
            {
                CreateBoxCollider(bound);
            }
        }

        private void CreateBoxCollider(Bounds bound)
        {
            GameObject boundary = new GameObject("Boundary");
            boundary.transform.SetParent(transform);
            boundary.layer = LayerMask.NameToLayer("PitBorder");

            BoxCollider2D boxCollider = boundary.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector3((bound.size.x * 1.076923f), (bound.size.y * 1.333333333333333333333f), 0f);

            // Set the position of the boundary object relative to the parent
            boundary.transform.localPosition = bound.center;

            // Offset is no longer needed as we're setting the localPosition directly
            boxCollider.offset = Vector2.zero;
        }
        
    }
}
