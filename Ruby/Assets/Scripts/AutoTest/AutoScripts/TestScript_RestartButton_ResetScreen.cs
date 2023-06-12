using UnityEngine;
using UnityEngine.UI;

public class RestartButtonButtonTest
{
    public void TestButton()
    {
        // 查找按钮
        Button button = GameObject.Find("RestartButton").GetComponent<Button>();

        // 调用按钮的点击事件
        button.onClick.Invoke();

        // 打印响应函数名称
        Debug.Log("Button RestartButton invoked method ResetScreen");
    }
}
