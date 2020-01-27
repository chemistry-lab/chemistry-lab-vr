using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(Renderer))]
    public class SceneLoader : MonoBehaviour
    {
        [Header("Fade")]
        [SerializeField] private float fadeInTime = 3.0f;
        [SerializeField] private float fadeOutTime = 3.0f;

        private bool isStarted = false;
        private Renderer panel = null;

        private void Awake()
        {
            panel = GetComponent<Renderer>();

            if (!panel) Debug.LogWarning("No Renderer Component Provided!");
        }

        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        public void LoadScene(UnityScene scene)
        {
            if (!isStarted)
            {
                StartCoroutine(FadeOut(scene, 0.0f));
            }
        }

        public void LoadScene(UnityScene scene, float timeout)
        {
            if (!isStarted)
            {
                StartCoroutine(FadeOut(scene, timeout));
            }
        }

        private IEnumerator FadeIn()
        {
            for (float t = 0.0f; t < 1; t += Time.deltaTime / fadeInTime)
            {
                panel.material.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
                yield return null;
            }

            panel.material.color = new Color(0, 0, 0, 0);
        }

        private IEnumerator FadeOut(UnityScene scene, float timeout)
        {
            isStarted = true;

            yield return new WaitForSeconds(timeout);
            for (float t = 0.0f; t < 1; t += Time.deltaTime / fadeOutTime)
            {
                panel.material.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
                yield return null;
            }

            SceneManager.LoadScene(scene);
        }
    }
}
