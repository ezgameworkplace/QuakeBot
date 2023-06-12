using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Assertions;
using System.Text;

public class ButtonAnalyzer : MonoBehaviour
{
    List<Tuple<string, string>> buttonActions;

    void Start()
    {
        // AnalyseButton();
        buttonActions = GetButtonActions();
        GenerateButtonEnum();
        foreach (Tuple<string, string> action in buttonActions)
        {
            string script = GenerateTestScript(action.Item1, action.Item2);

            // 创建一个唯一的文件名
            string fileName = ".\\Assets\\Scripts\\AutoTest\\AutoScripts\\TestScript_" + action.Item1 + "_" + action.Item2 + ".cs";

            // 将脚本保存到文件
            File.WriteAllText(fileName, script);
        }

    }

    public void AnalyseButton()
    {
        // 遍历场景中的所有按钮
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // 获取按钮的onClick事件
            UnityEvent onClickEvent = button.onClick;
            if (onClickEvent == null)
            {
                continue;
            }

            // 遍历onClick事件的所有响应函数
            for (int i = 0; i < onClickEvent.GetPersistentEventCount(); i++)
            {
                // 获取响应函数的目标对象和方法名
                UnityEngine.Object target = onClickEvent.GetPersistentTarget(i);
                string methodName = onClickEvent.GetPersistentMethodName(i);

                // 打印出目标对象的名字和方法名
                Debug.Log("Button: " + button.name + ", Target: " + target.name + ", Method: " + methodName);
            }
        }
    }

    public List<Tuple<string, string>> GetButtonActions()
    {
        List<Tuple<string, string>> buttonActions = new List<Tuple<string, string>>();

        // 遍历场景中的所有按钮
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // 获取按钮的onClick事件
            UnityEvent onClickEvent = button.onClick;
            if (onClickEvent == null)
            {
                continue;
            }

            // 遍历onClick事件的所有响应函数
            for (int i = 0; i < onClickEvent.GetPersistentEventCount(); i++)
            {
                // 获取响应函数的目标对象和方法名
                UnityEngine.Object target = onClickEvent.GetPersistentTarget(i);
                string methodName = onClickEvent.GetPersistentMethodName(i);

                // 将按钮的名字和方法名加入到列表中
                buttonActions.Add(new Tuple<string, string>(button.name, methodName));
            }
        }

        return buttonActions;
    }

    public void GenerateButtonEnum()
    {
        // 首先，我们需要获得所有的按钮名字
        var buttonNames = new HashSet<string>();
        foreach (var action in buttonActions)
        {
            buttonNames.Add(action.Item1);  // 添加按钮的名字
        }

        // 然后，我们创建一个字符串来存储枚举的定义
        var enumDefinition = new StringBuilder("public enum ButtonEnum\n{\n");

        // 对于每一个按钮的名字，我们添加一个枚举的成员
        foreach (var name in buttonNames)
        {
            enumDefinition.AppendLine($"    {name},");
        }

        // 添加枚举定义的结尾
        enumDefinition.AppendLine("}");

        // 将枚举的定义保存到一个文件中
        string fileName = ".\\Assets\\Scripts\\AutoTest\\ButtonEnum.cs";
        File.WriteAllText(fileName, enumDefinition.ToString());
    }


    public string GenerateTestScript(string buttonName, string methodName)
    {
        // 测试脚本的模板
        string template = $@"using UnityEngine;
using UnityEngine.UI;

public class {buttonName}ButtonTest
{{
    public void TestButton()
    {{
        // 查找按钮
        Button button = GameObject.Find(""{buttonName}"").GetComponent<Button>();

        // 调用按钮的点击事件
        button.onClick.Invoke();

        // 打印响应函数名称
        Debug.Log(""Button {buttonName} invoked method {methodName}"");
    }}
}}
";
        return template;
    }
}