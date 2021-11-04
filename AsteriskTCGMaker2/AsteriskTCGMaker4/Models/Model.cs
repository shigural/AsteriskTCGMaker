using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace AsteriskTCGMaker4.Models
{
    internal class Model : NotificationObject
    {
    }

    /// <summary>
    /// シングルトン
    /// </summary>
    public sealed class Singleton
    {
        private static Singleton instance = new Singleton();
        private static string _path;

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }

        private Singleton()
        {
        }

        /// <summary>
        /// 起動しているパス
        /// </summary>
        public string Path
        {
            get
            {
                if (_path == null) _path = System.AppDomain.CurrentDomain.BaseDirectory;
                return _path;
            }
        }

        /// <summary>
        /// カードのプロジェクトファイルが保存されているパス
        /// </summary>
        public string CardPath
        {
            get
            {
                return Path + "CardData\\";
            }
        }

    }
}
