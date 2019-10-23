﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
        public static string GetSummary(this Exception Exception, bool IncludeStackTrace = false, bool RecurseInnerExceptions = false)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Exception exception = Exception;
            while (exception != null)
            {
                // Include spaces to align text.
                stringBuilder.AppendLine($"Exception Type =             {exception.GetType().FullName}");
                stringBuilder.AppendLine($"Exception Message =          {exception.Message}");
                if (IncludeStackTrace) stringBuilder.AppendLine($"Exception StackTrace =       {exception.StackTrace?.TrimStart(' ')}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
                exception = RecurseInnerExceptions ? exception.InnerException : null;
            }
            return stringBuilder.ToString();
        }


        [UsedImplicitly]
        public static string Truncate(this string Text, int MaxLength)
        {
            if (string.IsNullOrEmpty(Text)) return Text;
            return Text.Length <= MaxLength ? Text : Text.Substring(0, MaxLength);
        }


        [UsedImplicitly]
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> Tuple, out T1 Key, out T2 Value)
        {
            Key = Tuple.Key;
            Value = Tuple.Value;
        }


        [UsedImplicitly]
        public static string RemoveControlCharacters(this string Text) => Regex.Replace(Text, @"[\p{C}-[\r\n\t]]+", string.Empty);


        [UsedImplicitly]
        public static string GetNumberWithSuffix(this int Number, string Format = null)
        {
            if (Number == 12) return $"{Number.ToString(Format)}th";
            int digitInOnesColumn = Number % 10;
            switch (digitInOnesColumn)
            {
                case 0:
                    return $"{Number.ToString(Format)}th";
                case 1:
                    return $"{Number.ToString(Format)}st";
                case 2:
                    return $"{Number.ToString(Format)}nd";
                case 3:
                    return $"{Number.ToString(Format)}rd";
                case 4:
                    return $"{Number.ToString(Format)}th";
                case 5:
                    return $"{Number.ToString(Format)}th";
                case 6:
                    return $"{Number.ToString(Format)}th";
                case 7:
                    return $"{Number.ToString(Format)}th";
                case 8:
                    return $"{Number.ToString(Format)}th";
                case 9:
                    return $"{Number.ToString(Format)}th";
                default:
                    throw new Exception($"{Number} digit not supported.");
            }
        }


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


        [UsedImplicitly]
        public static ListDiff<TSid> CalculateDiff<T, TSid>(this List<T> OriginalList, List<T> UpdatedList, Func<T, TSid> GetSid) => OriginalList.CalculateDiff(UpdatedList, GetSid, GetSid);


        [UsedImplicitly]
        public static ListDiff<TSid> CalculateDiff<TOriginal, TUpdated, TSid>(this List<TOriginal> OriginalList, List<TUpdated> UpdatedList,
            Func<TOriginal, TSid> GetOriginalSid, Func<TUpdated, TSid> GetUpdatedSid)
        {
            ListDiff<TSid> diff = new ListDiff<TSid>();
            // Extract the SIDs from the original and updated lists into two HashSets.
            HashSet<TSid> originalSids = new HashSet<TSid>();
            foreach (var item in OriginalList) originalSids.Add(GetOriginalSid(item));
            HashSet<TSid> updatedSids = new HashSet<TSid>();
            foreach (TUpdated item in UpdatedList) updatedSids.Add(GetUpdatedSid(item));
            // Determine which items have been added to the updated list.
            foreach (TSid sid in updatedSids) if (!originalSids.Contains(sid)) diff.Added.Add(sid);
            // Determine which items remain in the updated list and which were deleted from the updated list.
            foreach (TSid sid in originalSids)
            {
                if (updatedSids.Contains(sid)) diff.Remaining.Add(sid); // Item remains in updated list.
                else diff.Removed.Add(sid); // Item was removed from updated list.
            }
            return diff;
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