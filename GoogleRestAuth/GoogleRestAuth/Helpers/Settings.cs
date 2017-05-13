// Copyright (c) Tomasz Romaszkiewicz. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.

namespace GoogleRestAuth.Helpers
{
    using Windows.Storage;

    public static class Settings
    {
        /// <summary>
        /// Checks if value with given key exists in LocalSettings
        /// </summary>
        /// <param name="key">given key</param>
        /// <returns>true if value exists</returns>
        public static bool Contains(string key) => ApplicationData.Current.LocalSettings.Values.ContainsKey(key);

        /// <summary>
        /// Reads value from LocalSettings
        /// </summary>
        /// <param name="key">given key</param>
        /// <returns>value</returns>
        public static object Read(string key) => Contains(key) ? ApplicationData.Current.LocalSettings.Values[key] : null;

        /// <summary>
        /// Removes value with given key from LocalSettings
        /// </summary>
        /// <param name="key">used key</param>
        /// <returns>true if value has been removed</returns>
        public static bool Remove(string key) => ApplicationData.Current.LocalSettings.Values.Remove(key);

        /// <summary>
        /// Saves value in LocalSettings
        /// </summary>
        /// <param name="key">used key</param>
        /// <param name="value">saved value</param>
        public static void Save(string key, object value) => ApplicationData.Current.LocalSettings.Values[key] = value;

        /// <summary>
        /// Reads value from settings if exists, otherwise returns a defined default
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="key">key used in LocalSettings</param>
        /// <param name="defaultValue">defined default value</param>
        /// <returns>value from LocalSettings</returns>
        public static T ReadOrDefault<T>(string key, T defaultValue) => Contains(key) ? (T)ApplicationData.Current.LocalSettings.Values[key] : defaultValue;

        /// <summary>
        /// Reads value from setitngs if it exists, otherwise creates a new one
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="key">key used in LocalSettings</param>
        /// <param name="defaultValue">defined default value</param>
        /// <returns>value from LocalSettings</returns>
        public static T ReadOrCreate<T>(string key, T defaultValue)
        {
            if (Contains(key)) return (T)ApplicationData.Current.LocalSettings.Values[key];
            Save(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// Reads value from LocalSettings then removes it
        /// </summary>
        /// <param name="key">key used in LocalSettings</param>
        /// <returns>value from LocalSettings</returns>
        public static object ReadAndRemove(string key)
        {
            object value = Read(key);
            if (value != null) Remove(key);
            return value;
        }
    }
}
