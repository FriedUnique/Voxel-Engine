using GameEngine.Core.Utilities.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace GameEngine.Core.Rendering {
    public class Camera {
        public static float aspectRatio;

        // Those vectors are directions pointing outwards from the camera to define how it rotated.
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;

        // in radians
        private float _pitch;
        private float _yaw = -MathHelper.PiOver2; // Without this, you would be started rotated 90 degrees right.
        private float _fov = MathHelper.PiOver2;

        private readonly float speed = 5f;
        private readonly float sens = 0.2f;
        private readonly float nearPlane = 0.01f;
        private readonly float farPlane = 100f;

        public Frustum frustum;

        public Camera(Vector3 position) {
            Position = position;

            frustum = new Frustum();
            frustum.CalculateFrustum(GetProjectionMatrix(), GetViewMatrix());
        }
        public Camera(Vector3 position, float speed, float sensitivity) : this(position) {
            this.speed = speed;
            sens = sensitivity;
        }
        public Camera(Vector3 position, float speed, float sensitivity, float near, float far) : this(position, speed, sensitivity) {
            nearPlane = near;
            farPlane = far;
        }

        public Vector3 Position { get; set; }
        public Vector3 Front { get { return _front; } }
        public Vector3 Up { get { return _up; } }
        public Vector3 Right { get { return _right; } }


        // directly convert degrees to rad on set
        public float Pitch {
            get => MathHelper.RadiansToDegrees(_pitch);
            set {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw {
            get => MathHelper.RadiansToDegrees(_yaw);
            set {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float Fov {
            get => MathHelper.RadiansToDegrees(_fov);
            set {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Matrix4 GetViewMatrix() {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }
        public Matrix4 GetProjectionMatrix() {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, aspectRatio, nearPlane, farPlane);
        }

        private void UpdateVectors() {
            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);


            // normalized vectors
            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public void MoveCamera(float dt) {
            if (Input.IsKeyDown(Keys.W)) {
                Position += Front * speed * dt; // Forward
            }
            if (Input.IsKeyDown(Keys.S)) {
                Position -= Front * speed * dt; // Backwards
            }
            if (Input.IsKeyDown(Keys.A)) {
                Position -= Right * speed * dt; // Left
            }
            if (Input.IsKeyDown(Keys.D)) {
                Position += Right * speed * dt; // Right
            }
            if (Input.IsKeyDown(Keys.Space)) {
                Position += Vector3.UnitY * speed * dt; // Up
            }
            if (Input.IsKeyDown(Keys.LeftShift)) {
                Position -= Vector3.UnitY * speed * dt; // Down
            }



            Yaw += Input.MouseDelta("x") * sens;
            Pitch -= Input.MouseDelta("y") * sens;
        }

        public void CalculateFrustum() {
            frustum.CalculateFrustum(GetProjectionMatrix(), GetViewMatrix());
        }
    }
}