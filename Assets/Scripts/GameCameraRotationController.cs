using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameCameraRotationController : MonoBehaviour
    {
        private const float ROAMING_SPEED = 0.1F;
        
        private Camera _camera;

        public GameObject CameraAnchor;
        public GameObject CameraContainerAnchor;
        
        public CameraModel CameraModel;
        
        private float _rotationSpeed;
        private bool _zoomed;
        private double _fovChangeTime;

        private Vector3 _targetRoaming;
        private Vector3 _initAnchorPosition;

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
            
            InitRoaming();
        }

        private void InitRoaming()
        {
            CreateTargetRoaming();
            
            _initAnchorPosition = CameraContainerAnchor.transform.localPosition;
        }

        private void CreateTargetRoaming()
        {
            _targetRoaming = new Vector3(Random.Range(-1f, 1f), 0,
                                         Random.Range(-1f, 1f));
            _targetRoaming *= CameraModel.roamingRadius;
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
            UpdateRoaming();
            
            RotateCamera();
        }

        private void UpdateRoaming()
        {
            Transform cameraAnchorTransform = CameraContainerAnchor.transform;
            Vector3 position = cameraAnchorTransform.localPosition;
            position.x = Mathf.Lerp(position.x, _initAnchorPosition.x + _targetRoaming.x, Time.deltaTime * CameraModel.roamingDuration * ROAMING_SPEED);
            position.y = Mathf.Lerp(position.y, _initAnchorPosition.y + _targetRoaming.y, Time.deltaTime * CameraModel.roamingDuration * ROAMING_SPEED);
            position.z = Mathf.Lerp(position.z, _initAnchorPosition.z + _targetRoaming.z, Time.deltaTime * CameraModel.roamingDuration * ROAMING_SPEED);
            cameraAnchorTransform.localPosition = position;

            if (Vector3.Distance(position, _initAnchorPosition + _targetRoaming) <= 0.1f) {
                CreateTargetRoaming();
            }
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