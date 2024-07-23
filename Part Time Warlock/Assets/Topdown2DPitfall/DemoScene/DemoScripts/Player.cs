using Nevelson.Topdown2DPitfall.Assets.Scripts.Utils;
using UnityEngine;

public class Player : MonoBehaviour, IPitfallCheck, IPitfallObject {
    public float speed = 7f;
    private Rigidbody2D rb;
    private bool isMovable = true;
    private Vector2 moveVelocity;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (!isMovable) {
            MovementInput(Vector2.zero);
            return;
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MovementInput(input);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void MovementInput(Vector2 moveInput) {
        moveVelocity = moveInput.normalized * speed;
    }

    public bool PitfallConditionCheck() {
        return true;
    }

    public void PitfallActionsBefore() {
        isMovable = false;
    }

    public void PitfallResultingAfter() {
        isMovable = true;
    }
}
