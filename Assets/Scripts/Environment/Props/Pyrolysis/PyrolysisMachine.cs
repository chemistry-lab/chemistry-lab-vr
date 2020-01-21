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

        private int plasticCount = 0;
        private int methaneCount = 0;

        public void AddPlastic()
        {
            plasticCount++;

            if (plasticCount == amountOfPlastic && methaneCount == amountOfMethane)
            {
                CreateJerrycan();
            }
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
