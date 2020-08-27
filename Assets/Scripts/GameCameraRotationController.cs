using System;
using UnityEngine;

namespace Game
{
    public class GameCameraRotationController : MonoBehaviour
    {
        private Camera _camera;

        public GameObject CameraAnchor;
        public GameObject CameraContainerAnchor;
        
        public CameraModel CameraModel;
        
        private float _rotationSpeed;
        private bool _zoomed;
        private double _fovChangeTime;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Init()
        {
            InitRotationRadius();
            InitCameraHeight();
            InitRotationSpeed();
            InitFov();
        }

        private void InitFov()
        {
            _camera.fieldOfView = CameraModel.fovMax;
            _zoomed = false;
            _fovChangeTime = Time.time;
        }

        private void InitRotationSpeed()
        {
            _rotationSpeed = 360 / CameraModel.roundDuration;
        }

        private void InitCameraHeight()
        {
            var cameraTransform = _camera.transform;
            Vector3 cameraPosition = cameraTransform.position;
            cameraPosition.y = CameraModel.height;
            _camera.transform.SetPositionAndRotation(cameraPosition, cameraTransform.rotation);
        }

        private void InitRotationRadius()
        {
            Transform containerPosition = CameraContainerAnchor.transform;
            Vector3 cameraContainerPosition = containerPosition.position;
            cameraContainerPosition.x = CameraModel.roundRadius;
            containerPosition.SetPositionAndRotation(cameraContainerPosition, containerPosition.rotation);
        }

        private void Update()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            Transform cameraAnchorTransform = CameraAnchor.transform;
            Vector3 newRotation = cameraAnchorTransform.rotation.eulerAngles;
            newRotation.y += _rotationSpeed * Time.deltaTime;
            Quaternion newRotationQuatenion = Quaternion.Euler(newRotation);
            CameraAnchor.transform.SetPositionAndRotation(cameraAnchorTransform.position, newRotationQuatenion);
            
            _camera.transform.LookAt(new Vector3(0, CameraModel.lookAtHeight, 0));

            if (CanChangeFov()) {
                UpdateFOV();
            }
        }

        private void UpdateFOV()
        {
            float targetFov = _zoomed ? CameraModel.fovMax : CameraModel.fovMin;
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, CameraModel.fovDuration);
            
            if (Math.Abs(_camera.fieldOfView - targetFov) < 0.1f) {
                _zoomed = !_zoomed;
                _fovChangeTime = Time.time;
            }
        }

        private bool CanChangeFov()
        {
            return Time.time - _fovChangeTime > CameraModel.fovDelay;
        }
    }
}