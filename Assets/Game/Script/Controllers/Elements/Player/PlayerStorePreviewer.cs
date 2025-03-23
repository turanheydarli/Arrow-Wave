using UnityEngine;

namespace Game.Script.Controllers.Elements.Player
{
    public class PlayerStorePreviewer : MonoBehaviour
    {
        private Vector3 _defaultRotation;
        private bool _isDragging;
        private Vector2 _lastMousePosition;
        [SerializeField] private float _baseRotationSpeed = 5f;
        private bool _isReturning;
        private float _returnSpeed = 200f;

        private float _currentRotationSpeed;
        private float _dragAcceleration = 2f;
        private float _maxRotationSpeed = 15f;

        private float _idleAmplitude = 5f;
        private float _idleSpeed = 1f;

        private void Start()
        {
            _defaultRotation = transform.eulerAngles;
            _currentRotationSpeed = _baseRotationSpeed;
        }

        private void Update()
        {
            if (_isDragging)
            {
                HandleDragging();
            }
            else
            {
                _currentRotationSpeed = _baseRotationSpeed;

                if (_isReturning)
                {
                    HandleReturnToDefault();
                }
                else
                {
                    HandleIdleMovement();
                }
            }
        }

        public void StartDragging(UnityEngine.EventSystems.PointerEventData eventData)
        {
            _isDragging = true;
            _isReturning = false;
            _lastMousePosition = eventData.position;
            _currentRotationSpeed = _baseRotationSpeed;
        }

        public void StopDragging()
        {
            _isDragging = false;
            _isReturning = true;
        }

        private void HandleDragging()
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 difference = currentMousePosition - _lastMousePosition;

            float mouseDelta = Mathf.Abs(difference.x);
            if (mouseDelta > 1f)
            {
                _currentRotationSpeed = Mathf.Min(
                    _currentRotationSpeed + (_dragAcceleration * Time.deltaTime * mouseDelta),
                    _maxRotationSpeed
                );
            }

            float rotationAmount = -difference.x * _currentRotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount, Space.World);

            _lastMousePosition = currentMousePosition;
        }

        private void HandleReturnToDefault()
        {
            Quaternion targetRotation = Quaternion.Euler(_defaultRotation);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _returnSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                _isReturning = false;
            }
        }

        private void HandleIdleMovement()
        {
            float idleOffset = Mathf.Sin(Time.time * _idleSpeed) * _idleAmplitude;
            transform.rotation = Quaternion.Euler(
                _defaultRotation.x,
                _defaultRotation.y + idleOffset,
                _defaultRotation.z
            );
        }

        public void ResetRotation()
        {
            _isDragging = false;
            _isReturning = true;
            _currentRotationSpeed = _baseRotationSpeed;
        }
    }
}