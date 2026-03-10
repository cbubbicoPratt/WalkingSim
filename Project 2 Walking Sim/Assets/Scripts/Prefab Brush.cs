using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefab Brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(hideAssetInstances: false, hideDefaultInstance: true, defaultBrush: false, defaultName: "Prefab Brush")]
public class PrefabBrush : GameObjectBrush
{

}
