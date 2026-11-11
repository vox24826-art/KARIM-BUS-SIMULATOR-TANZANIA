using UnityEngine;

namespace KarimBusSimulator
{
    public class WorldEnvironmentBuilder : MonoBehaviour
    {
        public Vector3 terrainSize = new Vector3(200f, 1f, 800f);
        public int treeCount = 30;
        public int buildingCount = 40;

        private void Start()
        {
            BuildTerrain();
            BuildRoadNetwork();
            BuildVegetation();
            BuildBuildings();
        }

        private void BuildTerrain()
        {
            var terrain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            terrain.name = "Terrain";
            terrain.transform.position = new Vector3(0f, -0.6f, 380f);
            terrain.transform.localScale = terrainSize;
            var renderer = terrain.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(0.44f, 0.57f, 0.28f, 1f);
            }
        }

        private void BuildRoadNetwork()
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "MainRoad";
            road.transform.position = new Vector3(0f, 0.05f, 380f);
            road.transform.localScale = new Vector3(18f, 0.08f, 120f);

            var road2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road2.name = "ConnectorRoad";
            road2.transform.position = new Vector3(0f, 0.05f, 180f);
            road2.transform.localScale = new Vector3(18f, 0.08f, 30f);
        }

        private void BuildVegetation()
        {
            for (int i = 0; i < treeCount; i++)
            {
                var tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                tree.name = "Tree_" + i;
                tree.transform.position = new Vector3(Random.Range(-50f, 50f), 1.5f, Random.Range(20f, 760f));
                tree.transform.localScale = new Vector3(0.8f, 2.2f, 0.8f);

                var canopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                canopy.transform.SetParent(tree.transform);
                canopy.transform.localPosition = new Vector3(0f, 2.4f, 0f);
                canopy.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
                var renderer = canopy.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = new Color(0.16f, 0.42f, 0.2f, 1f);
                }
            }
        }

        private void BuildBuildings()
        {
            for (int i = 0; i < buildingCount; i++)
            {
                var building = GameObject.CreatePrimitive(PrimitiveType.Cube);
                building.name = "Building_" + i;
                building.transform.position = new Vector3(Random.Range(-80f, 80f), 5f, Random.Range(10f, 760f));
                building.transform.localScale = new Vector3(Random.Range(8f, 20f), Random.Range(8f, 24f), Random.Range(8f, 16f));

                var renderer = building.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = new Color(0.8f, 0.78f, 0.7f, 1f);
                }
            }
        }
    }
}
