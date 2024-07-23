using System;
using UnityEngine;

namespace Nevelson.Topdown2DPitfall.Assets.Scripts.Utils {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PitfallObject : MonoBehaviour, IPitfallCheck, IPitfall {
        [Tooltip("Controls how quickly the falling animations player")]
        public float pitfallAnimSpeed = .015f;
        [Tooltip("If you are respawning the object new the pit, " +
            "this value determines how far away from the pit this object will respawn")]
        public float respawnDistFromPit = 2;
        [Tooltip("If you supply a custom respawn location the pitfall object will " +
            "respawn here every time it falls. " +
            "This value can be changed at runtime.")]
        public Transform customRespawnLocation;
        private Action PitfallActionBefore;
        private Action PitfallActionAfter;
        private bool isFalling = false;
        private IPitfallCheck[] pitfallChecks;
        private IPitfallObject[] pitfallObjs;
        private Vector2 lastPosition = Vector2.zero;
        private Vector2 lastNonZeroMoveDirection = Vector2.zero;

        private void Start() {
            pitfallChecks = GetComponents<IPitfallCheck>();
            pitfallObjs = GetComponents<IPitfallObject>();
            AssignActions(true);
        }

        private void OnDestroy() {
            AssignActions(false);
        }

        private void Update() {
            DetermineRespawnLocation();
        }

        public void TriggerPitfall() {
            foreach (var pitfallCheck in pitfallChecks) {
                if (!pitfallCheck.PitfallConditionCheck()) return;
            }
            PitfallActionBefore();
            if (customRespawnLocation == null)
                StartCoroutine(UtilCoroutines.FallingCo(gameObject, PitfallActionAfter, pitfallAnimSpeed, GetDynamicRespawnLocation()));
            else {
                StartCoroutine(UtilCoroutines.FallingCo(gameObject, PitfallActionAfter, pitfallAnimSpeed, customRespawnLocation.position));
            }
        }

        public bool PitfallConditionCheck() {
            if (isFalling) return false;
            isFalling = true;
            return true;
        }

        private void AssignActions(bool enable) {
            if (enable) {
                PitfallActionAfter += PitfallActionAfter_ResetFallingCheck;
                foreach (var pitfallObj in pitfallObjs) {
                    PitfallActionBefore += pitfallObj.PitfallActionsBefore;
                    PitfallActionAfter += pitfallObj.PitfallResultingAfter;
                }
            } else {
                PitfallActionAfter -= PitfallActionAfter_ResetFallingCheck;
                foreach (var pitfallObj in pitfallObjs) {
                    PitfallActionBefore -= pitfallObj.PitfallActionsBefore;
                    PitfallActionAfter -= pitfallObj.PitfallResultingAfter;
                }
            }
        }

        private void DetermineRespawnLocation() {
            if (transform.GetPosition2D() != lastPosition) {
                Vector2 moveDirection = (transform.GetPosition2D() - lastPosition).normalized;
                if (moveDirection != Vector2.zero) lastNonZeroMoveDirection = moveDirection;
            }
            lastPosition = transform.GetPosition2D();
        }

        private Vector2 GetDynamicRespawnLocation() {
            return transform.GetPosition2D() +
                               -(lastNonZeroMoveDirection * respawnDistFromPit);
        }

        private void PitfallActionAfter_ResetFallingCheck() {
            isFalling = false;
        }
    }
}