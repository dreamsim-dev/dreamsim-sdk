using Dreamsim;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine;

namespace Tests.Runtime
{
[TestFixture]
public class DreamsimCommonResources
{
    [Test]
    public void Project_HasAsset_DreamsimCommon( )
    {
        Object prefab = Resources.Load( "Dreamsim/[Dreamsim] Common" );

        //yield return null;

        Assert.IsNotNull( prefab );
    }

    [Test]
    public void DreamsimCommonPrefab_HasComponent_DreamsimCommon( )
    {
        GameObject prefab = Resources.Load( "Dreamsim/[Dreamsim] Common" ) as GameObject;
        bool hasComponent = prefab.TryGetComponent<DreamsimCommon>( out _ );

        //yield return null;

        Assert.IsTrue( hasComponent );
    }

    [Test]
    public void Project_HasAsset_DreamsimBuildNumber( )
    {
        Object asset = Resources.Load( "[Dreamsim] BuildNumber" );

        //yield return null;

        Assert.IsNotNull( asset );
    }
}
}