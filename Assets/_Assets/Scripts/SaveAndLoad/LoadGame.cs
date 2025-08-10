using UnityEngine;

public class LoadGame
{
    public GameData _data;
    public void LoadGameBtn()
    {
        GameData data = Serializer.LoadGame();

        if (data == null) return;
        else
        {
            _data = data;
        }
    }
}
