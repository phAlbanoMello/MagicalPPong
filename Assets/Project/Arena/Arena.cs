using UnityEngine;

public struct Extents
{
    public float _length;
    public float _width;
}

public class Arena : MonoBehaviour
{
    [SerializeField]
    Transform northWall, southWall, westWall, eastWall;

    [SerializeField]
    private float arenaLength, arenaWidth;

    [SerializeField]
    private float MIN_LENGTH, MAX_LENGTH;

    public Extents Extents { get; private set; }

    public void Init()
    {
        BuildArena(arenaWidth, arenaLength);
    }

    private void BuildArena(float width, float lenght)
    {
        Extents = new Extents
        {
            _length = lenght,
            _width = width
        };

        northWall.localScale = new Vector3(width, northWall.localScale.y, northWall.localScale.z);
        southWall.localScale = new Vector3(width, southWall.localScale.y, southWall.localScale.z);


        westWall.localScale = new Vector3(westWall.localScale.x, westWall.localScale.y, lenght);
        westWall.position = new Vector3(-width / 2, 0, 0);

        eastWall.localScale = new Vector3(eastWall.localScale.x, eastWall.localScale.y, lenght);
        eastWall.position = new Vector3(width / 2, 0, 0);

        northWall.position = new Vector3(0, 0, lenght / 2);
        southWall.position = new Vector3(0, 0, -lenght / 2);
    }

    public void UpdateScale()
    {
        float updatedLength = Mathf.Clamp(arenaLength * GameSettings.Instance.ArenaScale, MIN_LENGTH, MAX_LENGTH);
        float updatedWidth = Mathf.Clamp(arenaWidth * GameSettings.Instance.ArenaScale, MIN_LENGTH, MAX_LENGTH);
        
        Extents = new Extents
        {
            _length = updatedLength,
            _width = updatedLength
        };

        BuildArena(updatedWidth, updatedLength);
    }
}
