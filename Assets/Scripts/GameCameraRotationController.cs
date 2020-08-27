using UnityEngine;

namespace Game
{
    public class GameCameraRotationController : MonoBehaviour
    {
        private Camera _camera;

        public GameObject CameraAnchor;
        public CameraModel CameraModel;
        
        private float _rotationSpeed;

        private void Awake()
        {
            _camera = Camera.main;

            PrepareParams();
        }

        private void PrepareParams()
        {
            _rotationSpeed = 0.1f;
        }

        private void Update()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            Transform cameraAnchorTransform = CameraAnchor.transform;
            Vector3 newRotation = cameraAnchorTransform.rotation.eulerAngles;
            newRotation.y += _rotationSpeed;
            CameraAnchor.transform.SetPositionAndRotation(cameraAnchorTransform.position, Quaternion.Euler(newRotation));
        }
    }
}