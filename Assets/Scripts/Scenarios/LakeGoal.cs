using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using Molecule.Pack;
using Utilities;

namespace Scenarios
{
    public class LakeGoal : MonoBehaviour
    {
        [Header("Solution")]
        [SerializeField] private MoleculePack correct = null;

        [Header("Environment")]
        [SerializeField] private List<ParticleSystem> dirtyParticles = new List<ParticleSystem>();
        [SerializeField] private List<Renderer> dirtyObjects = new List<Renderer>();
        [SerializeField] private List<Renderer> dirtyWater = new List<Renderer>();

        [Header("Water")]
        [SerializeField] private Texture cleanWater = null;

        [Header("Fade")]
        [SerializeField] private float fadeTimer = 3f;

        [Header("Scene")]
        [SerializeField] private SceneLoader loader = null;
        [SerializeField] private UnityScene scene = null;
        [SerializeField] private float timeout = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            MoleculePack molecule = other.GetComponent<MoleculePack>();
            if (molecule != null && molecule.MoleculeAbbreviation == correct.MoleculeAbbreviation)
            {
                foreach (Renderer gameObject in dirtyObjects) StartCoroutine(FadeMaterial(gameObject.material));
                foreach (ParticleSystem particleSystem in dirtyParticles) particleSystem.Stop();
                foreach (Renderer water in dirtyWater) water.material.SetTexture("_MainTex", cleanWater);

                loader.LoadScene(scene, timeout);
            }
        }

        public IEnumerator FadeMaterial(Material material)
        {
            for (float t = 0.0f; t < 1; t += Time.deltaTime / fadeTimer)
            {
                Color color = material.color;
                color.a = Mathf.Lerp(1, 0, t);
                material.color = color;
                yield return null;
            }
        }
    }
}
