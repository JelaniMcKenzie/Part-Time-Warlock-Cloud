using UnityEngine;
namespace Nevelson.Topdown2DPitfall.Assets.Scripts.Utils {
    public class Pitfall : MonoBehaviour {
        [Tooltip("If check, all bounds will move with the parent object." +
            "Useful for moving pitfalls like sinkholes")]
        public bool parentBoundsToPitfallManager = false;
        public Bounds[] bounds;
        private LayerMask whatIsPitfall;
        private Collider2D[] results = new Collider2D[1];
        private int pitfallObjCount = 0;
        private Vector3 currentPos;

        private void Start() {
            currentPos = transform.position;
            whatIsPitfall = LayerMask.GetMask(Constants.PITFALL_COLLIDER);
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            foreach (var bound in bounds) {
                Gizmos.DrawCube(bound.center, bound.size);
            }
        }

        private void Update() {
            if (parentBoundsToPitfallManager)
                CheckPitfallPositionsSynced();
            TriggerPitfallOnPitfallColliders();
        }

        private void CheckPitfallPositionsSynced() {
            if (transform.position != currentPos) {
                for (int i = 0; i < bounds.Length; i++) {
                    bounds[i].center += (transform.position - currentPos);
                }
                currentPos = transform.position;
            }
        }

        private void TriggerPitfallOnPitfallColliders() {
            foreach (var bound in bounds) {
                pitfallObjCount = Physics2D.OverlapBoxNonAlloc(bound.center, bound.extents * 2, 0, results, whatIsPitfall);
                if (pitfallObjCount > 0) {
                    Collider2D[] objsInPit = Physics2D.OverlapBoxAll(bound.center, bound.extents * 2, 0, whatIsPitfall);
                    foreach (var collider in objsInPit) {
                        IPitfall pitfall = GetIPitfall(collider);
                        if (pitfall != null) {
                            pitfall.TriggerPitfall();
                        } else Debug.LogError("Attempting to trigger pitfall that doesn't exist, check GetIPitfall method");
                    }
                }
            }
        }

        private IPitfall GetIPitfall(Collider2D collision) {
            return collision.GetComponentInParent<IPitfall>();
        }
    }
}