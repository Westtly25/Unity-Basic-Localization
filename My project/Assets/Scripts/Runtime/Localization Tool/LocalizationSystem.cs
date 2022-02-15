using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Threading;
using System;

namespace RogueLike.Localization
{
    public class LocalizationSystem
    {
        public static Language language;
        public static CultureInfo culture;
        public static readonly string defaultCulture = "en";
        public static Dictionary<string, string> localisedDictionary;
        public static Dictionary<string, string> localisedEN;
        public static Dictionary<string, string> localisedFR;
        public static Dictionary<string, string> localisedRU;
        public static Dictionary<string, string> localisedUA;

        public static event Action OnLanguageChanged;

        public static bool isInit;
        public static CSVLoader cSVLoader;

        public static void Init()
        {
            cSVLoader = new CSVLoader();
            cSVLoader.LoadCSV();

            GetLanguage();
            GetCurrentCultureInfo();

            UpdateDictionaries();

            isInit = true;
        }

        public static void UpdateDictionaries()
        {
            if(cSVLoader.GetDictionaryValues(culture.TwoLetterISOLanguageName) == null)
            {
                Debug.LogWarning("there are no data for this language, loading default language...");
                localisedDictionary = cSVLoader.GetDictionaryValues(defaultCulture);
            }else
            {
                localisedDictionary = cSVLoader.GetDictionaryValues(culture.TwoLetterISOLanguageName);
            }

            localisedEN = cSVLoader.GetDictionaryValues("en");
            localisedFR = cSVLoader.GetDictionaryValues("fr");
            localisedRU = cSVLoader.GetDictionaryValues("ru");
            localisedUA = cSVLoader.GetDictionaryValues("ua");
        }

        public static void GetCurrentCultureInfo()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo currentCultureUI = Thread.CurrentThread.CurrentUICulture;

            if(currentCulture == currentCultureUI)
            {
                culture = currentCulture;
            }

            Debug.Log("Current Culture is : " + currentCulture);
            Debug.Log("Current Culture UI is : " + currentCultureUI);
            Debug.Log("Current Culture Two Letters is : " + currentCulture.TwoLetterISOLanguageName);
        }

        public static void OnCurrentCultureInfoChanged(CultureInfo cultureInfo)
        {
            localisedDictionary = new Dictionary<string, string>();

            UpdateDictionaries();

            OnLanguageChanged?.Invoke();
        }

        private static void GetLanguage()
        {
            //if()
            //{
                //language = 
            //}

            language = Language.English;
        }

        public static string GetLocalisedValue(string key)
        {
            if(!isInit) { Init(); }

            string value = key;

            localisedDictionary.TryGetValue(key, out value);

            switch(language)
            {
                case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
                case Language.French:
                localisedFR.TryGetValue(key, out value);
                break;
                case Language.Russian:
                localisedRU.TryGetValue(key, out value);
                break;
                case Language.Ukrainian:
                localisedUA.TryGetValue(key, out value);
                break;
            }

            return value;
        }

#if UNITY_EDITOR

        public static void Add(string key,  string value)
        {
            if(value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if(cSVLoader == null)
            {
                cSVLoader = new CSVLoader();
            }

            cSVLoader.LoadCSV();
            cSVLoader.Add(key, value);
            cSVLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Replace(string key, string value)
        {
            if(value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if(cSVLoader == null)
            {
                cSVLoader = new CSVLoader();
            }

            cSVLoader.LoadCSV();
            cSVLoader.Edit(key, value);
            cSVLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Remove(string key)
        {
            if(cSVLoader == null)
            {
                cSVLoader = new CSVLoader();
            }

            cSVLoader.LoadCSV();
            cSVLoader.Remove(key);
            cSVLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if(!isInit)
            {
                Init();
            }

            return localisedEN;
        }
#endif
    }
}