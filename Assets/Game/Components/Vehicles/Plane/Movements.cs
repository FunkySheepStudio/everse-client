using UnityEngine;

namespace Game.Vehicles.Plane
{
    public class Movements : Game.Vehicles.Movements
    {
        public override void Move(float deltaTime)
        {
            Vector3 direction = transform.forward * inputManager.Current.movement.normalized.y + transform.right * inputManager.Current.movement.normalized.x;
            characterController.Move(transform.forward * deltaTime * 100);
            Rotate(deltaTime);
        }

        private void Rotate(float deltaTime)
        {

            float _targetRotationX = Mathf.Lerp(transform.parent.rotation.x, inputManager.Current.look.normalized.x, deltaTime * 300);
            float _targetRotationZ = Mathf.Lerp(transform.parent.rotation.z, inputManager.Current.look.normalized.y, deltaTime * 300);
            transform.parent.Rotate(new Vector3(0, _targetRotationX, _targetRotationZ));
        }
    }

}
