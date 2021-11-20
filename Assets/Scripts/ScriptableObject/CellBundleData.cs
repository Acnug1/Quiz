using UnityEngine;

[CreateAssetMenu(fileName = "New CellBundleData", menuName = "Cell Bundle Data/Create Bundle Data", order = 51)]
public class CellBundleData : ScriptableObject
{
    [SerializeField] private string _bundleIdentifier;
    [SerializeField] private CellData[] _cellData;

    public string BundleIdentifier => _bundleIdentifier;
    public CellData[] CellData => _cellData;
}
