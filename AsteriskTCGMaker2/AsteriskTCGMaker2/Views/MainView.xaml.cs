using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AsteriskTCGMaker2.Views
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>

    public partial class MainView : Window
    {
        private string path;
        public MainView()
        {
            path = System.AppDomain.CurrentDomain.BaseDirectory;
            InitializeComponent();
            InitializeContents();

        }
        private void InitializeContents()
        {

            CardFlame.Source = new BitmapImage(new Uri(path + "Resources/CardFlame.png", UriKind.Absolute));
            CardImage.Source = new BitmapImage(new Uri(path + "Resources/InputSample.png", UriKind.Absolute));
            CardEffectImageDown.Source = new BitmapImage(new Uri(path + "Resources/TextBoxDown.png", UriKind.Absolute));
            CardEffectImageUp.Source = new BitmapImage(new Uri(path + "Resources/TextBoxUp.png", UriKind.Absolute));
            CardEffectImageCenter.Source = new BitmapImage(new Uri(path + "Resources/TextBoxCenter.png", UriKind.Absolute));
            CardPowerImage.Source = new BitmapImage(new Uri(path + "Resources/PowerFlame.png", UriKind.Absolute));
            CardBreakImage1.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage2.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage3.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage4.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage5.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage6.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage7.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage8.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));
            CardBreakImage9.Source = new BitmapImage(new Uri(path + "Resources/Break.png", UriKind.Absolute));



        }



        private void Button_Output(object sender, RoutedEventArgs e)
        {

            var inputPath = path + "Result/Input.png";
            var outputName = CardNameText.Text;
            outputName = outputName.Replace(":", "");
            outputName = outputName.Replace("：", "");
            var outputPath1 = System.IO.Path.Combine(path, "Result/" + outputName + ".png");
            var outputPath2 = System.IO.Path.Combine(path, "Result/Output.png");
            var fullPath = System.IO.Path.Combine(path, "Result/All.png");
            System.IO.File.Delete(outputPath1);

            try
            {
                SaveImage(this.InputGrid, inputPath);

                SaveImage(this.FullGrid, fullPath);
                SaveImage(this.OutputGrid, outputPath2);
                SaveImage(this.OutputGrid, outputPath1);

                MessageBox.Show("画像出力が完了しました。", "出力完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("ファイル保存に失敗しました。\n他のアプリケーションで画像ファイルを開いていないか確認してください。", "画像出力エラー", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }


        private void Button_Print(object sender, RoutedEventArgs e)
        {

            PrintWindow printWin = new PrintWindow();
            printWin.Owner = this;
            printWin.Show();

        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "ファイルを開く";
            dialog.Filter = "全てのファイル(*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                MemoryStream data = new MemoryStream(File.ReadAllBytes(dialog.FileName));
                WriteableBitmap output_bi = new WriteableBitmap(BitmapFrame.Create(data));
                data.Close();
                CardImage.Source = output_bi;
            }

        }

        private void Check_beta(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                betaMessage.Visibility = Visibility.Visible;
            }
            else
            {
                betaMessage.Visibility = Visibility.Hidden;
            }

        }

        static string _cardColor = "";
        private void RadioButton_MainRate(object sender, RoutedEventArgs e)
        {
            _cardColor = ((RadioButton)sender).Content.ToString();
            ViewNamePlate();

        }
        private void ViewNamePlate()
        {

            bool suteraMode = false;

            if (suteraBox == null || suteraBox.IsChecked == true) suteraMode = true;

            switch (_cardColor)
            {
                case "黒":
                    if (suteraMode == true) CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Main_Black.png", UriKind.Absolute));
                    else CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Only_Black.png", UriKind.Absolute));
                    break;
                case "青":
                    if (suteraMode == true) CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Main_Blue.png", UriKind.Absolute));
                    else CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Only_Blue.png", UriKind.Absolute));
                    break;
                case "緑":
                    if (suteraMode == true) CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Main_Green.png", UriKind.Absolute));
                    else CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Only_Green.png", UriKind.Absolute));
                    break;
                case "赤":
                    if (suteraMode == true) CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Main_Red.png", UriKind.Absolute));
                    else CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Only_Red.png", UriKind.Absolute));
                    break;
                case "白":
                    if (suteraMode == true) CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Main_White.png", UriKind.Absolute));
                    else CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Only_White.png", UriKind.Absolute));
                    break;
                case "黄":
                    if (suteraMode == true) CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Main_Yellow.png", UriKind.Absolute));
                    else CardTitleImage.Source = new BitmapImage(new Uri(path + "Resources/Only_Yellow.png", UriKind.Absolute));
                    break;
            }
        }



        private void RadioButton_CardKind(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Content.ToString())
            {
                case "ステラ":
                    CardPowerImage.Visibility = Visibility.Visible;
                    CardPowerText.Visibility = Visibility.Visible;
                    CardRace1Text.Visibility = Visibility.Visible;
                    CardRace2Text.Visibility = Visibility.Visible;
                    Canvas.SetBottom(effectCanvas, 55);
                    ViewNamePlate();
                    CardBreakUp.Visibility = Visibility.Visible;
                    CardBreakDown.Visibility = Visibility.Visible;
                    break;
                case "スペル":
                    CardPowerImage.Visibility = Visibility.Hidden;
                    CardPowerText.Visibility = Visibility.Hidden;
                    CardRace1Text.Visibility = Visibility.Hidden;
                    CardRace2Text.Visibility = Visibility.Hidden;
                    Canvas.SetBottom(effectCanvas, 20);
                    ViewNamePlate();
                    CardBreakUp.Visibility = Visibility.Hidden;
                    CardBreakDown.Visibility = Visibility.Hidden;
                    break;
            }

        }


        private void UpdateHeight(object sender, RoutedEventArgs e)
        {
            this.Height += 5;

        }

        private void RadioButton_SubRate(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Content.ToString())
            {
                case "黒":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_Black.png", UriKind.Absolute));
                    break;
                case "青":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_Blue.png", UriKind.Absolute));
                    break;
                case "緑":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_Green.png", UriKind.Absolute));
                    break;
                case "赤":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_Red.png", UriKind.Absolute));
                    break;
                case "白":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_White.png", UriKind.Absolute));
                    break;
                case "黄":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_Yellow.png", UriKind.Absolute));
                    break;
                case "無":
                    CardSubImage.Source = new BitmapImage(new Uri(path + "Resources/Sub_less.png", UriKind.Absolute));
                    break;
            }

        }

        private void CheckBox_CardType(object sender, RoutedEventArgs e)
        {
            var iconList = new List<Image>();
            iconList.Add(CardTypeImage1);
            iconList.Add(CardTypeImage2);
            iconList.Add(CardTypeImage3);
            iconList.Add(CardTypeImage4);

            foreach (Image item in iconList)
            {
                item.Visibility = Visibility.Hidden;
            }
            var place = 0;

            if (burst.IsChecked == true)
            {
                iconList[place].Source = new BitmapImage(new Uri(path + "Resources/burst.png", UriKind.Absolute));
                iconList[place].Visibility = Visibility.Visible;
                place += 1;
            }
            if (reAction.IsChecked == true)
            {
                iconList[place].Source = new BitmapImage(new Uri(path + "Resources/re-action.png", UriKind.Absolute));
                iconList[place].Visibility = Visibility.Visible;
                place += 1;
            }
            if (sealdtrigger.IsChecked == true)
            {
                iconList[place].Source = new BitmapImage(new Uri(path + "Resources/sealedtrigger.png", UriKind.Absolute));
                iconList[place].Visibility = Visibility.Visible;
                place += 1;
            }
            if (spellstep.IsChecked == true)
            {
                iconList[place].Source = new BitmapImage(new Uri(path + "Resources/spellstep.png", UriKind.Absolute));
                iconList[place].Visibility = Visibility.Visible;
            }

        }

        // 要素を画像として保存する
        private void SaveImage(FrameworkElement target, string path)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("pathが未設定");

            // レンダリング
            // ディスプレイDPIのn倍の解像度で保存する
            var n = 10;
            var dpi = 96;



            PresentationSource source = PresentationSource.FromVisual(this);
            var bmp = new RenderTargetBitmap(
                (int)(target.ActualWidth * n),
                (int)(target.ActualHeight * n),
                dpi * n * source.CompositionTarget.TransformToDevice.M11, dpi * n * source.CompositionTarget.TransformToDevice.M22, // DPI
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

        private int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }

        private void EffectChanged(object sender, TextChangedEventArgs e)
        {
            double fontSize = 0, fontFlavorSize = 0;
            if (CardEffectTextBox == null) return;
            if (CardEffectFontSizeBox == null || CardFlavorEffectFontSizeBox == null || double.TryParse(CardEffectFontSizeBox.Text, out fontSize) != true || double.TryParse(CardFlavorEffectFontSizeBox.Text, out fontFlavorSize) != true) return;

            //テキストを取得する
            var text = CardEffectTextBox.Text;


            //取得したテキストから「---」の前後に切り分ける
            var placeDelimiter = text.IndexOf("\r\n---\r\n");
            var effectText = "";
            var flavorText = "";
            if (placeDelimiter >= 0)
            {
                effectText = text.Substring(0, text.IndexOf("\r\n---\r\n"));
                flavorText = text.Substring(placeDelimiter + 7, text.Length - placeDelimiter - 7);
            }
            else
            {
                effectText = text;
            }
            //---の前を通常のフォント、---以降をフレーバーテキストフォントにする




            var flavorRuns = new Run();
            flavorRuns.Text = flavorText;
            flavorRuns.FontFamily = new FontFamily("HGS 明朝B");
            flavorRuns.FontSize = fontFlavorSize;



            var beforeTextSize = CardEffectText.ActualHeight;
            //テキストを結合して描画する
            CardEffectText.Inlines.Clear();

            if (CountChar(effectText, '@') % 2 == 0)//@が偶数個含まれる場合
            {
                CardTextIcon.Visibility = Visibility.Hidden;
                if (effectText.IndexOf("@") == 0)
                {//文頭がいきなりアイコンの場合だけ例外処理を行う。
                    effectText = effectText.Substring(1, effectText.Length - 1);
                    var atPlace = effectText.IndexOf("@");
                    var str = "";
                    str = effectText.Substring(0, atPlace);
                    effectText = effectText.Substring(atPlace + 1, effectText.Length - atPlace - 1);
                    if (File.Exists(path + "Icon/" + str + ".png") == true)
                    {
                        CardTextIcon.Source = new BitmapImage(new Uri(path + "Icon/" + str + ".png", UriKind.Absolute));
                        Canvas.SetBottom(CardTextIcon, beforeTextSize - 6 - fontSize / 4);
                    }
                    CardTextIcon.Visibility = Visibility.Visible;

                }
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
                        if (fontSize >= 2) effectRuns.FontSize = fontSize;
                        CardEffectText.Inlines.Add(effectRuns);
                    }
                    else
                    {//アイコンに関する処理

                        if (File.Exists(path + "Icon/" + str + ".png") == true)
                        {
                            Image image = new Image();
                            image.Width = fontSize;
                            image.Height = fontSize;
                            image.Stretch = Stretch.Uniform;
                            image.Source = new BitmapImage(new Uri(path + "Icon/" + str + ".png", UriKind.Absolute));
                            InlineUIContainer icon = new InlineUIContainer();
                            icon.Child = image;
                            CardEffectText.Inlines.Add(icon);
                        }

                    }


                    loopMode = (loopMode + 1) % 2;

                }

            }
            else
            {//@が奇数個含まれる場合
                var effectRuns = new Run();
                effectRuns.FontFamily = new FontFamily("HGS ゴシックM");
                if (fontSize >= 2) effectRuns.FontSize = fontSize;

                flavorRuns.Text = effectText;
                CardEffectText.Inlines.Add(effectRuns);
            }

            var gapRuns = new Run();
       
            gapRuns.FontFamily = new FontFamily("HGS ゴシックM");
            gapRuns.FontSize = 3;
            gapRuns.Text = "\n    あ   \n";
            gapRuns.Foreground = new SolidColorBrush(Colors.Transparent);
            CardEffectText.Inlines.Add(gapRuns);
            CardEffectText.Inlines.Add(flavorRuns);

        }
        private delegate void Test();

        private void DoEvents()
        {

            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(ExitFrames);
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }


        private object ExitFrames(object obj)
        {
            ((DispatcherFrame)obj).Continue = false;
            return null;
        }

        private void BreakView(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var breakNum = ((Slider)sender).Value;
            var cardBreakList = new List<Image>();
            cardBreakList.Add(CardBreakImage1);
            cardBreakList.Add(CardBreakImage2);
            cardBreakList.Add(CardBreakImage3);
            cardBreakList.Add(CardBreakImage4);
            cardBreakList.Add(CardBreakImage5);
            cardBreakList.Add(CardBreakImage6);
            cardBreakList.Add(CardBreakImage7);
            cardBreakList.Add(CardBreakImage8);
            cardBreakList.Add(CardBreakImage9);

            for (int i = 0; i < cardBreakList.Count; i++)
            {
                (cardBreakList[i]).Visibility = Visibility.Hidden;
            }

            for (int i = 0; i < breakNum; i++)
            {
                (cardBreakList[i]).Visibility = Visibility.Visible;
            }
        }

        private void CardEffectHeight(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CardEffectImageCenter.Height = ((Slider)sender).Value;
            Canvas.SetBottom(CardEffectImageUp, ((Slider)sender).Value + 12);

        }
    }
}
