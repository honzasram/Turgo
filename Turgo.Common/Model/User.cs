using System.ComponentModel;
using System.Xml.Serialization;
using Sramek.FX;

namespace Turgo.Common.Model
{
    /// <summary>
    /// player
    /// </summary>
    public class User : ObservableObject,ISelectable, IChangeable
    {
        [XmlIgnore]
        private static int _no;
        [XmlIgnore]
        public static int No
        {
            get { return _no++; }
            set { _no = value; }
        }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Surname { get; set; }
        [XmlAttribute]
        public uint ID { get; set; }

        [XmlIgnore]
        private bool mIsSelected;


        [XmlIgnore]
        public bool IsSelected
        {
            get { return mIsSelected; }
            set
            {
                if (mIsSelected == value) return;
                mIsSelected = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public bool IsChanged { get; set; }
    }
}