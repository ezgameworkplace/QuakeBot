using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    public bool RunTest = false;
    private object scriptBInstance; // ScriptB 实例
    void Start()
    {

    }

    void Update()
    {
        if (RunTest) { StartCoroutine(RunAllTests()); RunTest = false; }
    }

    public IEnumerator RunAttackButtonTest()
    {
        yield return new WaitForSeconds(2f);
        AttackButtonTest test = new AttackButtonTest();
        test.TestButton();
    }

    public IEnumerator RunTalkButtonTest()
    {
        yield return new WaitForSeconds(2f);
        GameObject rubyObject = GameObject.Find("Ruby");
        rubyObject.transform.position = new Vector3(0.5f, -3.49f, 0f);
        TalkButtonTest test = new TalkButtonTest();
        test.TestButton();
    }

    public IEnumerator RunAllTests()
    {
        StartCoroutine(RunAttackButtonTest());
        yield return new WaitForSeconds(2f);
        StartCoroutine(RunTalkButtonTest());
    }
}
