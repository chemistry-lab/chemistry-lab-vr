using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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

        [Header("Light")] [SerializeField] private Light pointLight = null;

        public void AddPlastic()
        {
            plasticCount++;

            if (plasticCount == amountOfPlastic && methaneCount == amountOfMethane)
            {
                CreateJerrycan();
            }
            UpdatePlasticText();
            UpdateLight();
        }

        private void UpdatePlasticText()
        {
            amountOfPlasticText.text = $"Je hebt nog {amountOfPlastic - plasticCount} plastic fles(sen) nodig.";
        }

        private void UpdateLight()
        {
            if(plasticCount >= amountOfPlastic && methaneCount >= amountOfMethane)
            {
                pointLight.color = Color.green;
            }
            else
            {
                pointLight.color = Color.red;
            }
        }

        public void AddMethane()
        {
            methaneCount++;

            if (plasticCount == amountOfPlastic && methaneCount == amountOfMethane)
            {
                CreateJerrycan();
            }
            UpdateLight();
        }

        public void CreateJerrycan()
        {
            Instantiate(jerrycan, spawnPoint.position, Quaternion.identity);
        }
    }
}
