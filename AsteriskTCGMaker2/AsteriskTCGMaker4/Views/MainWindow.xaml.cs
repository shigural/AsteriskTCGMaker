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
