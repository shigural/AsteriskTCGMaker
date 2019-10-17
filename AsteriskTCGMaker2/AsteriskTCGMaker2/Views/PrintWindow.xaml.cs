using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Threading;
using AsteriskTCGMaker2.ViewModels;
using AsteriskTCGMaker2.Views;


namespace AsteriskTCGMaker2.Views
{
    /// <summary>
    /// PrintWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PrintWindow : Window
    {
        private static string path;
        public PrintWindow()
        {

            InitializeComponent();
            InitializeContents();
            Height = 600;
            Width = 433;
        }
        private void InitializeContents()
        {
            path = System.AppDomain.CurrentDomain.BaseDirectory;

            MemoryStream data = new MemoryStream(File.ReadAllBytes(path + "Result/Output.png"));
            WriteableBitmap output_bi = new WriteableBitmap(BitmapFrame.Create(data));
            data.Close();
            card0.Source = output_bi;
            card1.Source = output_bi;
            card2.Source = output_bi;
            card3.Source = output_bi;
            card4.Source = output_bi;
            card5.Source = output_bi;
            card6.Source = output_bi;
            card7.Source = output_bi;
            card8.Source = output_bi;
            PrintFlame.Source = new BitmapImage(new Uri(path + "Resources/PrintFlame.png", UriKind.Absolute));
            OutsideFlame.Source = new BitmapImage(new Uri(path + "Resources/OutsideFlame.png", UriKind.Absolute));
            backPackage.Source = new BitmapImage(new Uri(path + "Resources/backPackage.png", UriKind.Absolute));
        }

        //画面を更新する
        private void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }

        private void Button_BackPrint(object sender, RoutedEventArgs e)
        {
            backPackage.Visibility = Visibility.Visible;
           // DoEvents();
            Button_Print(sender, e);
            backPackage.Visibility = Visibility.Hidden;
            //DoEvents();
        }



        private void Button_Print(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveImage(this.InputGrid, path + "Result/printData.png");
            }
            catch
            {
                MessageBox.Show("ファイル保存に失敗しました。\n他のアプリケーションで画像ファイルを開いていないか確認してください。", "画像出力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                // 印刷ダイアログを作成。
                var dPrt = new PrintDialog();

                // 印刷ダイアログを表示して、プリンタ選択と印刷設定を行う。
                if (dPrt.ShowDialog() == true)
                {
                    // ここから印刷を実行する。

                    // 印刷可能領域を取得する。
                    var area = dPrt.PrintQueue.GetPrintCapabilities().PageImageableArea;

                    // 上と左の余白を含めた印刷可能領域の大きさのCanvasを作る。
                    var canv = new Canvas();


                    // ここでCanvasに描画する。
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    Canvas.SetTop(image, 7.5);
                    Canvas.SetLeft(image, 7.5);
                    MemoryStream data = new MemoryStream(File.ReadAllBytes(path + "Result/printData.png"));
                    WriteableBitmap output_bi = new WriteableBitmap(BitmapFrame.Create(data));
                    data.Close();
                    image.Source = output_bi;
                    image.Width = area.ExtentWidth + area.OriginWidth * 2 - 15;
                    image.Height = area.ExtentHeight + area.OriginHeight * 2 - 15;

                    canv.Children.Add(image);



                    // FixedPageを作って印刷対象（ここではCanvas）を設定する。
                    var page = new FixedPage();
                    page.Width = image.Width + 1000;
                    page.Height = image.Height + 200;
                    page.Children.Add(canv);

                    // PageContentを作ってFixedPageを設定する。
                    var cont = new PageContent();
                    cont.Child = page;

                    // FixedDocumentを作ってPageContentを設定する。
                    var doc = new FixedDocument();
                    doc.Pages.Add(cont);

                    // 印刷する。
                    dPrt.PrintDocument(doc.DocumentPaginator, "Print1");

                }
            }
            catch
            {
                MessageBox.Show("不明なエラーにより印刷に失敗しました。\nプリンタの接続状況や印刷状況を確認してください。", "印刷エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_PDF(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveImage(this.InputGrid, path + "Result/printData.png");
            }
            catch
            {
                MessageBox.Show("ファイル保存に失敗しました。\n他のアプリケーションで画像ファイルを開いていないか確認してください。", "画像出力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var doc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
                var stream = new MemoryStream();

                var pw = PdfWriter.GetInstance(doc, stream);

                doc.Open();
                var pdfContentByte = pw.DirectContent;
                var bf = BaseFont.CreateFont(@"c:\windows\fonts\msgothic.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                // A4 595x842 pt = 210x297  

                iTextSharp.text.Image images = iTextSharp.text.Image.GetInstance(new Uri(path + "Result/printData.png", UriKind.Absolute));

                images.ScalePercent(100 * 842 / images.Height);
                images.SetAbsolutePosition(0, 0);
                
                doc.Add(images);
                doc.Close();

                
                System.IO.File.Delete(@"result.pdf");
                using (BinaryWriter w = new BinaryWriter(File.OpenWrite(@"result.pdf")))
                {
                    w.Write(stream.ToArray());
                }
                MessageBox.Show("PDF出力が完了しました。", "出力完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("ファイル保存に失敗しました。\n他のアプリケーションでpdfファイルを開いていないか確認してください。", "pdf出力エラー", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }


        private void SaveImage(FrameworkElement target, string path)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("pathが未設定");

            // レンダリング
            // ディスプレイDPIのn倍の解像度で保存する
            var n = 10;
            if (downView.IsChecked == true) n = 5;//低解像度とする場合
            
            PresentationSource source = PresentationSource.FromVisual(target);
            var bmp = new RenderTargetBitmap(
                (int)(target.ActualWidth * n),
                (int)(target.ActualHeight * n),
                96 * n * source.CompositionTarget.TransformToDevice.M11, 96 * n * source.CompositionTarget.TransformToDevice.M22, // DPI
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
    }
}
