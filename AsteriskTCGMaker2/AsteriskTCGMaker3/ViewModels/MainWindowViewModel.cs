using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using AsteriskTCGMaker3.Models;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Windows.Controls;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows;
using Microsoft.Win32;

namespace AsteriskTCGMaker3.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
        }
        private DelegateCommand _openFileCommand;
        /// <summary>
        /// ファイルを開くコマンドを取得します。
        /// </summary>
        public DelegateCommand OpenFileCommand
        {
            get
            {
                return this._openFileCommand ?? (this._openFileCommand = new DelegateCommand(
                _ =>
                {
                    System.Diagnostics.Debug.WriteLine("ファイルを開きます。");
                }));
            }
        }

        private double _cardNameFontSize = 10;
        public double CardNameFontSize
        {
            get { return this._cardNameFontSize; }
            set
            {
                if (value >= 100) return; this._cardNameFontSize = value;
                OnPropertyChanged(nameof(CardNameFontSize));
            }
        }

        private byte _cardRubyFontSize = 4;
        public byte CardRubyFontSize
        {
            get { return this._cardRubyFontSize; }
            set
            {
                if (value >= 100) return; this._cardRubyFontSize = value;
                OnPropertyChanged(nameof(CardRubyFontSize));
            }
        }

        private double _cardEffectFontSize = 8;
        public double CardEffectFontSize
        {
            get { return this._cardEffectFontSize; }
            set
            {
                if (value >= 100) return; this._cardEffectFontSize = value;
                OnPropertyChanged(nameof(CardEffectFontSize));
            }
        }

        private int _cardEffectHeight = 45;
        public int CardEffectHeight
        {
            get { return this._cardEffectHeight; }
            set
            {
                this._cardEffectHeight = value;
                OnPropertyChanged(nameof(CardEffectHeight));
            }
        }

        private string _cardRubyColor = "White";
        public string CardRubyColor
        {
            get { return this._cardRubyColor; }
            set
            {
                this._cardRubyColor = value;
                OnPropertyChanged(nameof(CardRubyColor));
            }
        }

        private string _cardNameColor = "White";
        public string CardNameColor
        {
            get { return this._cardNameColor; }
            set
            {
                this._cardNameColor = value;
                OnPropertyChanged(nameof(CardNameColor));
            }
        }

        private string _cardEffectColor = "Black";
        public string CardEffectColor
        {
            get { return this._cardEffectColor; }
            set
            {
                this._cardEffectColor = value;
                OnPropertyChanged(nameof(CardEffectColor));
            }

        }

        private string _cardManaColor = "White";
        public string CardManaColor
        {
            get { return this._cardManaColor; }
            set
            {
                this._cardManaColor = value;
                OnPropertyChanged(nameof(CardManaColor));
            }
        }

        private string _cardSubManaColor = "White";
        public string CardSubManaColor
        {
            get { return this._cardSubManaColor; }
            set
            {
                this._cardSubManaColor = value;
                OnPropertyChanged(nameof(CardSubManaColor));
            }
        }


        private ObservableCollection<CardData> _cardList = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> CardList
        {
            get
            {
                if (_cardList.Count == 0)
                {

                    /*  try
                      {*/
                    foreach (string fileName in Directory.GetFiles(Singleton.Instance.CardPath, "*.atcg"))
                    {
                        _cardList.Add(new CardData(fileName));
                    }
                    /*  }
                      catch (Exception e)
                      {
                          Console.WriteLine(e.ToString());
                      }*/


                }
                return _cardList;
            }
        }

        private CardData _selectedCard = new CardData();
        public CardData SelectedCard
        {
            get
            {
                return _selectedCard;
            }
            set
            {
                _selectedCard = value;
                this.OnPropertyChanged(nameof(SelectedCard));
            }
        }

        private ViewModelCommand _graphicInsertCommand;
        public ViewModelCommand GraphicInsertCommand
        {
            get
            {
                if (this._graphicInsertCommand == null)
                {
                    this._graphicInsertCommand = new ViewModelCommand(GraphicInsert);//this利用のため別関数とする
                }
                return this._graphicInsertCommand;

            }

        }


       

      


        private void GraphicInsert()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "ファイルを開く";
            dialog.Filter = "全てのファイル(*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                SelectedCard.ImageSource = dialog.FileName;
                this.OnPropertyChanged(nameof(SelectedCard));
            }


        }
    }


    /// <summary>
    /// ある色情報を含んでいるか返すコンバーター
    /// </summary>
    public class BoolColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == parameter.ToString()) return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return parameter;
        }
    }


    public class BoolVisibillityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible) return true;
            else return false;
        }
    }

    public class BRToVisibillityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var BR = Int64.Parse(value.ToString());
            var para = Int64.Parse(parameter.ToString());
            if (BR >= para) return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class SteraSpellToVisibillity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == parameter.ToString()) return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }



    /// <summary>
    /// Color-Color1Source
    /// </summary>
    public class ColorToColor1SourceConverter : IValueConverter
    {
        private IDictionary<string, ImageSource> _color1SourceStera = new Dictionary<string, ImageSource>() {
            {"黒",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Main_Black.png", UriKind.Absolute)) },
            {"青",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Main_Blue.png", UriKind.Absolute))},
            {"緑",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Main_Green.png", UriKind.Absolute))},
            {"赤",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Main_Red.png", UriKind.Absolute))},
            {"白",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Main_White.png", UriKind.Absolute))},
            {"黄",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Main_Yellow.png", UriKind.Absolute))}
        };
        private IDictionary<string, ImageSource> _color1SourceSpell = new Dictionary<string, ImageSource>() {
            {"黒",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Only_Black.png", UriKind.Absolute)) },
            {"青",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Only_Blue.png", UriKind.Absolute))},
            {"緑",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Only_Green.png", UriKind.Absolute))},
            {"赤",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Only_Red.png", UriKind.Absolute))},
            {"白",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Only_White.png", UriKind.Absolute))},
            {"黄",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Only_Yellow.png", UriKind.Absolute))}
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.ToString() == "ステラ") return _color1SourceStera[value.ToString()];
            else return _color1SourceSpell[value.ToString()];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }


    /// <summary>
    /// Color-Color2Source
    /// </summary>
    public class ColorToColor2SourceConverter : IValueConverter
    {
        private IDictionary<string, ImageSource> _color2Source = new Dictionary<string, ImageSource>() {
            {"黒",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_Black.png", UriKind.Absolute)) },
            {"青",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_Blue.png", UriKind.Absolute))},
            {"緑",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_Green.png", UriKind.Absolute))},
            {"赤",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_Red.png", UriKind.Absolute))},
            {"白",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_White.png", UriKind.Absolute))},
            {"黄",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_Yellow.png", UriKind.Absolute))},
            {"無",new BitmapImage(new Uri(Singleton.Instance.Path + "Resources/Sub_Less.png", UriKind.Absolute))},
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return _color2Source[value.ToString()];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Color-Color2Source
    /// </summary>
    public class BoxSizeToBoxBottomConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Int64.Parse(value.ToString()) + 12;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class IsSteraConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() == "ステラ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return "ステラ";
            else return "スペル";
        }
    }

    public class SteraSpellToTextBoxBottom : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == "ステラ") return 55;
            else return 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }


    public class IsSpellConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() == "スペル";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return "スペル";
            else return "ステラ";
        }
    }


    public class ValueEqualParaToBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return parameter.ToString();
            else return "";
        }
    }

    public class ValueEqualParaToVisibillityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == parameter.ToString()) return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }


    public class CardToEffectTextConverter : IMultiValueConverter
    {
        private int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var effectText = values[0].ToString();
            var effectFontSize = double.Parse(values[1].ToString());
            var flavorText = values[2].ToString();
            var flavorFontSize = double.Parse(values[3].ToString());

            var outputDocument = new FlowDocument();
            var paragraph = new Paragraph();



            var flavorRuns = new Run();
            flavorRuns.Text = flavorText;
            flavorRuns.FontFamily = new FontFamily("HGS 明朝B");
            flavorRuns.FontSize = flavorFontSize;

            if (CountChar(effectText, '@') % 2 == 0)//@が偶数個含まれる場合
            {
                var loopMode = 0;
                while (effectText != "")
                {

                    //@マークまで切り出す（存在しなければ最後まで）
                    var atPlace = effectText.IndexOf("@");
                    var str = "";
                    if (atPlace >= 0)
                    {
                        str = effectText.Substring(0, atPlace);
                        effectText = effectText.Substring(atPlace + 1, effectText.Length - atPlace - 1);
                    }
                    else
                    {
                        str = effectText;
                        effectText = "";
                    }
                    if (loopMode == 0)
                    {//通常のテキストに関する処理
                     //Run情報を生成する
                        var effectRuns = new Run();
                        effectRuns.Text = str;
                        effectRuns.FontFamily = new FontFamily("HGS ゴシックM");
                        effectRuns.FontSize = effectFontSize;
                        paragraph.Inlines.Add(effectRuns);
                    }
                    else
                    {//アイコンに関する処理

                        if (File.Exists(Singleton.Instance.Path + "Icon/" + str + ".png") == true)
                        {
                            Image image = new Image();
                            image.Width = effectFontSize;
                            image.Height = effectFontSize;
                            image.Stretch = Stretch.Uniform;
                            image.Source = new BitmapImage(new Uri(Singleton.Instance.Path + "Icon/" + str + ".png", UriKind.Absolute));
                            InlineUIContainer icon = new InlineUIContainer();
                            icon.Child = image;
                            paragraph.Inlines.Add(icon);
                        }

                    }


                    loopMode = (loopMode + 1) % 2;

                }

            }
            else
            {//@が奇数個含まれる場合
                var effectRuns = new Run();
                effectRuns.FontFamily = new FontFamily("HGS ゴシックM");
                effectRuns.FontSize = effectFontSize;

                flavorRuns.Text = effectText;
                paragraph.Inlines.Add(effectRuns);
            }

            var gapRuns = new Run();

            gapRuns.FontFamily = new FontFamily("HGS ゴシックM");
            gapRuns.FontSize = 3;
            gapRuns.Text = "\n    あ   \n";
            gapRuns.Foreground = new SolidColorBrush(Colors.Transparent);


            //paragraph.Inlines.Add(effectRuns);
            paragraph.Inlines.Add(gapRuns);
            paragraph.Inlines.Add(flavorRuns);




            outputDocument.Blocks.Add(paragraph);



            return outputDocument;
        }

        public object[] ConvertBack(object value, Type[] types, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
    public class BindableRichTextBox : RichTextBox
    {
        #region 依存関係プロパティ
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new UIPropertyMetadata(null, OnRichTextItemsChanged));
        #endregion  // 依存関係プロパティ

        #region 公開プロパティ
        public new FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }
        #endregion  // 公開プロパティ

        #region イベントハンドラ
        private static void OnRichTextItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as RichTextBox;
            if (control != null)
            {
                control.Document = e.NewValue as FlowDocument;
            }
        }
        #endregion  // イベントハンドラ
    }

    class CardData
    {
        //Wiki&内部データ
        public string CardName { get; set; }
        public string CardEffect { get; set; }
        public string Color { get; set; }
        public string SteraSpell { get; set; }
        public string Kind1 { get; set; }
        public string Kind2 { get; set; }
        public string CostColor1 { get; set; }
        public string CostMana1 { get; set; }
        public string CostColor2 { get; set; }
        public string CostMana2 { get; set; }
        public string Power { get; set; }
        public string BR { get; set; } = "0";
        public string SpellType { get; set; }
        public string FlavorText { get; set; }
        public string Illustration { get; set; }

        //内部データ
        public string CardRuby { get; set; }
        public Double EffectFontSize { get; set; } = 8;
        public Double FlavorFontSize { get; set; } = 6;
        public bool Burst { get; set; }
        public bool ReAction { get; set; }
        public bool SealedTrigger { get; set; }
        public bool SpellStep { get; set; }
        public Double TextBoxSize { get; set; } = 50;
        public bool Beta { get; set; } = false;
        public string ImageSource { get; set; }

        public CardData()
        {
            CardName = "Name";
            CardEffect = "Effect";
            Color = "赤";
            SteraSpell = "ステラ";
            Kind1 = "死神";
            Kind2 = "";
            CostColor1 = "黒";
            CostMana1 = "1";
            CostColor2 = "無";
            CostMana2 = "5";
            Power = "5000";
            BR = "1";
            SpellType = "永続スペル";
            FlavorText = "FlavorText";
            Illustration = "Illustration:翡翠 蒼輝";
            CardRuby = "Ruby";
        }

        public CardData(string filePath)
        {
            CardName = Path.GetFileName(filePath);
            using (var reader = new StreamReader(filePath))
            {
                string text = reader.ReadToEnd();
                WikiToData(text);
            }
        }

        private void WikiToData(string WikiData)
        {
            /*
             カード名
             [色][ステラ/スペル]《種類》
            〔色:コスト 色:コスト〕
            パワー:パワー量 BR:BR量/スペル種類
            効果
            フレーバーテキスト
            Illustration:イラスト
             */

            //カード名
            CardName = WikiData.Substring(0, WikiData.IndexOf("\r\n"));
            WikiData = WikiData.Substring(WikiData.IndexOf("\r\n") + 2, WikiData.Length - (WikiData.IndexOf("\r\n") + 2));

            //[色][ステラ/スペル]《種類》
            MatchCollection matches1 = Regex.Matches(WikiData, @"\[.*?\]");
            MatchCollection matches2 = Regex.Matches(WikiData, @"《.*?》");
            Color = matches1[0].Value.Substring(1, matches1[0].Value.Length - 2);
            SteraSpell = matches1[1].Value.Substring(1, matches1[1].Value.Length - 2);
            if (matches2.Count == 0) Kind1 = "";
            else Kind1 = matches2[0].Value.Substring(1, matches2[0].Value.Length - 2);

            WikiData = WikiData.Substring(WikiData.IndexOf("\r\n") + 2, WikiData.Length - (WikiData.IndexOf("\r\n") + 2));

            //〔色:コスト 色:コスト〕
            WikiData = WikiData.Substring(WikiData.IndexOf("〔") + 1, WikiData.Length - (WikiData.IndexOf("〔") + 1));
            CostColor1 = WikiData.Substring(0, WikiData.IndexOf(":"));
            WikiData = WikiData.Substring(WikiData.IndexOf(":") + 1, WikiData.Length - (WikiData.IndexOf(":") + 1));
            if (WikiData.IndexOf("　") == -1)
            {
                CostMana1 = WikiData.Substring(0, WikiData.IndexOf("〕"));
                CostColor2 = "無";
                CostMana2 = "0";
            }

            else
            {
                CostMana1 = WikiData.Substring(0, WikiData.IndexOf("　"));
                CostColor2 = WikiData.Substring(WikiData.IndexOf("　") + 1, WikiData.IndexOf(":") - (WikiData.IndexOf("　") + 1));
                WikiData = WikiData.Substring(WikiData.IndexOf(":") + 1, WikiData.Length - (WikiData.IndexOf(":") + 1));
                CostMana2 = WikiData.Substring(0, WikiData.IndexOf("〕"));
            }
            WikiData = WikiData.Substring(WikiData.IndexOf("\r\n") + 2, WikiData.Length - (WikiData.IndexOf("\r\n") + 2));

            //[Power；パワー量 BR:BR量]/スペル種類
            switch (SteraSpell)
            {
                case "ステラ":
                    WikiData = WikiData.Substring(WikiData.IndexOf(":") + 1, WikiData.Length - (WikiData.IndexOf(":") + 1));
                    Power = WikiData.Substring(0, WikiData.IndexOf("　"));
                    WikiData = WikiData.Substring(WikiData.IndexOf(":") + 1, WikiData.Length - (WikiData.IndexOf(":") + 1));
                    BR = WikiData.Substring(0, WikiData.IndexOf("\r\n"));
                    WikiData = WikiData.Substring(WikiData.IndexOf("\r\n\r\n") + 4, WikiData.Length - (WikiData.IndexOf("\r\n\r\n") + 4));
                    //効果
                    CardEffect = WikiData.Substring(0, WikiData.IndexOf("\r\n\r\n"));
                    //フレーバーテキスト
                    FlavorText = WikiData.Substring(WikiData.IndexOf("\r\n\r\n") + 4, WikiData.LastIndexOf("Illustration:") - 4 - (WikiData.IndexOf("\r\n\r\n") + 4));
                    break;

                case "スペル":
                    SpellType = WikiData.Substring(0, WikiData.IndexOf("\r\n"));
                    WikiData = WikiData.Substring(WikiData.IndexOf("\r\n\r\n") + 4, WikiData.Length - (WikiData.IndexOf("\r\n\r\n") + 4));
                    //効果
                    CardEffect = WikiData.Substring(0, WikiData.IndexOf("\r\n\r\n"));
                    //フレーバーテキスト
                    FlavorText = "";
                    break;
            }

            //イラスト
            Illustration = WikiData.Substring(WikiData.IndexOf("Illustration:") + 13, WikiData.Length - (WikiData.LastIndexOf("Illustration:") + 13));
        }

    }
}
