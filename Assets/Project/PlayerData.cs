using System;
using Unity.Properties;

[GeneratePropertyBag]
[Serializable]
public class PlayerData
{
    public string Name;
    public int Score;

    public PlayerData() { }

    public PlayerData(string playerName, int currentScore)
    {
        this.Name = playerName;
        this.Score = currentScore;
    }
}

