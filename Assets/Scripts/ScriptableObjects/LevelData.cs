using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 51)]
    public class LevelData : ScriptableObject
    {
        public static List<PlacementObject> AllObjects;

        public PlacementObject[] objects;
        public Vector2Int size;
        public int height;
        public PlacementObjectInCar[] placementObjects;
        public float cameraView = 22f;

        public static LevelData Generate(List<PlacementObject> allItems, Vector2Int levelSize, int levelHeight)
        {
            var random = new Random();
            int volume = levelSize.x * levelSize.y * levelHeight;
            var all = new List<PlacementObject>(allItems);
            List<PlacementObject> objectsList = new List<PlacementObject>();

            int currentVolume = 0;

            while (currentVolume != volume)
            {
                if (currentVolume == volume) break;

                if (currentVolume < volume)
                {
                    var randomIndex = random.Next(0, all.Count - 1);
                    var currentObj = all[randomIndex];

                    objectsList.Add(currentObj);
                    all.Remove(currentObj);
                    currentVolume += currentObj.Volume;
                }

                if (currentVolume > volume)
                {
                    var objectToDel = objectsList[random.Next(0, objectsList.Count - 1)];

                    objectsList.Remove(objectToDel);
                    all.Add(objectToDel);
                    currentVolume -= objectToDel.Volume;
                }
            }

            var matrix = new PlacementObject[levelSize.x, levelSize.y, levelHeight];
            var isCanPlace = CanPlaceObjects(objectsList, matrix);
            Debug.Log(ArrayToString(matrix));
            
            if (isCanPlace)
            {
                LevelData levelData = new LevelData();
                levelData.objects = objectsList.ToArray();
                levelData.placementObjects = new PlacementObjectInCar[0];
                levelData.size = levelSize;
                levelData.height = levelHeight;
                levelData.height = levelHeight;
                return levelData;
            }
            
            return Generate(allItems, levelSize, levelHeight);
        }

        private static IEnumerable<Vector3Int> GetRotations(Vector3Int size)
        {
            return new List<Vector3Int>
            {
                size,
                new(size.z, size.y, size.x)
            };
        }
        
        static string ArrayToString(PlacementObject[,,] array)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        sb.Append(array[i, j, k]);
                        sb.Append(" ");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
            }
    
            return sb.ToString();
        }

        public static bool CanPlaceObjects(List<PlacementObject> objects, PlacementObject[,,] grid)
        {
            objects.Sort((a, b) => (b.Size.x * b.Size.y * b.Size.z).CompareTo(a.Size.x * a.Size.y * a.Size.z));

            foreach (var obj in objects)
            {
                bool isPlaced = false;

                // Для каждого объекта получаем все возможные варианты поворота
                var rotations = GetRotations(obj.Size);
                foreach (var rotation in rotations)
                {
                    // Просматриваем все позиции на сетке для каждого варианта поворота
                    for (int x = 0; x < grid.GetLength(0) && !isPlaced; x++)
                    {
                        for (int y = 0; y < grid.GetLength(1) && !isPlaced; y++)
                        {
                            for (int z = 0; z < grid.GetLength(2) && !isPlaced; z++)
                            {
                                Debug.Log(CanPlace(obj, rotation, new Vector3Int(x, y, z), grid));
                                if (CanPlace(obj, rotation, new Vector3Int(x, y, z), grid))
                                {
                                    PlaceObject(obj, rotation, new Vector3Int(x, y, z), grid);
                                    isPlaced = true;
                                }
                            }
                        }
                    }
                    // Если объект размещен, прерываем цикл поворотов
                    if (isPlaced) break;
                }

                // Если не удалось разместить объект, возвращаем false
                if (!isPlaced) return false;
            }

            // Если все объекты размещены, возвращаем true
            return true;
        }

        private static void PlaceObject(PlacementObject obj, Vector3Int size, Vector3Int location,
            PlacementObject[,,] grid)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        grid[location.x + x, location.y + y, location.z + z] = obj;
                    }
                }
            }
        }

        private static bool CanPlace(PlacementObject obj, Vector3Int size, Vector3Int location,
            PlacementObject[,,] grid)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        if (location.x + x >= grid.GetLength(0) ||
                            location.y + y >= grid.GetLength(1) ||
                            location.z + z >= grid.GetLength(2) ||
                            grid[location.x + x, location.y + y, location.z + z] != null)
                        {
                            return false; // возвращаем false, если не можем поместить объект
                        }
                    }
                }
            }
            return true; // возвращаем true, если можем поместить объект
        }
    }

    [System.Serializable]
    public struct PlacementObjectInCar
    {
        public PlacementObject placementObject;
        public Vector2Int position;
    }
}