using TMPro;
using UnityEngine;

namespace Environment.Props.Pyrolysis
{
    public class PyrolysisMachine : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private PyrolysisJerrycan jerrycan = null;

        [Header("Requirements")]
        [SerializeField] private int amountOfPlastic = 3;
        [SerializeField] private int amountOfMethane = 1;

        [Header("TextElements")] [SerializeField]
        private TextMeshPro amountOfPlasticText;
        private int plasticCount = 0;
        private int methaneCount = 0;

        public void AddPlastic()
        {
            plasticCount++;

            if (plasticCount == amountOfPlastic && methaneCount == amountOfMethane)
            {
                CreateJerrycan();
            }
            UpdatePlasticText();
        }

        private void UpdatePlasticText()
        {
            amountOfPlasticText.text = $"Je hebt nog {amountOfPlastic - plasticCount} plastic fles(sen) nodig.";
        }

        public void AddMethane()
        {
            methaneCount++;

            if (plasticCount == amountOfPlastic && methaneCount == amountOfMethane)
            {
                CreateJerrycan();
            }
        }

        public void CreateJerrycan()
        {
            Instantiate(jerrycan, spawnPoint.position, Quaternion.identity);
            Reset();
        }

        public void Reset()
        {
            plasticCount = 0;
            methaneCount = 0;
        }
    }
}
