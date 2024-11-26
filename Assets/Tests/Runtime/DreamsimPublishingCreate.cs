using System.Collections;
using Dreamsim.Publishing;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
[TestFixture]
public class DreamsimPublishingCreate
{
    [OneTimeSetUp]
    public void OneTimeSetUp( )
    {
        DreamsimPublishing.Create();
    }

    [Test]
    public void Hierarchy_HasGameObject_DreamsimPublishing_Exactly1( )
    {
        DreamsimPublishing[] found = Object.FindObjectsOfType<DreamsimPublishing>();

        // yield return null;

        Assert.AreEqual( 1, found.Length );
        Assert.IsNotNull( found[0] );
        Debug.Log( $"<color=cyan> {found[0].gameObject.scene} </color>" ); //.scene.name == "DontDestroyOnLoad"
        // bool IsInDontDestroyOnLoad(GameObject gameObject) => gameObject.scene.buildIndex == -1;
    }

    [Test]
    public void DreamsimPublishing_Analytics_NotNull( )
    {
        var component = DreamsimPublishing.Analytics;

        Assert.IsNotNull( component );
    }

    [Test]
    public void DreamsimPublishing_Advertisement_NotNull( )
    {
        var component = DreamsimPublishing.Advertisement;

        Assert.IsNotNull( component );
    }

    [Test]
    public void DreamsimPublishing_CrossPromoHelper_NotNull( )
    {
        var component = DreamsimPublishing.CrossPromo;

        Assert.IsNotNull( component );
    }
}
}