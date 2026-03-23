using NUnit.Framework;
using UnityEngine;

public class CharacterManagerTests
{
    private GameObject testObject;
    private CharacterSelectManager manager;
    private GameObject[] characterOptions;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("TestObject");
        manager = testObject.AddComponent<CharacterSelectManager>();
        
        characterOptions = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            characterOptions[i] = new GameObject("Char" + i);
            characterOptions[i].transform.parent = testObject.transform;
            characterOptions[i].SetActive(false);
        }
        manager.characterOptions = characterOptions;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testObject);
    }

    [Test]
    public void NextCharacter_CyclesCorrectly()
    {
        // Simulate Start
        manager.Invoke("Start", 0); 
        
        Assert.AreEqual(0, manager.CurrentIndex);
        Assert.IsTrue(characterOptions[0].activeSelf);
        Assert.IsFalse(characterOptions[1].activeSelf);
        Assert.IsFalse(characterOptions[2].activeSelf);

        manager.NextCharacter();
        Assert.AreEqual(1, manager.CurrentIndex);
        Assert.IsFalse(characterOptions[0].activeSelf);
        Assert.IsTrue(characterOptions[1].activeSelf);
        Assert.IsFalse(characterOptions[2].activeSelf);

        manager.NextCharacter();
        Assert.AreEqual(2, manager.CurrentIndex);
        Assert.IsFalse(characterOptions[0].activeSelf);
        Assert.IsFalse(characterOptions[1].activeSelf);
        Assert.IsTrue(characterOptions[2].activeSelf);

        manager.NextCharacter();
        Assert.AreEqual(0, manager.CurrentIndex);
        Assert.IsTrue(characterOptions[0].activeSelf);
        Assert.IsFalse(characterOptions[1].activeSelf);
        Assert.IsFalse(characterOptions[2].activeSelf);
    }

    [Test]
    public void PreviousCharacter_CyclesCorrectly()
    {
        // Simulate Start
        manager.Invoke("Start", 0);

        manager.PreviousCharacter();
        Assert.AreEqual(2, manager.CurrentIndex);
        Assert.IsFalse(characterOptions[0].activeSelf);
        Assert.IsFalse(characterOptions[1].activeSelf);
        Assert.IsTrue(characterOptions[2].activeSelf);

        manager.PreviousCharacter();
        Assert.AreEqual(1, manager.CurrentIndex);
        Assert.IsFalse(characterOptions[0].activeSelf);
        Assert.IsTrue(characterOptions[1].activeSelf);
        Assert.IsFalse(characterOptions[2].activeSelf);
    }
}
