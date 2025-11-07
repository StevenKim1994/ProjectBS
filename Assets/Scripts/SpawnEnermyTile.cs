using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "BS/Tiles/SpawnEnermyTile")]
public class SpawnEnermyTile : Tile
{
    [SerializeField] private AssetReference enermyAsset;

}
