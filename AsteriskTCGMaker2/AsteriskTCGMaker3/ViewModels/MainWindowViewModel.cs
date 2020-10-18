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
using System.Windows.Media.Effects;
using System.Windows.Controls.Primitives;
using AsteriskTCGMaker3.Views;
using System.Windows.Threading;
using System.Windows.Input;

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

        public string CreateDeleateCardName { get; set; } = "CardName";

        private string _statusText = "";
        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public string CardEffect
        {
            get
            {
                return SelectedCard.CardEffect;
            }
            set
            {
                SelectedCard.CardEffect = value;
                OnPropertyChanged(nameof(EffectDocument));
                OnPropertyChanged(nameof(CardEffect));
            }

        }

        public string FlavorText
        {
            get
            {
                return SelectedCard.FlavorText;
            }
            set
            {
                SelectedCard.FlavorText = value;
                OnPropertyChanged(nameof(EffectDocument));
                OnPropertyChanged(nameof(FlavorText));
            }

        }


        public double EffectFontSize
        {
            get
            {
                return SelectedCard.EffectFontSize;
            }
            set
            {
                SelectedCard.EffectFontSize = value;
                OnPropertyChanged(nameof(EffectDocument));
                OnPropertyChanged(nameof(EffectFontSize));
            }

        }


        public double FlavorFontSize
        {
            get
            {
                return SelectedCard.FlavorFontSize;
            }
            set
            {
                SelectedCard.FlavorFontSize = value;
                OnPropertyChanged(nameof(EffectDocument));
                OnPropertyChanged(nameof(FlavorFontSize));
            }

        }

        private int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }


        private void createRunEffectText(Paragraph paragraph, string effectText, double effectFontSize)
        {
            //*でかこまれたものを太字にする
            if (CountChar(effectText, '*') % 2 == 0)//偶数個含まれる場合
            {
                var loopMode = 0;
                while (effectText != "")
                {

                    //*マークまで切り出す（存在しなければ最後まで）
                    var atPlace = effectText.IndexOf("*");
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

                    var effectRuns = new Run();
                    effectRuns.Text = str;
                    effectRuns.FontFamily = new FontFamily("HGS ゴシックM");
                    effectRuns.FontSize = effectFontSize;

                    if (loopMode == 0)
                    {//通常のテキストに関する処理                       
                        paragraph.Inlines.Add(effectRuns);
                    }
                    else
                    {//太字にする部分
                        paragraph.Inlines.Add(new Bold(effectRuns));
                    }


                    loopMode = (loopMode + 1) % 2;

                }

            }
            else
            {//奇数個含まれる場合
                var effectRuns = new Run();
                effectRuns.FontFamily = new FontFamily("HGS ゴシックM");
                effectRuns.FontSize = effectFontSize;
                paragraph.Inlines.Add(effectRuns);
            }

        }

        public FlowDocument createText()
        {

            var effectText = SelectedCard.CardEffect;
            var effectFontSize = SelectedCard.EffectFontSize;
            var flavorText = SelectedCard.FlavorText;
            var flavorFontSize = SelectedCard.FlavorFontSize;

            var outputDocument = new FlowDocument();
            var paragraph = new Paragraph();



            var flavorRuns = new Run();
            flavorRuns.Text = flavorText;
            flavorRuns.FontFamily = new FontFamily("HGS 明朝B");
            flavorRuns.FontSize = flavorFontSize;

            //指定の文字を指定のアイコンに変換する
            effectText = Regex.Replace(effectText, @"\[DT\]", "@DT@");
            effectText = Regex.Replace(effectText, @"\[常\]", "@jo@");
            effectText = Regex.Replace(effectText, @"\[起\]", "@ki@");
            effectText = Regex.Replace(effectText, @"\[自\]", "@ji@");
            effectText = Regex.Replace(effectText, @"\[出\]", "@syu@");
            effectText = Regex.Replace(effectText, @"\(黒\)", "@K@");
            effectText = Regex.Replace(effectText, @"\(青\)", "@B@");
            effectText = Regex.Replace(effectText, @"\(赤\)", "@R@");
            effectText = Regex.Replace(effectText, @"\(黄\)", "@Y@");
            effectText = Regex.Replace(effectText, @"\(白\)", "@W@");
            effectText = Regex.Replace(effectText, @"\(緑\)", "@G@");
            effectText = Regex.Replace(effectText, @"\(無\)", "@N@");

            //機種依存文字を利用しても、見栄えを重視する
            effectText = Regex.Replace(effectText, "□", "◻");



            //@でかこまれたものをオブジェクトにする
            if (CountChar(effectText, '@') % 2 == 0)//@が偶数個含まれる場合
            {
                var loopMode = 0;
                while (effectText != "")
                {

                    //@マークまで切り出す（存在しなければ最後まで）
                    var atPlace = effectText.IndexOf("@");
                    string str;
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
                        createRunEffectText(paragraph, str, effectFontSize);
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
            {
                createRunEffectText(paragraph, effectText, effectFontSize);
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




        FlowDocument _effectDocument = new FlowDocument();
        public FlowDocument EffectDocument
        {
            get
            {
                _effectDocument = createText();
                return _effectDocument;
            }
            set
            {
                _effectDocument = value;
                OnPropertyChanged(nameof(EffectDocument));
            }
        }





        private ObservableCollection<CardData> _cardList;
        public ObservableCollection<CardData> CardList
        {
            get
            {
                if (_cardList == null)
                {
                    _cardList = new ObservableCollection<CardData>();
                    SetCardList();
                }
                return _cardList;
            }
        }

        private void SetCardList()
        {
            _cardList.Clear();
            try
            {
                foreach (string fileName in Directory.GetFiles(Singleton.Instance.CardPath, "*.atcg"))
                {
                    try
                    {
                        _cardList.Add(new CardData(fileName));
                    }
                    catch (Exception e)
                    {

                    }
                }
                _cardList = new ObservableCollection<CardData>(
                _cardList.OrderBy(elm => elm.CostColor1)
                .ThenByDescending(elm => elm.SteraSpell)
                .ThenByDescending(elm => elm.CostMana1 + elm.CostMana2)
                .ThenByDescending(elm => elm.Power));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            OnPropertyChanged(nameof(CardList));
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
                if (value != _selectedCard)
                {
                    _selectedCard = value;
                }
                this.OnPropertyChanged(nameof(SelectedCard));
                this.OnPropertyChanged(nameof(CardEffect));
                this.OnPropertyChanged(nameof(FlavorText));
                this.OnPropertyChanged(nameof(EffectFontSize));
                this.OnPropertyChanged(nameof(FlavorFontSize));
                this.OnPropertyChanged(nameof(EffectDocument));

            }
        }

        private int _selectedNo = 0;
        public int SelectedNo
        {
            get { return _selectedNo; }
            set
            {
                _selectedNo = value;
                if (value >= 0) SelectedCard = CardList[value];
                OnPropertyChanged(nameof(SelectedNo));
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

        private ViewModelCommand _createNewCardCommand;
        public ViewModelCommand CreateNewCardCommand
        {
            get
            {
                if (this._createNewCardCommand == null)
                {
                    this._createNewCardCommand = new ViewModelCommand(() =>
                    {
                        if (File.Exists(Singleton.Instance.CardPath + "\\" + CreateDeleateCardName + ".atcg"))
                        {
                            StatusText = CreateDeleateCardName + "はすでに存在します";
                        }
                        else
                        {
                            var card = new CardData();
                            card.CardName = CreateDeleateCardName;
                            SelectedCard = card;
                            Save(SelectedCard);
                            SetCardList();

                            for (var i = 0; i < CardList.Count; i++)
                            {
                                if (CardList[i].CardName != card.CardName) continue;
                                SelectedNo = i;
                                break;
                            }
                        }
                    });
                }
                return this._createNewCardCommand;

            }

        }

        private ViewModelCommand _reloadListCommand;
        public ViewModelCommand ReloadListCommand
        {
            get
            {
                if (this._reloadListCommand == null)
                {
                    this._reloadListCommand = new ViewModelCommand(() =>
                    {
                        SetCardList();
                    });
                }
                return this._reloadListCommand;

            }

        }


        private ViewModelCommand _deleteCardCommand;
        public ViewModelCommand DeleteCardCommand
        {
            get
            {
                if (this._deleteCardCommand == null)
                {
                    this._deleteCardCommand = new ViewModelCommand(() =>
                    {
                        try
                        {
                            StatusText = CreateDeleateCardName + "は存在しませんでした";
                            if (File.Exists(Singleton.Instance.CardPath + "\\" + CreateDeleateCardName + ".atcg"))
                            {
                                File.Delete(Singleton.Instance.CardPath + "\\" + CreateDeleateCardName + ".atcg");
                                StatusText = CreateDeleateCardName + "を削除しました";
                            }
                            if (File.Exists(Singleton.Instance.CardPath + "\\" + CreateDeleateCardName + ".png"))
                            {
                                File.Delete(Singleton.Instance.CardPath + "\\" + CreateDeleateCardName + ".png");
                                StatusText = CreateDeleateCardName + "を削除しました";
                            }
                        }
                        catch (Exception e)
                        {
                            StatusText = CreateDeleateCardName + "の削除に失敗しました";
                        }
                        SetCardList();
                        SelectedCard = new CardData();
                    });
                }
                return this._deleteCardCommand;

            }

        }



        private ViewModelCommand _printCommand;
        public ViewModelCommand PrintCommand
        {
            get
            {
                if (this._printCommand == null)
                {
                    this._printCommand = new ViewModelCommand(() =>
                    {
                        PrintWindow printWin = new PrintWindow();
                        printWin.Show();
                    });
                }
                return this._printCommand;

            }

        }

        private string getOutputText(bool WithInf, bool clipMode)
        {
            /*
                  ,フォントサイズルビ,カード名,効果,フレーバーテキスト,テキストサイズ,β版か,バースト有無,リアクション有無,シールドトリガー有無,スペルステップ有無,
                  ,色ルビ,カード名,効果,フレーバーテキスト,メイン色,サブ色,カードルビそのもの,
                     カード名
                     [色][ステラ/スペル]《種族（ステラのみ）》
                     〔黒:2　無:4〕（ただし無が0の場合〔黒:2〕）
                     パワー:5500　BR:1（）ステラのみ/【永続スペル】（スペルのみ）

                    効果

                    フレーバーテキスト

                    illustration:
                     */

            var text = "";
            if (WithInf)
            {
                text += ";" + SelectedCard.Version.ToString();
                text += ";" + SelectedCard.RubyFontSize.ToString();
                text += ";" + SelectedCard.CardNameFontSize.ToString();
                text += ";" + SelectedCard.EffectFontSize.ToString();
                text += ";" + SelectedCard.FlavorFontSize.ToString();
                text += ";" + SelectedCard.TextBoxSize.ToString();
                text += ";" + SelectedCard.Beta.ToString();
                text += ";" + SelectedCard.Burst.ToString();
                text += ";" + SelectedCard.ReAction.ToString();
                text += ";" + SelectedCard.SealedTrigger.ToString();
                text += ";" + SelectedCard.SpellStep.ToString();
                text += ";" + SelectedCard.KeepSpell.ToString();
                text += ";" + "\r\n";
                text += ";" + SelectedCard.RubyColor.ToString();
                text += ";" + SelectedCard.NameColor.ToString();
                text += ";" + SelectedCard.EffectTextColor.ToString();
                text += ";" + SelectedCard.FlavorTextColor.ToString();
                text += ";" + SelectedCard.MainColorColor.ToString();
                text += ";" + SelectedCard.SubColorColor.ToString();
                text += ";" + SelectedCard.CardRuby.ToString();
                text += ";" + "\r\n";
            }


            text += SelectedCard.CardName.ToString();
            text += "\r\n";
            if (SelectedCard.SteraSpell == "ステラ")
            {
                text += "[" + SelectedCard.CostColor1.ToString() + "][ステラ]《" + SelectedCard.Kind1.ToString() + "》";
                if (SelectedCard.Kind2.ToString() != "") text += "《" + SelectedCard.Kind2.ToString() + "》";
            }
            else
            {
                text += "[" + SelectedCard.CostColor1.ToString() + "][スペル]";
            }
            text += "\r\n";
            text += "〔" + SelectedCard.CostColor1 + ":" + SelectedCard.CostMana1;
            if (SelectedCard.CostMana2 == "0") text += "〕\r\n";
            else text += "　" + SelectedCard.CostColor2 + ":" + SelectedCard.CostMana2 + "〕\r\n";
            if (SelectedCard.SteraSpell == "ステラ")
            {
                text += "パワー:" + SelectedCard.Power.ToString() + "　BR:" + SelectedCard.BR.ToString() + "\r\n"; ;
            }

            if (clipMode)
            {
                if (SelectedCard.Burst) text += "【バースト】";
                if (SelectedCard.ReAction) text += "【リアクション】";
                if (SelectedCard.SealedTrigger) text += "【ダメージトリガー】";
                if (SelectedCard.SpellStep) text += "【スペルステップ】";
                if (SelectedCard.KeepSpell) text += "【永続スペル】";
                //if (SelectedCard.Burst || SelectedCard.ReAction || SelectedCard.SealedTrigger || SelectedCard.SpellStep || SelectedCard.KeepSpell) text += "\r\n";
            }
            text += "\r\n\r\n";
            if (clipMode == true)
            {
                text += Regex.Replace(SelectedCard.CardEffect.ToString(), "\n　", "\n");
                text += "\r\n\r\n";
                var flavor = SelectedCard.FlavorText.ToString();
                while (flavor.Length >= 1 && (flavor.Substring(0, 1) == "　" || flavor.Substring(0, 1) == " "))
                {
                    flavor = flavor.Substring(1, flavor.Length - 1);
                }
                text += flavor;
                text += "\r\n\r\nIllustraion:";
                text += SelectedCard.Illustration.ToString();
            }
            else
            {
                text += SelectedCard.CardEffect.ToString() + "\r\n\r\n" + SelectedCard.FlavorText.ToString() + "\r\n\r\n" + SelectedCard.Illustration.ToString();
            }

            return text;
        }

        private ViewModelCommand _savetCommand;
        public ViewModelCommand SaveCommand
        {
            get
            {
                if (this._savetCommand == null)
                {
                    this._savetCommand = new ViewModelCommand(() =>
                    {
                        if (File.Exists(Singleton.Instance.CardPath + "\\" + SelectedCard.CardName + ".atcg"))
                        {
                            Save(SelectedCard);
                        }
                        else
                        {
                            Save(SelectedCard);
                        }


                    });
                }
                return this._savetCommand;

            }

        }
        private void Save(CardData card)
        {
            try
            {
                File.WriteAllText(Singleton.Instance.CardPath + card.CardName + ".atcg", getOutputText(true, false));

                StatusText = "セーブしました（" + Singleton.Instance.CardPath + card.CardName + ".atcg）";
            }
            catch (Exception e)
            {
                StatusText = "セーブに失敗しました（" + Singleton.Instance.CardPath + card.CardName + ".atcg/" + SelectedCard.ImageSource + "）";
            }


            try
            {
                if (File.Exists(card.ImageSource))
                {
                    File.Copy(card.ImageSource, Singleton.Instance.CardPath + card.CardName + ".png", true);
                }
            }
            catch (Exception e)
            {
                //StatusText = "画像のみセーブに失敗しました（" + SelectedCard.ImageSource + "）";
            }

        }


        private ListenerCommand<object> _outputCommand;
        public ListenerCommand<object> OutputCommand
        {
            get
            {
                if (this._outputCommand == null)
                {
                    this._outputCommand = new ListenerCommand<object>((elem) =>
                    {
                        var inputPath = Singleton.Instance.Path + "Result/Input.png";
                        var outputName = SelectedCard.CardName;
                        try
                        {
                            StatusText = "画像出力中...";
                            doEvent();



                            //ファイル名に使えない文字を削除
                            outputName = outputName.Replace(":", "");
                            outputName = outputName.Replace("：", "");


                            var outputPath1 = System.IO.Path.Combine(Singleton.Instance.Path, "Result\\big\\" + SelectedCard.CostColor1 + "\\" + outputName + ".png");
                            var outputPath2 = System.IO.Path.Combine(Singleton.Instance.Path, "Result\\small\\" + SelectedCard.CostColor1 + "\\" + outputName + ".png");
                            var outputPath3 = System.IO.Path.Combine(Singleton.Instance.Path, "Result\\Output.png");
                            if (File.Exists(outputPath1)) System.IO.File.Delete(outputPath1);
                            if (File.Exists(outputPath2)) System.IO.File.Delete(outputPath2);

                            Directory.CreateDirectory(Singleton.Instance.Path + "Result\\big\\" + SelectedCard.CostColor1);
                            Directory.CreateDirectory(Singleton.Instance.Path + "Result\\small\\" + SelectedCard.CostColor1);
                            SaveImage((FrameworkElement)elem, outputPath1, 10);
                            SaveImage((FrameworkElement)elem, outputPath2, 1);
                            SaveImage((FrameworkElement)elem, outputPath3, 10);
                            StatusText = "出力完了（" + outputPath1 + "）";
                        }
                        catch (Exception e)
                        {
                            StatusText = "出力失敗";
                        }
                    });
                }
                return this._outputCommand;
            }
        }

        //指定要素を画像として保存する
        private void SaveImage(FrameworkElement target, string path, int resolutionRate = 10)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("pathが未設定");

            // レンダリング
            // ディスプレイDPIのn倍の解像度で保存する
            var dpi = 96;



            // PresentationSource source = PresentationSource.FromVisual(this);
            var bmp = new RenderTargetBitmap(
                (int)(target.ActualWidth * resolutionRate),
                (int)(target.ActualHeight * resolutionRate),
                dpi * resolutionRate, dpi * resolutionRate, // DPI
                PixelFormats.Pbgra32);
            bmp.Render(target);

            // pngで保存
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (var fs = File.Open(path, FileMode.Create))
            {
                encoder.Save(fs);
            }

        }

        //画面を明示的に更新
        void doEvent()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback((ExitFrames) =>
            {
                ((DispatcherFrame)ExitFrames).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }




        private ViewModelCommand _copyToClipBoardCommand;
        public ViewModelCommand CopyToClipBoardCommand
        {
            get
            {
                if (this._copyToClipBoardCommand == null)
                {
                    this._copyToClipBoardCommand = new ViewModelCommand(() =>
                    {
                        Clipboard.SetText(getOutputText(false, true));
                        StatusText = "クリップボードにコピーしました";
                    });
                }
                return this._copyToClipBoardCommand;

            }

        }


        private ViewModelCommand _pasteFromClipBoardCommand;
        public ViewModelCommand PasteFromClipBoardCommand
        {
            get
            {
                if (this._pasteFromClipBoardCommand == null)
                {
                    this._pasteFromClipBoardCommand = new ViewModelCommand(TextPaste);
                }
                return this._pasteFromClipBoardCommand;

            }

        }

        private void TextPaste()
        {
            try
            {
                SelectedCard.TextPaste(Clipboard.GetText());
                StatusText = "クリップボードから貼り付けました";
            }
            catch (Exception e)
            {
                StatusText = "貼り付けに失敗しました";


            }
            this.OnPropertyChanged(nameof(SelectedCard));
            this.OnPropertyChanged(nameof(CardEffect));
            this.OnPropertyChanged(nameof(FlavorText));
            this.OnPropertyChanged(nameof(EffectFontSize));
            this.OnPropertyChanged(nameof(FlavorFontSize));
            this.OnPropertyChanged(nameof(EffectDocument));

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
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!((bool)value)) return Binding.DoNothing;
            else return parameter;
        }
    }


    /// <summary>
    /// 種族がからの場合はハイフンにするコンバーター
    /// </summary>
    public class KindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == "") return "-";
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
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
        public string SteraSpell { get; set; }
        public string Kind1 { get; set; }
        public string Kind2 { get; set; } = "";
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
        public int Version { get; set; } = 3000;
        public string CardRuby { get; set; } = "";
        public double EffectFontSize { get; set; } = 8;
        public double FlavorFontSize { get; set; } = 6;
        public bool Burst { get; set; }
        public bool ReAction { get; set; }
        public bool SealedTrigger { get; set; }
        public bool SpellStep { get; set; }
        public bool KeepSpell { get; set; }
        public double TextBoxSize { get; set; } = 50;
        public bool Beta { get; set; } = false;
        public string ImageSource { get; set; }
        public BitmapImage Source
        {
            get
            {
                if (ImageSource == null) return null;
                BitmapImage bmpImage = new BitmapImage();
                FileStream stream = File.OpenRead(ImageSource);
                bmpImage.BeginInit();
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.StreamSource = stream;
                bmpImage.EndInit();
                stream.Close();
                return bmpImage;
            }
        }
        public double RubyFontSize { get; set; } = 4;
        public double CardNameFontSize { get; set; } = 10;

        public string RubyColor { get; set; } = "white";
        public string NameColor { get; set; } = "white";
        public string EffectTextColor { get; set; } = "white";
        public string FlavorTextColor { get; set; } = "white";
        public string MainColorColor { get; set; } = "white";
        public string SubColorColor { get; set; } = "white";

        public void TextPaste(string text)
        {
            Burst = false;
            ReAction = false;
            SealedTrigger = false;
            SpellStep = false;
            KeepSpell = false;
            SetInf(text);
            if (File.Exists(Singleton.Instance.CardPath + "\\" + CardName + ".png")) ImageSource = Singleton.Instance.CardPath + "\\" + CardName + ".png";
        }

        private void SetInf(string text)
        {
            //カード名
            CardName = text.Substring(0, text.IndexOf("\r\n"));
            text = text.Substring(text.IndexOf("\r\n") + 2, text.Length - (text.IndexOf("\r\n") + 2));

            //[色][ステラ/スペル]《種類1》《種類2》
            MatchCollection matches1 = Regex.Matches(text, @"\[.*?\]");
            MatchCollection matches2 = Regex.Matches(text, @"《.*?》");
            //Color = matches1[0].Value.Substring(1, matches1[0].Value.Length - 2);
            SteraSpell = matches1[1].Value.Substring(1, matches1[1].Value.Length - 2);
            Kind1 = "";
            Kind2 = "";
            if (matches2.Count >= 1) Kind1 = matches2[0].Value.Substring(1, matches2[0].Value.Length - 2);
            if (matches2.Count >= 2) Kind2 = matches2[1].Value.Substring(1, matches2[1].Value.Length - 2);
            text = text.Substring(text.IndexOf("\r\n") + 2, text.Length - (text.IndexOf("\r\n") + 2));

            //〔色:コスト 色:コスト〕
            text = text.Substring(text.IndexOf("〔") + 1, text.Length - (text.IndexOf("〔") + 1));
            CostColor1 = text.Substring(0, text.IndexOf(":"));
            text = text.Substring(text.IndexOf(":") + 1, text.Length - (text.IndexOf(":") + 1));
            if (text.Substring(0, text.IndexOf("〕")).IndexOf("　") == -1)
            {
                CostMana1 = text.Substring(0, text.IndexOf("〕"));
                CostColor2 = "無";
                CostMana2 = "0";
            }

            else
            {
                CostMana1 = text.Substring(0, text.IndexOf("　"));
                CostColor2 = text.Substring(text.IndexOf("　") + 1, text.IndexOf(":") - (text.IndexOf("　") + 1));
                text = text.Substring(text.IndexOf(":") + 1, text.Length - (text.IndexOf(":") + 1));
                CostMana2 = text.Substring(0, text.IndexOf("〕"));
            }
            text = text.Substring(text.IndexOf("\r\n") + 2, text.Length - (text.IndexOf("\r\n") + 2));

            //[Power；パワー量 BR:BR量]（ステラのみ）
            if (SteraSpell == "ステラ")
            {
                text = text.Substring(text.IndexOf(":") + 1, text.Length - (text.IndexOf(":") + 1));
                Power = text.Substring(0, text.IndexOf("　"));
                text = text.Substring(text.IndexOf(":") + 1, text.Length - (text.IndexOf(":") + 1));
                BR = text.Substring(0, text.IndexOf("\r\n"));
                text = text.Substring(text.IndexOf("\r\n") + 2, text.Length - (text.IndexOf("\r\n") + 2));
            }


            //【永続スペル】などの表記+二回改行
            var spelltype = text.Substring(0, text.IndexOf("\r\n"));
            SpellType = spelltype;
            if (spelltype.IndexOf("【バースト】") >= 0) Burst = true;
            if (spelltype.IndexOf("【リアクション】") >= 0) ReAction = true;
            if (spelltype.IndexOf("【ダメージトリガー】") >= 0) SealedTrigger = true;
            if (spelltype.IndexOf("【スペルステップ】") >= 0) SpellStep = true;
            if (spelltype.IndexOf("【永続スペル】") >= 0) KeepSpell = true;


            text = text.Substring(text.IndexOf("\r\n\r\n") + 4, text.Length - (text.IndexOf("\r\n\r\n") + 4));
            //効果
            CardEffect = text.Substring(0, text.IndexOf("\r\n\r\n"));
            //フレーバーテキスト
            FlavorText = text.Substring(text.IndexOf("\r\n\r\n") + 4, text.LastIndexOf("\r\n\r\n") - (text.IndexOf("\r\n\r\n") + 4));


            text = text.Substring(text.IndexOf("\r\n\r\n") + 4, text.Length - (text.IndexOf("\r\n\r\n") + 4));
            //イラスト
            Illustration = Regex.Replace(text.Substring(text.IndexOf("\r\n\r\n") + 4, text.Length - (text.IndexOf("\r\n\r\n") + 4)), "Illustraion:", "");
        }

        public CardData()
        {
            CardName = "";
            CardEffect = "";
            SteraSpell = "";
            Kind1 = "";
            Kind2 = "";
            CostColor1 = "黒";
            CostMana1 = "";
            CostColor2 = "無";
            CostMana2 = "";
            Power = "";
            BR = "1";
            SpellType = "";
            FlavorText = "";
            Illustration = "";
            CardRuby = "";
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

        private string getNextElement(string Str)
        {
            return Str.Substring(1, Str.IndexOf(";", 1) - 1);
        }

        private void WikiToData(string WikiData)
        {
            /*
             ,フォントサイズルビ,カード名,効果,フレーバーテキスト,テキストサイズ,β版か,バースト有無,リアクション有無,シールドトリガー有無,スペルステップ有無,
             ,色ルビ,カード名,効果,フレーバーテキスト,メイン色,サブ色,カードルビそのもの,リソース
             カード名
             [色][ステラ/スペル]《種類1》《種類2》
            〔色:コスト 色:コスト〕
            パワー:パワー量 BR:BR量/スペル種類
            効果
            フレーバーテキスト
            Illustration:イラスト
             */

            Version = int.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            RubyFontSize = double.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            CardNameFontSize = double.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            EffectFontSize = double.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            FlavorFontSize = double.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            TextBoxSize = double.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));

            Beta = bool.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            Burst = bool.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            ReAction = bool.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            SealedTrigger = bool.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            SpellStep = bool.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            KeepSpell = bool.Parse(getNextElement(WikiData));
            WikiData = WikiData.Substring(WikiData.IndexOf("\r\n") + 2, WikiData.Length - (WikiData.IndexOf("\r\n") + 2));

            RubyColor = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            NameColor = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            EffectTextColor = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            FlavorTextColor = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            MainColorColor = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            SubColorColor = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf(";", 1), WikiData.Length - (WikiData.IndexOf(";", 1)));
            CardRuby = getNextElement(WikiData);
            WikiData = WikiData.Substring(WikiData.IndexOf("\r\n") + 2, WikiData.Length - (WikiData.IndexOf("\r\n") + 2));

            SetInf(WikiData);
            if (File.Exists(Singleton.Instance.CardPath + "\\" + CardName + ".png")) ImageSource = Singleton.Instance.CardPath + "\\" + CardName + ".png";
        }

    }

}

