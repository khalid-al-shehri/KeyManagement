using System;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain.Common;

public abstract class BaseEvent : INotification
{
    public string GetObjectDifferencesAsJson(object original, object updated)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var origJson = JObject.FromObject(original, JsonSerializer.CreateDefault(settings));
        var updatedJson = JObject.FromObject(updated, JsonSerializer.CreateDefault(settings));

        var diff = new JObject();

        bool hasChanges = false;

        foreach (var prop in origJson.Properties())
        {
            if (prop.Name.Equals("LastModified") || prop.Name.Equals("LastModifiedBy"))
                continue;

            var propName = prop.Name;
            var origValue = prop.Value;
            var updatedValue = updatedJson[propName];

            if (origValue.Type == JTokenType.Array || origValue.Type == JTokenType.Object || origValue.Type == JTokenType.Property)
                continue;

            if (updatedValue != null && (updatedValue.Type == JTokenType.Array || updatedValue.Type == JTokenType.Object || updatedValue.Type == JTokenType.Property))
                continue;

            if (propName == "TrackUUId" || propName == "Id")
            {
                continue;
            }

            if (propName == "Username" )
            {
                diff[propName] = origValue;
            }

            if (!JToken.DeepEquals(origValue, updatedValue))
            {
                hasChanges = true;
                dynamic jsonObject = new JObject();
                jsonObject.Old = origValue;
                jsonObject.New = updatedValue;

                diff[propName] = jsonObject; //new JArray(origValue, updatedValue);
            }
        }

        if (hasChanges)
        {
            return diff.ToString(Newtonsoft.Json.Formatting.Indented);
        }
        else
        {
            return string.Empty;
        }
    }


}