using AsteriskTCGMaker4.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace AsteriskTCGMaker4.Views
{
    /* 
     * If some events were receive from ViewModel, then please use PropertyChangedWeakEventListener and CollectionChangedWeakEventListener.
     * If you want to subscribe custome events, then you can use LivetWeakEventListener.
     * When window closing and any timing, Dispose method of LivetCompositeDisposable is useful to release subscribing events.
     *
     * Those events are managed using WeakEventListener, so it is not occurred memory leak, but you should release explicitly.
     */
    public partial class MainWindow : Window
    {
        private string path;
        public MainWindow()
        {
            InitializeComponent();
            path = System.AppDomain.CurrentDomain.BaseDirectory;
            //CardFlameST.Source = new BitmapImage(new Uri(path + "Resources/3/CardFlame_Stera_Black.png", UriKind.Absolute));
            //CardFlameSP.Source = new BitmapImage(new Uri(path + "Resources/3/CardFlame_Spell_Black.png", UriKind.Absolute));
            //CardImage.Source = new BitmapImage(new Uri(path + "Resources/2/InputSample.png", UriKind.Absolute));
            CardEffectImageDown.Source = new BitmapImage(new Uri(path + "Resources/2/TextBoxDown.png", UriKind.Absolute));
            CardEffectImageUp.Source = new BitmapImage(new Uri(path + "Resources/2/TextBoxUp.png", UriKind.Absolute));
            CardEffectImageCenter.Source = new BitmapImage(new Uri(path + "Resources/2/TextBoxCenter.png", UriKind.Absolute));
            CardPowerImage.Source = new BitmapImage(new Uri(path + "Resources/2/PowerFlame.png", UriKind.Absolute));
            CardKind1.Source = new BitmapImage(new Uri(path + "Resources/2/KindFrame.png", UriKind.Absolute));
            CardKind2.Source = new BitmapImage(new Uri(path + "Resources/2/KindFrame.png", UriKind.Absolute));
            CardBreakImage1.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage2.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage3.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage4.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage5.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage6.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage7.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage8.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardBreakImage9.Source = new BitmapImage(new Uri(path + "Resources/2/Break.png", UriKind.Absolute));
            CardTypeImage1.Source = new BitmapImage(new Uri(path + "Resources/2/Burst.png", UriKind.Absolute));
            CardTypeImage2.Source = new BitmapImage(new Uri(path + "Resources/2/re-action.png", UriKind.Absolute));
            CardTypeImage3.Source = new BitmapImage(new Uri(path + "Resources/2/sealedtrigger.png", UriKind.Absolute));
            CardTypeImage4.Source = new BitmapImage(new Uri(path + "Resources/2/spellstep.png", UriKind.Absolute));
            CardTypeImage5.Source = new BitmapImage(new Uri(path + "Resources/2/Ke.png", UriKind.Absolute));
        }

        //指定要素を画像として保存する
        public void SaveImage(FrameworkElement target, string path)
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

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            doEvent();

            var inputPath = Singleton.Instance.Path + "Result/Input.png";
            var outputName = this.CardNameTextBox.Text;

            //ファイル名に使えない文字を削除
            outputName = outputName.Replace(":", "");
            outputName = outputName.Replace("：", "");


            var outputPath1 = System.IO.Path.Combine(Singleton.Instance.Path, "Result/" + outputName + ".png");
            var outputPath2 = System.IO.Path.Combine(Singleton.Instance.Path, "Result/Output.png");
            if (File.Exists(outputPath1)) System.IO.File.Delete(outputPath1);

            try
            {

                SaveImage(this.OutPutCanvas, outputPath1);
                SaveImage(this.OutPutCanvas, outputPath2);

                MessageBox.Show("画像出力が完了しました。", "出力完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("ファイル保存に失敗しました。\n他のアプリケーションで画像ファイルを開いていないか確認してください。", "画像出力エラー", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


    }
}
