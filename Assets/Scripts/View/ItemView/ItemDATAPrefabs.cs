using UnityEngine;
namespace View.Items.DataConfig
{
    [CreateAssetMenu(menuName = "Match3/Item Prefabs Data")]
    public class ItemDATAPrefabs : ScriptableObject
    {
        [SerializeField] private GameObject[] _prefabCommonRed;
        [SerializeField] private GameObject[] _prefabCommonYellow;
        [SerializeField] private GameObject[] _prefabCommonBlue;
        [SerializeField] private GameObject[] _prefabCommonGreen;
        [SerializeField] private GameObject[] _prefabCommonPurple;
        public int GetPrefabCommonRedCount => _prefabCommonRed.Length;
        public int GetPrefabCommonYellowCount => _prefabCommonYellow.Length;
        public int GetPrefabCommonBlueCount => _prefabCommonBlue.Length;
        public int GetPrefabCommonGreenCount => _prefabCommonGreen.Length;
        public int GetPrefabCommonPurpleCount => _prefabCommonPurple.Length;
        public GameObject GetPrefabCommonRed(int i) => _prefabCommonRed[i];
        public GameObject GetPrefabCommonYellow(int i) => _prefabCommonYellow[i];
        public GameObject GetPrefabCommonBlue(int i) => _prefabCommonBlue[i];
        public GameObject GetPrefabCommonGreen(int i) => _prefabCommonGreen[i];
        public GameObject GetPrefabCommonPurple(int i) => _prefabCommonPurple[i];
    }
}