namespace Sramek.FX.WPF.ViewModel
{
    public class TabContent
    {
        public string Header { get; }

        public object Content { get; }

        public TabContent(string aHeader, object aContent)
        {
            Header = aHeader;
            Content = aContent;
        }
    }
}