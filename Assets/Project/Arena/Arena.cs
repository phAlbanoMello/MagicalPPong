using UnityEngine;

public struct Extents
{
    public float _lenght;
    public float _width;
}

public class Arena : MonoBehaviour
{
    [SerializeField]
    Transform northWall, southWall, westWall, eastWall;

    [SerializeField]
    float arenaLength, arenaWidth;

    public Extents Extents { get; private set; }

    internal void Init()
    {
        Extents = new Extents
        {
            _lenght = arenaLength,
            _width = arenaWidth
        };

        BuildArena();
    }

    private void BuildArena()
    {
        northWall.localScale = new Vector3(arenaWidth, northWall.localScale.y, northWall.localScale.z);
        southWall.localScale = new Vector3(arenaWidth, southWall.localScale.y, southWall.localScale.z);


        westWall.localScale = new Vector3(westWall.localScale.x, westWall.localScale.y, arenaLength);
        westWall.position = new Vector3(-arenaWidth / 2, 0, 0);

        eastWall.localScale = new Vector3(eastWall.localScale.x, eastWall.localScale.y, arenaLength);
        eastWall.position = new Vector3(arenaWidth / 2, 0, 0);

        northWall.position = new Vector3(0, 0, arenaLength / 2);
        southWall.position = new Vector3(0, 0, -arenaLength / 2);
    }
}
