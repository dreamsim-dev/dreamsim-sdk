using System.Collections;
using Dreamsim;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
[TestFixture]
public class DreamsimCommonCreate
{
    [OneTimeSetUp]
    public void OneTimeSetUp( )
    {
        // EditorSceneManager.LoadSceneInPlayMode( "Assets/Scenes/Menu.unity", new LoadSceneParameters(LoadSceneMode.Single) );
        DreamsimCommon.Create();
    }

    [Test]
    public void Hierarchy_HasGameObject_DreamsimCommon_Exactly1( )
    {
        DreamsimCommon[] found = Object.FindObjectsOfType<DreamsimCommon>();

        // yield return null;

        Assert.AreEqual( 1, found.Length );
        Assert.IsNotNull( found[0] );
    }
}
}