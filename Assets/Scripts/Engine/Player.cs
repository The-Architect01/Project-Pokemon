using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Trainer Trainer;
    public Animator Animator;
    public float Speed = .015f;
    public static bool CanMove { get; set; } = true;

    // Start is called before the first frame update
    void Start() {
        if (Engine.PlayerSaveData == null) { } else {
            Animator.SetBool("IsMale", Engine.IsLucas);
        }
    }

    void Update() {
        if (CanMove) {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) { //Up
                Animator.SetBool("IsWalking", true);
                Animator.SetInteger("Direction", 2);
                transform.Translate(0f, Speed, 0f);
                return;
            } else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) { //Down
                Animator.SetBool("IsWalking", true);
                Animator.SetInteger("Direction", 0);
                transform.Translate(0f, -Speed, 0f);
                return;
            } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) { //Left
                Animator.SetBool("IsWalking", true);
                Animator.SetInteger("Direction", 3);
                transform.Translate(-Speed, 0f, 0f);
                return;
            } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) { //Right
                Animator.SetBool("IsWalking", true);
                transform.Translate(Speed, 0f, 0f);
                Animator.SetInteger("Direction", 1);
                return;
            }
        } if (Input.GetKey(KeyCode.Space)) { //Interaction
            Debug.Log("Interaction");
            Animator.SetBool("IsWalking", false);
            return;
        }
    }
    private void LateUpdate() {
        Animator.SetBool("IsWalking", false);
    }

}
