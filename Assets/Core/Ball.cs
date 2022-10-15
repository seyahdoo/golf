using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {
    public Camera cam;
    public Rigidbody2D body;
    public LineRenderer line;

    public float maxForce;
    public float maxLineLength = .5f;
    private Vector3 _startPoint;

    private void Awake() {
        line.gameObject.SetActive(false);
        line.useWorldSpace = true;
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _startPoint = Input.mousePosition;
            line.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0)) {
            var diff = _startPoint - Input.mousePosition;
            var dir = diff.normalized;
            var screenSize = Mathf.Min(Screen.height, Screen.width);
            var maxScreenDistance = screenSize / 4f;
            var pow = diff.magnitude;
            pow = Mathf.Clamp(pow, 0, maxScreenDistance);
            pow = pow / maxScreenDistance;
            var pos = transform.position;
            line.SetPosition(0, pos);
            line.SetPosition(1, pos + dir * (pow * maxLineLength));
            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.Lerp(Color.white, Color.red, pow), 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );
            line.colorGradient = gradient;
        }
        if (Input.GetMouseButtonUp(0)) {
            var diff = _startPoint - Input.mousePosition;
            var dir = diff.normalized;

            var screenSize = Mathf.Min(Screen.height, Screen.width);
            var maxScreenDistance = screenSize / 4f;
            var pow = diff.magnitude;
            pow = Mathf.Clamp(pow, 0, maxScreenDistance);
            pow = pow / maxScreenDistance;

            pow = pow * maxForce;
            
            body.AddForce(dir * pow, ForceMode2D.Impulse);
            line.gameObject.SetActive(false);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Goal>()) {
            Invoke(nameof(LoadNextScene), 1f);
        }
        if (col.gameObject.GetComponent<Spike>()) {
            Invoke(nameof(RestartLevel), 1f);
        }
    }
    private void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}