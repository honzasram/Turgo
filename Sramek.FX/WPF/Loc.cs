using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using log4net;
namespace Sramek.FX.WPF
{
    public class DefaultAttribute : Attribute
    {
        public string Default { get; private set; }
        public bool Ignore { get; private set; }

        public DefaultAttribute(string aDefault, bool aIgnore = false)
        {
            Default = aDefault;
            Ignore = aIgnore;
        }
    }

    public class LocalizationBase : ObservableObject
    {
        protected Dictionary<string, string> mSelectedLanguageDictionary = new Dictionary<string, string>();
        protected Dictionary<string, string> mDefaultDictionary = new Dictionary<string, string>();
        protected Dictionary<string, Dictionary<string,string>> mLocalizationDictionary = new Dictionary<string, Dictionary<string, string>>();
        protected ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Assembly.FullName);
        private List<PropertyInfo> mPropertiesCache = new List<PropertyInfo>();

        [Default(null, true)]
        public string CultureName { get; set; } = null;
        
        public LocalizationBase()
        {
            GetProperties();
            CreateDefault();
            SetLanguage(null);
        }

        private void GetProperties()
        {
            mPropertiesCache = GetType().GetProperties()
                .Where(a => a.PropertyType == typeof(string) 
                    && a.GetCustomAttributes(typeof(DefaultAttribute), true).Any(b=> !((b as DefaultAttribute)?.Ignore ?? true)))
                .ToList();
        }

        private void CreateDefault()
        {
            foreach (var iProp in mPropertiesCache)
            {
                mDefaultDictionary.Add(iProp.Name, iProp.Name);
            }
        }
        
        public void SetLanguage(string aLoc)
        {
            if (aLoc == null)
            {
                mSelectedLanguageDictionary = mDefaultDictionary;
            }
            else if (mLocalizationDictionary.ContainsKey(aLoc))
            {
                mSelectedLanguageDictionary = mLocalizationDictionary[aLoc];
            }
            else
            {
                throw new Exception($"Unable to load localization {aLoc}. Check your locallication file.");
            }

            ApplyDictionary();
            OnPropertyChanged();
        }

        private void ApplyDictionary()
        {
            foreach (var iProperty in mPropertiesCache)
            {
                try
                {
                    iProperty.SetValue(this, mSelectedLanguageDictionary[iProperty.Name]);
                }
                catch (Exception e)
                {
                    mLog.Info(iProperty.Name);
                    mLog.Error(Messages.BuildErrorMessage(e));
                }
            }
        }

        //public void AddDictionary(string aLocName, Dictionary<string,string> aDict)
        //{
        //    if (!mLocalizationDictionary.ContainsKey(aLocName))
        //        mLocalizationDictionary.Add(aLocName, aDict);
        //    else
        //        mLocalizationDictionary[aLocName] = aDict;
        //}
    }

    public class Loc<T> : CsvLocalization
        where T: Loc<T>, new()
    {
        public static T I { get; set; } = new T();
        
        public void Load()
        {
            Load(true);
        }
    }

    public class CsvLocalization : LocalizationBase
    {
        private readonly string mCsvFilePath;

        public CsvLocalization()
        {
            //var lAssemblyPath = Path.GetFullPath(new Uri(Assembly.GetAssembly(GetType()).CodeBase).LocalPath);
            //var lDirectoryPath = Path.GetDirectoryName(lAssemblyPath);
            //var lAssemblyNameWithoutExt = Path.GetFileNameWithoutExtension(lAssemblyPath);
            //mCsvFilePath = Path.Combine(lDirectoryPath, lAssemblyNameWithoutExt) + "Localization.csv";
            mCsvFilePath = "Loc.csv";
        }
        
        protected void Load(bool aUpdateFile)
        {
            try
            {
                mLocalizationDictionary.Clear();
                var lKeyHashSet = new HashSet<string>(mSelectedLanguageDictionary.Keys);
                var lLocFilePath = mCsvFilePath;
                var lFileLines = new List<string>();
                var lNeedUpdate = false;

                if (File.Exists(lLocFilePath))
                {
                    using (var lSr = new StreamReader(lLocFilePath, Encoding.UTF8))
                    {
                        string lLine;
                        while ((lLine = lSr.ReadLine()) != null)
                        {
                            lFileLines.Add(lLine.Trim().Trim());
                        }
                    }
                }

                if (lFileLines.Count == 0)
                {
                    mLog.WarnFormat("No data in localization file \"{0}\" !", lLocFilePath);
                }
                else
                {
                    var lHeader = lFileLines[0];
                    var lColumns = lHeader.Split(';').Select(a => a.Trim()).Where(a => a.Length != 0).ToArray();
                    if (lColumns.Length < 2)
                    {
                        mLog.WarnFormat("Not enough columns in localization file \"{0}\" !", lLocFilePath);
                        return;
                    }

                    if (lColumns[0] != "Key")
                    {
                        mLog.WarnFormat("First column name \"{0}\" is not keyword \"Key\" in localization file \"{1}\" !", lColumns[0], lLocFilePath);
                    }

                    if (lColumns[1] != "Default")
                    {
                        mLog.WarnFormat("Second column name \"{0}\" is not keyword \"Default\" in localization file \"{1}\" !", lColumns[1], lLocFilePath);
                    }

                    var lCultureHashSet = new HashSet<string>(CultureInfo.GetCultures(CultureTypes.AllCultures).Select(a => a.Name));
                    var lDictList = new List<Dictionary<string, string>>();
                    for (int iColumn = 2; iColumn < lColumns.Length; iColumn++)
                    {
                        var nColumn = lColumns[iColumn];
                        if (!lCultureHashSet.Contains(nColumn))
                        {
                            mLog.WarnFormat(
                                "Unexpected column name \"{0}\" in localization file \"{1}\" ! It is not a valid culture name !", nColumn,
                                lLocFilePath);
                            return;
                        }

                        var lDict = new Dictionary<string, string>();
                        mLocalizationDictionary.Add(nColumn, lDict);
                        lDictList.Add(lDict);
                    }

                    for (int iLine = 1; iLine < lFileLines.Count; iLine++)
                    {
                        var nLine = lFileLines[iLine];

                        // preskoceni prazdneho radku nebo komentare
                        if (nLine.Length == 0 || nLine.StartsWith("//")) continue;

                        var lValues = nLine.Split(';').Select(a => a.Trim()).ToArray();

                        // preskoceni radku s neznamym klicem
                        if (!lKeyHashSet.Remove(lValues[0]))
                        {
                            mLog.WarnFormat("Unknown key \"{0}\" at line {1} in localization file \"{2}\" !", lValues[0], iLine + 1,
                                            lLocFilePath);
                            continue;
                        }

                        // preskoceni neuplnych radku
                        if (lValues.Length != lColumns.Length)
                        {
                            mLog.WarnFormat("Unexpected number of columns at line {0} in localization file \"{1}\" !", iLine + 1, lLocFilePath);
                            continue;
                        }

                        for (int iItem = 2; iItem < lValues.Length; iItem++)
                        {
                            if (lDictList[iItem - 2].ContainsKey(lValues[0]))
                            {
                                mLog.WarnFormat("Duplicate key at line {0} in localization file \"{1}\" !", iLine + 1, lLocFilePath);
                                break;
                            }
                            lDictList[iItem - 2].Add(lValues[0], lValues[iItem]);
                            if (lValues[1] != mSelectedLanguageDictionary[lValues[0]])
                            {
                                lValues[1] = mSelectedLanguageDictionary[lValues[0]];
                                var lSb = new StringBuilder();
                                foreach (var nValue in lValues)
                                {
                                    lSb.Append(nValue + ";");
                                }
                                lFileLines[iLine] = lSb.ToString();
                                lNeedUpdate = true;
                            }
                        }
                    }
                }

                if (lFileLines.Count == 0)
                {
                    lFileLines.Add("Key;Default");
                    lNeedUpdate = true;
                }

                // add missing keys
                if (lKeyHashSet.Count > 0)
                {
                    lFileLines.Add("// automatically appended lines at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    lFileLines.AddRange(lKeyHashSet.Select(a => $"{a};{mSelectedLanguageDictionary[a]}"));
                    lNeedUpdate = true;
                }

                // update file
                lNeedUpdate = false;
                if (lNeedUpdate)
                {
                    try
                    {
                        using (var lSw = new StreamWriter(lLocFilePath, false, Encoding.UTF8))
                        {
                            foreach (var nLine in lFileLines)
                            {
                                lSw.WriteLine(nLine);
                            }
                        }
                    }
                    catch (Exception lEx)
                    {
                        mLog.WarnFormat("Couldn't update localization file \"{0}\" !", lEx, lLocFilePath);
                    }
                }
            }
            catch (Exception lEx)
            {
                mLog.WarnFormat("Couldn't read localization file ! Exception: {0}", Messages.BuildErrorMessage(lEx));
            }

            SetLanguage(CultureName);
        }
    }
}