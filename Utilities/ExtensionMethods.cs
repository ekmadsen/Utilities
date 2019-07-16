using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace ErikTheCoder.Utilities
{
    [UsedImplicitly]
    public static class ExtensionMethods
    {
        [UsedImplicitly]
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> Tuple, out T1 Key, out T2 Value)
        {
            Key = Tuple.Key;
            Value = Tuple.Value;
        }


        [UsedImplicitly]
        public static string RemoveControlCharacters(this string Text) => Regex.Replace(Text, @"[\p{C}-[\r\n\t]]+", string.Empty);


        // Performs a recursive copy of all class fields (public and private), except those the caller specifies should not be copied.
        // Supports circular references and fields whose type is an interface or abstract class.
        // In comparison, Object.MemberwiseClone in the Base Class Library performs a shallow copy.  Reference type fields of the copy point to the original reference.
        // In other words, for reference type fields...
        // For MemberwiseClone():  Object.ReferenceEquals(copy.FieldName, original.FieldName) == true;
		// For DeepCopy():         Object.ReferenceEquals(copy.FieldName, original.FieldName) == false;
		[UsedImplicitly]
        public static T DeepCopy<T>(this T Original, List<string> SkipFields = null) where T : class
        {
            if (Original != null)
            {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    // Preserving object references enables serialization to function correctly when an object graph contains circular references.
                    //   Such as a parent object referring to a child object, and the child object referring back to the parent object.
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    // Including type names increases size of serialized JSON but prevents an exception:
                    //   "Could not create an instance of type [name].  Type is an interface or abstract class and cannot be instantiated."
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new FieldContractResolver(SkipFields ?? new List<string>())
                };
                string json = JsonConvert.SerializeObject(Original, jsonSerializerSettings);
                if (json != null) { return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings); }
            }
            return null;
        }


        [UsedImplicitly]
        public static void Shuffle<T>(this List<T> Items, IThreadsafeRandom Random)
        {
            // Use the Fischer-Yates algorithm.
            // See https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            int maxIndex = Items.Count - 1;
            for (int index = maxIndex; index > 0; index--)
            {
                int swapIndex = Random.Next(0, index + 1);
                T temp = Items[index];
                Items[index] = Items[swapIndex];
                Items[swapIndex] = temp;
            }
        }


        // By default, Json.NET serializes public properties.
        // This works fine for Data Transfer Objects (DTOs) that contain auto-implemented properties with no logic in getters and setters.
        // It can cause null reference exceptions in domain objects that do contain logic in property getters and setters (if they're sensitive to the sequence in which they're called).
        // So, copy fields instead of properties.
        // See https://stackoverflow.com/a/24107081/8992299
        private class FieldContractResolver : DefaultContractResolver
        {
            private readonly HashSet<string> _skipFields;


            public FieldContractResolver(IEnumerable<string> SkipFields)
            {
                _skipFields = new HashSet<string>(SkipFields, StringComparer.CurrentCultureIgnoreCase);
            }


            protected override IList<JsonProperty> CreateProperties(Type Type, MemberSerialization MemberSerialization)
            {
                List<JsonProperty> jsonProperties = new List<JsonProperty>();
                FieldInfo[] fields = Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (_skipFields.Contains(field.Name)) { continue; }
                    JsonProperty jsonProperty = base.CreateProperty(field, MemberSerialization);
                    jsonProperty.Ignored = false;
                    jsonProperty.Readable = true;
                    jsonProperty.Writable = true;
                    jsonProperties.Add(jsonProperty);
                }
                return jsonProperties;
            }
        }
    }
}