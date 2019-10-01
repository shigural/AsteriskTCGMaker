namespace AsteriskTCGMaker2.ViewModels
{
    /// <summary>
    /// MainView ウィンドウに対するデータコンテキストを表します。
    /// </summary>
    internal class MainViewModel : NotificationObject
    {
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

        private string _cardNameText = "カード名";
        public string CardNameText
        {
            get { return this._cardNameText; }
            set { SetProperty(ref this._cardNameText, value); }
        }

        private string _cardRubyText = "";
        public string CardRubyText
        {
            get { return this._cardRubyText; }
            set { SetProperty(ref this._cardRubyText, value); }
        }

        private string _cardRace1 = "-";
        public string CardRace1
        {
            get { return this._cardRace1; }
            set { SetProperty(ref this._cardRace1, value); }
        }

        private string _cardRace2 = "-";
        public string CardRace2
        {
            get { return this._cardRace2; }
            set { SetProperty(ref this._cardRace2, value); }
        }

        private double _cardNameFontSize = 10;
        public double CardNameFontSize
        {
            get { return this._cardNameFontSize; }
            set { if (value >= 100) return; SetProperty(ref this._cardNameFontSize, value); }
        }

        private double _cardRubyFontSize = 4;
        public double CardRubyFontSize
        {
            get { return this._cardRubyFontSize; }
            set { if (value >= 100) return; SetProperty(ref this._cardRubyFontSize, value); }
        }

        private double _cardEffectFontSize = 8;
        public double CardEffectFontSize
        {
            get { return this._cardEffectFontSize; }
            set { if (value >= 100) return; SetProperty(ref this._cardEffectFontSize, value); }
        }


        private int _cardMainMana = 1;
        public int CardMainMana
        {
            get { return this._cardMainMana; }
            set { if (value >= 100) return; SetProperty(ref this._cardMainMana, value); }
        }

        private int _cardSubMana = 1;
        public int CardSubMana
        {
            get { return this._cardSubMana; }
            set { if (value >= 100) return; SetProperty(ref this._cardSubMana, value); }
        }

        
        private int _cardEffectHeight = 45;
        public int CardEffectHeight
        {
            get { return this._cardEffectHeight; }
            set {
                SetProperty(ref this._cardEffectHeight, value); }
        }

        private string _cardPower ="1000";
        public string CardPower
        {
            get { return this._cardPower; }
            set { SetProperty(ref this._cardPower, value); }
        }



        private string _cardIllustration = "Illustration:翡翠 蒼輝";
        public string CardIllustration
        {
            get { return this._cardIllustration; }
            set { SetProperty(ref this._cardIllustration, value); }
        }

        private string _cardRubyColor = "White";
        public string CardRubyColor
        {
            get { return this._cardRubyColor; }
            set { SetProperty(ref this._cardRubyColor, value); }
        }

        private string _cardNameColor = "White";
        public string CardNameColor
        {
            get { return this._cardNameColor; }
            set { SetProperty(ref this._cardNameColor, value); }
        }

        private string _cardEffectColor = "Black";
        public string CardEffectColor
        {
            get { return this._cardEffectColor; }
            set { SetProperty(ref this._cardEffectColor, value); }

        }

        private string _cardManaColor = "White";
        public string CardManaColor
        {
            get { return this._cardManaColor; }
            set { SetProperty(ref this._cardManaColor, value); }
        }

        private string _cardSubManaColor = "White";
        public string CardSubManaColor
        {
            get { return this._cardSubManaColor; }
            set { SetProperty(ref this._cardSubManaColor, value); }
        }

        private string _cardEffectText = "カード効果";
        public string CardEffectText
        {
            get { return this._cardEffectText; }
            set { SetProperty(ref this._cardEffectText, value); }
        }


        private int _breakCount = 0;
        public int BreakCount
        {
            get { return this._breakCount; }
            set
            {
                SetProperty(ref this._breakCount, value);
            }
        }


    }
}