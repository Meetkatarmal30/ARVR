using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one => new Vector3(1, 1, 1);
        public static Vector3 zero => new Vector3(0, 0, 0);
        public static Vector3 forward => new Vector3(0, 0, 1);
        public static Vector3 right => new Vector3(1, 0, 0);
        public static Vector3 up => new Vector3(0, 1, 0);

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator *(float d, Vector3 a) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator /(Vector3 a, float d) => new Vector3(a.x / d, a.y / d, a.z / d);

        public float sqrMagnitude => x * x + y * y + z * z;
        public float magnitude => (float)Math.Sqrt(sqrMagnitude);
        public Vector3 normalized => magnitude > 1e-5f ? this / magnitude : zero;

        public static float Distance(Vector3 a, Vector3 b) => (a - b).magnitude;
        public static float Dot(Vector3 a, Vector3 b) => a.x * b.x + a.y * b.y + a.z * b.z;
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * Mathf.Clamp(t, 0f, 1f);

        public override string ToString() => $"({x:F2}, {y:F2}, {z:F2})";
    }

    public struct Color
    {
        public float r, g, b, a;
        public Color(float r, float g, float b, float a = 1f) { this.r = r; this.g = g; this.b = b; this.a = a; }
        public static Color blue => new Color(0, 0, 1);
        public static Color red => new Color(1, 0, 0);
        public static Color green => new Color(0, 1, 0);
        public static Color black => new Color(0, 0, 0);
        public static Color white => new Color(1, 1, 1);
        public static Color operator *(Color c, float f) => new Color(c.r * f, c.g * f, c.b * f, c.a * f);
    }

    public struct Quaternion
    {
        public float x, y, z, w;
        public static Quaternion identity => new Quaternion { x = 0, y = 0, z = 0, w = 1 };
        public static Quaternion Euler(float x, float y, float z) => identity;
        public static Quaternion operator *(Quaternion a, Quaternion b) => identity;
        public static float Angle(Quaternion a, Quaternion b) => 0f;
        public static Quaternion Slerp(Quaternion a, Quaternion b, float t) => b;
    }

    public struct Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public Ray(Vector3 o, Vector3 d) { origin = o; direction = d; }
    }

    public struct RaycastHit
    {
        public Collider collider;
        public Vector3 point;
        public float distance;
    }

    public struct LayerMask
    {
        public int value;
        public static implicit operator LayerMask(int intVal) => new LayerMask { value = intVal };
        public static implicit operator int(LayerMask mask) => mask.value;
    }

    public static class Physics
    {
        public static bool Raycast(Ray ray, out RaycastHit hit, float maxDistance, int layerMask = ~0)
        {
            hit = default(RaycastHit);
            return false;
        }
    }

    public static class Mathf
    {
        public const float PI = (float)Math.PI;
        public static float Sin(float f) => (float)Math.Sin(f);
        public static float Exp(float power) => (float)Math.Exp(power);
        public static float Clamp(float value, float min, float max) => value < min ? min : (value > max ? max : value);
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * Clamp(t, 0f, 1f);
    }

    public static class Random
    {
        private static System.Random rand = new System.Random();
        public static float Range(float min, float max) => min + (float)rand.NextDouble() * (max - min);
    }

    public static class Time
    {
        public static float deltaTime => 0.016f;
    }

    public enum CursorLockMode
    {
        None = 0,
        Locked = 1,
        Confined = 2
    }

    public static class Cursor
    {
        public static CursorLockMode lockState { get; set; }
        public static bool visible { get; set; }
    }

    public enum KeyCode
    {
        Escape, Tab, M, E, Space, LeftShift
    }

    public static class Input
    {
        public static bool GetKeyDown(KeyCode key) => false;
        public static bool GetKey(KeyCode key) => false;
        public static bool GetMouseButtonDown(int button) => false;
        public static float GetAxis(string axisName) => 0f;
        public static float GetAxisRaw(string axisName) => 0f;
    }

    public static class Debug
    {
        public static void Log(object message) => Console.WriteLine(message);
        public static void LogWarning(object message) => Console.WriteLine(message);
        public static void LogError(object message) => Console.WriteLine(message);
    }

    public static class Application
    {
        public static void Quit() { }
    }

    public class Attribute { }
    public class HeaderAttribute : System.Attribute { public HeaderAttribute(string h) { } }
    public class TextAreaAttribute : System.Attribute { public TextAreaAttribute(int min, int max) { } }
    public class RangeAttribute : System.Attribute { public RangeAttribute(float min, float max) { } }
    public class SerializeField : System.Attribute { }
    public class RequireComponent : System.Attribute { public RequireComponent(Type t) { } }

    public class Object
    {
        public string name { get; set; }
        public static void Destroy(Object obj) { }
        public static T Instantiate<T>(T original, object parent = null) where T : Object => original;
    }

    public class Component : Object
    {
        public Transform transform => new Transform();
        public GameObject gameObject => new GameObject();
        public T GetComponent<T>() => default(T);
        public T GetComponentInChildren<T>() => default(T);
        public T GetComponentInParent<T>() => default(T);
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localPosition { get; set; }
        public Vector3 localScale { get; set; } = Vector3.one;
        public Vector3 forward => Vector3.forward;
        public Vector3 right => Vector3.right;
        public Quaternion rotation { get; set; }
        public Quaternion localRotation { get; set; }
        public void Rotate(Vector3 euler) { }
        public void SetParent(Transform parent) { }
    }

    public class GameObject : Object
    {
        public GameObject() { }
        public GameObject(string name) { this.name = name; }
        public Transform transform => new Transform();
        public bool activeSelf { get; set; } = true;
        public void SetActive(bool active) { activeSelf = active; }
        public bool activeInHierarchy => activeSelf;
        public T AddComponent<T>() where T : Component, new() => new T();
        public T GetComponent<T>() => default(T);
    }

    public class Coroutine { }

    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) { }
    }

    public class MonoBehaviour : Component
    {
        public static void print(object msg) => Console.WriteLine(msg);
        public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine coroutine) { }
        public static T FindObjectOfType<T>() where T : class => default(T);
    }

    public class Collider : Component { }

    public class CharacterController : Collider
    {
        public bool isGrounded => true;
        public bool enabled { get; set; } = true;
        public void Move(Vector3 motion) { }
    }

    public class Material : Object
    {
        public Color color { get; set; }
        public bool HasProperty(string prop) => true;
        public void SetColor(string prop, Color color) { }
        public void EnableKeyword(string keyword) { }
    }

    public class Renderer : Component
    {
        public Material material => new Material();
    }

    public class Light : Component
    {
        public bool enabled { get; set; } = true;
    }

    public class Camera : Component
    {
        public static Camera main => new Camera();
        public Ray ViewportPointToRay(Vector3 pos) => new Ray(Vector3.zero, Vector3.forward);
    }

    public class AudioClip : Object
    {
        public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream) => new AudioClip();
        public void SetData(float[] data, int offsetSamples) { }
    }

    public class AudioSource : Component
    {
        public AudioClip clip { get; set; }
        public bool loop { get; set; }
        public bool playOnAwake { get; set; }
        public float volume { get; set; }
        public float pitch { get; set; } = 1.0f;
        public float spatialBlend { get; set; } = 0f;
        public void Play() { }
        public void PlayOneShot(AudioClip clip, float scale = 1f) { }
    }

    namespace UI
    {
        public class Graphic : Component
        {
            public Color color { get; set; }
        }

        public class Image : Graphic { }

        public class Text : Graphic
        {
            public string text { get; set; }
        }

        public class ButtonClickedEvent
        {
            private List<Action> actions = new List<Action>();
            public void AddListener(Action call) => actions.Add(call);
            public void RemoveAllListeners() => actions.Clear();
            public void Invoke() { foreach (var a in actions) a(); }
        }

        public class Button : Component
        {
            public ButtonClickedEvent onClick { get; set; } = new ButtonClickedEvent();
        }

        public class InputFieldValueChangedEvent
        {
            private List<Action<string>> actions = new List<Action<string>>();
            public void AddListener(Action<string> call) => actions.Add(call);
            public void Invoke(string val) { foreach (var a in actions) a(val); }
        }

        public class InputField : Component
        {
            public string text { get; set; }
            public InputFieldValueChangedEvent onValueChanged { get; set; } = new InputFieldValueChangedEvent();
            public void ActivateInputField() { }
        }
    }

    namespace XR
    {
        public static class XRSettings
        {
            public static bool isDeviceActive => false;
            public static string loadedDeviceName => "";
        }

        public enum InputDeviceCharacteristics
        {
            HeadMounted = 1
        }

        public struct InputDevice
        {
            public string name { get; set; }
        }

        public static class InputDevices
        {
            public static void GetDevicesWithCharacteristics(InputDeviceCharacteristics characteristics, List<InputDevice> devices) { }
        }
    }
}
