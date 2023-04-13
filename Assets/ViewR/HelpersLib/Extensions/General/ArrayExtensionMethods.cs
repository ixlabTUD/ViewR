using System;

namespace ViewR.HelpersLib.Extensions.General
{
    public static class ArrayExtensionMethods
    {
        /// <summary>
        /// Finds the first occurrence. 
        /// </summary>
        public static int FindIndex<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item);
        }

        // timeSpans avg: 00:00:00.0013061
        public static T[] AppendOneDimensionalArray<T>(this T[] array, T item)
        {
            // Ensure it exists
            if (array == null)
                return new[] {item};

            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;

            return array;
        }

        // timeSpans avg: 00:00:00.0012866
        public static T[] Append<T>(this T[] array, T item)
        {
            // Ensure it exists
            if (array == null)
                return new[] {item};

            var result = new T[array.Length + 1];
            array.CopyTo(result, 0);
            result[array.Length] = item;
            
            return result;
        }

        //timeSpans avg: 00:00:00.0051213
        //public static T[] Append<T> (this T[] array, T item) {
        //	if (array == null) {
        //		return new T[] {
        //			item
        //		};
        //	}
        //	return array.Concat(new T[] {
        //		item
        //	}).ToArray();
        //}
    }
}