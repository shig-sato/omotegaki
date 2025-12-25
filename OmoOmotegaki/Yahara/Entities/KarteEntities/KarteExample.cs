#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    public static class KarteExample
    {
        public static void TestSerialization(string outputDir)
        {
            var karte = CreateSample();
            var xml = karte.ToXML();
            Console.WriteLine("=== カルテシリアライゼーションテスト ===");
            Console.WriteLine(xml);

            var path = Path.Combine(outputDir, "Karte_debug.xml");
            File.WriteAllText(path, xml + "\n");
            Console.WriteLine($"\nXMLファイルを保存しました: {path}");
        }

        private static Day CreateDay20200908()
        {
            // <Day Date="2020/09/08" DateNo="0" Doctor="担当医 氏名">
            //   <Bui No="0" Teeth="- - -" Doctor="担当医 氏名">
            //     <Treatment Point="261" Count="1">初診料</Treatment>
            //   </Bui>
            //   <Bui No="1" Teeth="- - - ┘#1 └#1 └#3 └#4 └#5 └#6" Doctor="担当医 氏名">
            //     <Disease Name="Ｐ" />
            //     <Treatment Point="0" Count="0">欠損歯登録</Treatment>
            //     <Treatment Point="402" Count="1">断層撮影（パノラマ）ｵﾙｿﾊﾟﾝﾄﾓ型（電子化）</Treatment>
            //     <Treatment Point="0" Count="0">歯槽骨の吸収が認められる｡</Treatment>
            //     <Treatment Point="50" Count="1">歯周基本検査(10歯未満)</Treatment>
            //     <Treatment Point="80" Count="1">歯科疾患管理料(初診月)</Treatment>
            //     <Treatment Point="10" Count="1">文書提供加算(歯科疾患管理)</Treatment>
            //     <Treatment Point="80" Count="1">歯科衛生実地指導料１</Treatment>
            //     <Treatment Point="70" Count="1">機械的歯面清掃処置 (ラバーカップ､研磨用クリーム)</Treatment>
            //     <Treatment Point="0" Count="0">歯清1回目</Treatment>
            //   </Bui>
            // </Day>

            return new Day()
            {
                Date = "2020/09/08",
                DateNo = "0",
                Doctor = "担当医 氏名",
                Buis =  {
                new Bui()
                    {
                      No = "0",
                      Teeth = "- - -",
                      Doctor = "担当医 氏名",
                      Treatments = {
                        new Treatment()
                        {
                          Point = "261",
                          Count = "1",
                          Name = "初診料"
                        }
                    }
                    },
                    new Bui()
                    {
                      No = "1",
                      Teeth = "- - - ┘#1 └#1 └#3 └#4 └#5 └#6",
                      Doctor = "担当医 氏名",
                      Diseases ={
                        new Disease() { Name = "Ｐ" }
                      },
                      Treatments = {
                        new Treatment()
                        {
                          Point = "0",
                          Count = "0",
                          Name = "欠損歯登録"
                        },
                        new Treatment()
                        {
                          Point = "402",
                          Count = "1",
                          Name = "断層撮影（パノラマ）ｵﾙｿﾊﾟﾝﾄﾓ型（電子化）"
                        },
                        new Treatment()
                        {
                          Point = "0",
                          Count = "0",
                          Name = "歯槽骨の吸収が認められる｡"
                        },
                        new Treatment()
                        {
                          Point = "50",
                          Count = "1",
                          Name = "歯周基本検査(10歯未満)"
                        },
                        new Treatment()
                        {
                          Point = "80",
                          Count = "1",
                          Name = "歯科疾患管理料(初診月)"
                        },
                        new Treatment()
                        {
                          Point = "10",
                          Count = "1",
                          Name = "文書提供加算(歯科疾患管理)"
                        },
                        new Treatment()
                        {
                          Point = "80",
                          Count = "1",
                          Name = "歯科衛生実地指導料１"
                        },
                        new Treatment()
                        {
                          Point = "70",
                          Count = "1",
                          Name = "機械的歯面清掃処置 (ラバーカップ､研磨用クリーム)"
                        },
                        new Treatment()
                        {
                          Point = "0",
                          Count = "0",
                          Name = "歯清1回目"
                        }
                        }
                    },
                }
            };
        }
        private static Day CreateDay20200918()
        {
            // <Day Date="2020/09/18" DateNo="0" Doctor="担当医 氏名">
            //   <Bui No="0" Teeth="- - -" Doctor="担当医 氏名">
            //     <Treatment Point="53" Count="1">再診料</Treatment>
            //     <Treatment Point="1" Count="1">明細書発行体制等加算</Treatment>
            //     <Treatment Point="0" Count="0">症状・所見：</Treatment>
            //   </Bui>
            //   <Bui No="1" Teeth="- - - └#1 └#3" Doctor="担当医 氏名">
            //     <Disease Name="" />
            //     <Treatment Point="1100" Count="1">レーザー使用料</Treatment>
            //   </Bui>
            //   <Bui No="2" Teeth="- - - └#1 └#3" Doctor="担当医 氏名">
            //     <Disease Name="Ｐｅｒ" />
            //     <Treatment Point="228" Count="2">感根即充(1根) (Ｇ-ｐｏｉｎｔ+Ｃａｎ) (ＮＣ+ＦＣ+ＥＺ)</Treatment>
            //     <Treatment Point="30" Count="2">ＥＭＲ(1根)</Treatment>
            //     <Treatment Point="136" Count="2">加圧根管充填処置（１根）</Treatment>
            //     <Treatment Point="48" Count="1">D×１（確認）</Treatment>
            //     <Treatment Point="86" Count="2">窩洞形成(KP)(複雑)</Treatment>
            //     <Treatment Point="212" Count="1" InnerBui="- - - └#1">支台築造(ﾌｧｲﾊﾞｰﾎﾟｽﾄ)(直接)前歯</Treatment>
            //     <Treatment Point="0" Count="0">(ﾌｧｲﾊﾞｰﾎﾟｽﾄ+ﾚｼﾞﾝｺｱ)</Treatment>
            //     <Treatment Point="212" Count="1" InnerBui="- - - └#3">支台築造(ﾌｧｲﾊﾞｰﾎﾟｽﾄ)(直接)前歯</Treatment>
            //     <Treatment Point="0" Count="0">(ﾌｧｲﾊﾞｰﾎﾟｽﾄ+複合ﾚｼﾞﾝ)</Treatment>
            //     <Treatment Point="0" Count="1">ＥＥ・ＥＢ</Treatment>
            //     <Treatment Point="158" Count="2">充填１(複雑)</Treatment>
            //     <Treatment Point="29" Count="4">光重合ＣＲ(複雑) &lt;M,D&gt;</Treatment>
            //     <Treatment Point="0" Count="0">└13１歯２窩洞</Treatment>
            //     <Treatment Point="0" Count="1">研磨</Treatment>
            //   </Bui>
            // </Day>

            return new Day
            {
                Date = "2020/09/18",
                DateNo = "0",
                Doctor = "担当医 氏名",
                Buis =
          {
            new Bui()
            {
              No = "0",
              Teeth = "- - -",
              Doctor = "担当医 氏名",
              Treatments =               {
                new Treatment()
                {
                  Point = "53",
                  Count = "1",
                  Name = "再診料"
                },
                new Treatment()
                {
                  Point = "1",
                  Count = "1",
                  Name = "明細書発行体制等加算"
                },
                new Treatment()
                {
                  Point = "0",
                  Count = "0",
                  Name = "症状・所見："
                }
              }
            },
            new Bui()
            {
              No = "1",
              Teeth = "- - - └#1 └#3",
              Doctor = "担当医 氏名",
              Diseases =              {
                new Disease()
                {
                  Name = ""
                }
              },
              Treatments =   {
                new Treatment()
                {
                  Point = "1100",
                  Count = "1",
                  Name = "レーザー使用料"
                }
              }
            },
            new Bui()
            {
              No = "2",
              Teeth = "- - - └#1 └#3",
              Doctor = "担当医 氏名",
              Diseases = {
                new Disease()
                {
                  Name = "Ｐｅｒ"
                }
              },
              Treatments =
                {
                new Treatment()
                {
                  Point = "228",
                  Count = "2",
                  Name = "感根即充(1根) (Ｇ-ｐｏｉｎｔ+Ｃａｎ) (ＮＣ+ＦＣ+ＥＺ)"
                },
                new Treatment()
                {
                  Point = "30",
                  Count = "2",
                  Name = "ＥＭＲ(1根)"
                },
                new Treatment()
                {
                  Point = "136",
                  Count = "2",
                  Name = "加圧根管充填処置（１根）"
                },
                new Treatment()
                {
                  Point = "48",
                  Count = "1",
                  Name = "D×１（確認）"
                },
                new Treatment()
                {
                  Point = "86",
                  Count = "2",
                  Name = "窩洞形成(KP)(複雑)"
                },
                new Treatment()
                {
                  Point = "212",
                  Count = "1",
                  InnerBui = "- - - └#1",
                  Name = "支台築造(ﾌｧｲﾊﾞｰﾎﾟｽﾄ)(直接)前歯"
                },
                new Treatment()
                {
                  Point = "0",
                  Count = "0",
                  Name = "(ﾌｧｲﾊﾞｰﾎﾟｽﾄ+ﾚｼﾞﾝｺｱ)"
                },
                new Treatment()
                {
                  Point="212",
                  Count="1",
                  InnerBui="- - - └#3",
                  Name = "支台築造(ﾌｧｲﾊﾞｰﾎﾟｽﾄ)(直接)前歯"
                },
                new Treatment()
                {
                  Point="0",
                  Count="0",
                  Name = "(ﾌｧｲﾊﾞｰﾎﾟｽﾄ+複合ﾚｼﾞﾝ)"
                },
                new Treatment()
                {
                  Point="0",
                  Count="1",
                  Name = "ＥＥ・ＥＢ"
                },
                new Treatment()
                {
                  Point="158",
                  Count="2",
                  Name = "充填１(複雑)"
                },
                new Treatment()
                {
                  Point="29",
                  Count="4",
                  Name = "光重合ＣＲ(複雑) <M,D>"
                },
                new Treatment()
                {
                  Point="0",
                  Count="0",
                  Name = "└13１歯２窩洞"
                },
                new Treatment()
                {
                  Point="0",
                  Count="1",
                  Name = "研磨"
                },
                }
            }
          }
            };
        }

        private static Karte CreateSample()
        {
            var days = new List<Day>
        {
          CreateDay20200908(),
          CreateDay20200918()
        };

            return new Karte
            {
                KarteNo = "7282",
                Name = "テスト 太郎",
                Doctor = "担当医 氏名",
                Days = days
            };
        }


    }
}
