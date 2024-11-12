using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Dreamsim.Publishing.Editor
{
public static class AndroidManifestHelper
{
    private const string ManifestPermission = "uses-permission";
    private const string MetaData = "meta-data";
    private const string ADIDPermissionAttr = "com.google.android.gms.permission.AD_ID";
    private const string AdMobApplicationIdAttr = "com.google.android.gms.ads.APPLICATION_ID";

    private static readonly XNamespace XNamespace = "http://schemas.android.com/apk/res/android";

    private static readonly string AndroidManifestPath =
        Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

    public static void Update(string adMobAppId)
    {
        var doc = XDocument.Load(AndroidManifestPath);
        var elManifest = doc.Element("manifest");
        if (elManifest == null)
        {
            DreamsimLogger.LogError("No manifest element in AndroidManifest.xml");
            return;
        }

        var elApplication = elManifest.Element("application");
        if (elApplication == null)
        {
            DreamsimLogger.LogError("No application element in AndroidManifest.xml");
            return;
        }

        CreateAdIdPermissionElement(elManifest);
        CreateAdMobAppIdElement(elApplication, adMobAppId);

        doc.Save(AndroidManifestPath);
    }

    private static void CreateAdMobAppIdElement(XElement elApplication, string adMobAppId)
    {
        var elsMeta = GetElementsByLocalName(elApplication, MetaData);
        var el = GetMetaDataElement(elsMeta, AdMobApplicationIdAttr);
        if (el != null)
            el.SetAttributeValue(XNamespace + "value", adMobAppId);
        else
            elApplication.Add(CreateMetaDataElement(AdMobApplicationIdAttr, adMobAppId));
    }

    private static void CreateAdIdPermissionElement(XContainer container)
    {
        var permissions = GetElementsByLocalName(container, ManifestPermission);

        if (GetPermissionElement(permissions, ADIDPermissionAttr) != null) return;

        Debug.Log("HERE");
        container.Add(CreatePermissionElement(ADIDPermissionAttr));
    }

    private static IEnumerable<XElement> GetElementsByLocalName(XContainer container, string localName)
    {
        return container!.Descendants().Where(elem => elem.Name.LocalName.Equals(localName));
    }

    private static XElement GetPermissionElement(IEnumerable<XElement> permissions, string permissionName)
    {
        return (from elem in permissions
            let attrs = elem.Attributes()
            from attr in attrs
            where attr.Name.Namespace.Equals(XNamespace)
                && attr.Name.LocalName.Equals("name")
                && attr.Value.Equals(permissionName)
            select elem).FirstOrDefault();
    }

    private static XElement CreatePermissionElement(string name)
    {
        return new XElement(ManifestPermission, new XAttribute(XNamespace + "name", name));
    }

    private static XElement GetMetaDataElement(IEnumerable<XElement> metaDataElements, string metaDataName)
    {
        return (from metaDataElement in metaDataElements
            let attributes = metaDataElement.Attributes()
            where attributes.Any(attribute => attribute.Name.Namespace.Equals(XNamespace)
                && attribute.Name.LocalName.Equals("name")
                && attribute.Value.Equals(metaDataName))
            select metaDataElement).FirstOrDefault();
    }

    private static XElement CreateMetaDataElement(string name, object value)
    {
        var metaData = new XElement("meta-data");
        metaData.Add(new XAttribute(XNamespace + "name", name));
        metaData.Add(new XAttribute(XNamespace + "value", value));

        return metaData;
    }
}
}