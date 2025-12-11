using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    void Start()
    {
        // 複数ディスプレイがあるか確認
        Debug.Log("Connected Displays: " + Display.displays.Length);

        // メイン以外のディスプレイを有効化
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate(); // 2枚目をONにする
        }
    }
}

