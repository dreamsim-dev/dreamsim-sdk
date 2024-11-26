using Dreamsim.Publishing;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Runtime
{
[TestFixture]
public class DreamsimPublishingInit
{
    [OneTimeSetUp]
    public async void OneTimeSetUp( )
    {
        await DreamsimPublishing.InitAsync();
    }

}
}