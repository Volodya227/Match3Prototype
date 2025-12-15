using UnityEngine;
namespace View.Items.DataConfig
{
    [CreateAssetMenu(menuName = "Match3/Item Prefabs Data")]
    public class ItemDATAPrefabs : ScriptableObject
    // DATA в caps - нетипово. Краще ItemDataPrefabs або ItemPrefabsConfig.
    {
        [SerializeField] private GameObject[] _prefabCommonRed;
        [SerializeField] private GameObject[] _prefabCommonYellow;
        [SerializeField] private GameObject[] _prefabCommonBlue;
        [SerializeField] private GameObject[] _prefabCommonGreen;
        [SerializeField] private GameObject[] _prefabCommonPurple;
        
        // Окремий масив для кожного кольору - багато дублювання коду.
        // можнра використати Serializable клас або Dictionary:
        //
        // [System.Serializable]
        // public class ColorPrefabs {
        //     public ItemTypeColor color;
        //     public GameObject[] prefabs;
        // }
        // [SerializeField] private ColorPrefabs[] _allPrefabs;
        //
        // Це дозволить легко додавати нові кольори без зміни коду.
        
        public int GetPrefabCommonRedCount => _prefabCommonRed.Length;
        public int GetPrefabCommonYellowCount => _prefabCommonYellow.Length;
        public int GetPrefabCommonBlueCount => _prefabCommonBlue.Length;
        public int GetPrefabCommonGreenCount => _prefabCommonGreen.Length;
        public int GetPrefabCommonPurpleCount => _prefabCommonPurple.Length;
        // Ці properties не використовуються в проекті.
        // Або використати, або видалити.
        
        public GameObject GetPrefabCommonRed(int i) => _prefabCommonRed[i];
        public GameObject GetPrefabCommonYellow(int i) => _prefabCommonYellow[i];
        public GameObject GetPrefabCommonBlue(int i) => _prefabCommonBlue[i];
        public GameObject GetPrefabCommonGreen(int i) => _prefabCommonGreen[i];
        public GameObject GetPrefabCommonPurple(int i) => _prefabCommonPurple[i];
        // Немає перевірки меж масиву.
        // Якщо i >= array.Length - буде IndexOutOfRangeException.
        
        // Один універсальний метод замість п'яти:
        // public GameObject GetPrefab(Data.ItemTypeColor color, int typeIndex)
        // {
        //     var array = color switch
        //     {
        //         Data.ItemTypeColor.Red => _prefabCommonRed,
        //         Data.ItemTypeColor.Yellow => _prefabCommonYellow,
        //         Data.ItemTypeColor.Blue => _prefabCommonBlue,
        //         Data.ItemTypeColor.Green => _prefabCommonGreen,
        //         Data.ItemTypeColor.Purple => _prefabCommonPurple,
        //         _ => throw new System.ArgumentException($"Unknown color: {color}")
        //     };
        //     return array[Mathf.Clamp(typeIndex, 0, array.Length - 1)];
        // }
    }
}