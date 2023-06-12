using UnityEngine;
using UnityEngine.UI;

public class TalkButtonTest
{
    public void TestButton()
    {
        // 查找按钮
        Button button = GameObject.Find("Talk").GetComponent<Button>();

        // 调用按钮的点击事件
        button.onClick.Invoke();

        // 打印响应函数名称
        Debug.Log("Button Talk invoked method OnTalkButtonClicked");
    }
}
