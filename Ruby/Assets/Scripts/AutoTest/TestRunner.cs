using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    private object scriptBInstance; // ScriptB 实例
    void Start()
    {
        StartCoroutine(RunAttackButtonTest());
    }

    public IEnumerator RunAttackButtonTest()
    {
        yield return new WaitForSeconds(2f);
        AttackButtonTest test = new AttackButtonTest();
        test.TestButton();
    }

    public void RunAllTests()
    {

    }
}
