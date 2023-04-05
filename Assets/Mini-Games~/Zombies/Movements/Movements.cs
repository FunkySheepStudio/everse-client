using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WIP.Zombies
{
    public class Movements : MonoBehaviour
    {
        CharacterController characterController;
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Move();
        }

        public void Move()
        {
            characterController.SimpleMove(transform.forward * 5);
        }
    }
}
