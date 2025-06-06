using System;
using System.Text;

namespace OmoSeitokuEreceipt.SER
{
    [Serializable]
    public sealed class SinryouViewerSettings
    {
        public string Name { get; set; }
        public bool 選択歯完全一致 { get; set; }
        public bool Checked病名P { get; set; }
        public bool Checked病名G { get; set; }
        public bool Checked病名義歯 { get; set; }
        /// <summary>
        /// SinryouFilter.除外種別_除外, SinryouFilter.除外種別_のみ
        /// </summary>
        public int 病名FilterType { get; set; }
        public SinryouDataLoader.診療統合種別 診療統合 { get; set; }
        public ShinryouOrderType 並び順Type { get; set; }

        public SinryouViewerSettings(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public SinryouViewerSettings()
        {
        }

        public string Serialize()
        {
            var sb = new StringBuilder();
            var serializer = new System.Xml.Serialization.XmlSerializer(GetType());
            using (var writer = new System.IO.StringWriter(sb))
            {
                serializer.Serialize(writer, this);
            }
            return sb.ToString();
        }
    }
}
