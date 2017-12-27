using System.Collections.Generic;
using System.Xml.Serialization;
using Sramek.FX;

namespace Turgo.Common.Model
{
    /// <summary>
    /// rocnik
    /// </summary>
    public class Class : ObservableObject
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Year { get; set; }
        public List<User> UserBase { get; set; }
        public List<Round> Rounds { get; set; }

        [XmlIgnore]
        private bool mFinished;
        [XmlAttribute]
        public bool Finished
        {
            get { return mFinished; }
            set
            {
                if (mFinished == value) return;
                mFinished = value;
                OnPropertyChanged();
                SetShowName();
            }
        }

        [XmlIgnore]
        private string mShowName;
        [XmlIgnore]
        public string ShowName
        {
            get
            {
                if(string.IsNullOrEmpty(mShowName)) SetShowName();
                return mShowName;
            }
            set
            {
                if (mShowName == value) return;
                mShowName = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        private bool mSelected;
        [XmlIgnore]
        public bool Selected
        {
            get { return mSelected; }
            set
            {
                if (mSelected == value) return;
                mSelected = value;
                SetShowName();
                OnPropertyChanged();
            }
        }

        private void SetShowName()
        {
            ShowName = $"{Name} - {Year}: {Rounds?.Count} kol" + (Finished?" Ukončena":"") + (Selected?" Vybrána":"");
        } 
    }
}