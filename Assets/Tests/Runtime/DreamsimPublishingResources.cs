using Dreamsim;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using Dreamsim.Publishing;
using UnityEngine;

namespace Tests.Runtime
{
[TestFixture]
public class DreamsimPublishingResources
{
    [Test]
    public void Project_HasAsset_Settings( )
    {
        Object asset = Resources.Load( "[Dreamsim] Publishing Settings" );

        Assert.IsNotNull( asset );
    }

    [Test]
    public void Project_HasAsset_DreamsimPublishing( )
    {
        Object prefab = Resources.Load( "Dreamsim/[Dreamsim] Publishing" );

        Assert.IsNotNull( prefab );
    }

    [Test]
    public void DreamsimPublishingPrefab_HasComponent_DreamsimPublishing( )
    {
        GameObject prefab = Resources.Load( "Dreamsim/[Dreamsim] Publishing" ) as GameObject;
        bool hasComponent = prefab.TryGetComponent<DreamsimPublishing>( out _ );

        Assert.IsTrue( hasComponent );
    }
}

}