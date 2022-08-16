using UnityEngine;

namespace Game.Vehicles.Plane
{
    public class Movements : Game.Vehicles.Movements
    {
        private void Awake()
        {
            speed = 200;
        }

        public override void Move(float deltaTime)
        {
            Rotate(deltaTime);
            Vector3 direction = transform.forward * inputManager.Current.movement.normalized.y + transform.right * inputManager.Current.movement.normalized.x;
            characterController.Move(transform.forward * deltaTime * speed);
        }

        private void Rotate(float deltaTime)
        {

            float _targetRotationX = -inputManager.Current.movement.normalized.x * 600;
            float _targetRotationZ  =inputManager.Current.movement.normalized.y * 600;
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, Quaternion.Euler(transform.parent.eulerAngles.x + _targetRotationZ, transform.parent.eulerAngles.y + _targetRotationX, 0), deltaTime);
        }
    }

}
