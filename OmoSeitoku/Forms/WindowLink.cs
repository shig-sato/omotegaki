using System;
using System.Text;

namespace OmoSeitoku.Forms
{
    public sealed class WindowLink
    {
        #region Const

        private const string WM_USER = "WM_USER";
        private const string WM_USER_WNDLINK = WM_USER + "_WNDLINK";

        private const int WNDLINK_OK = 1;
        private const int WNDLINK_ERROR = 2;

        private const int WNDLINK_WPARAM_POLLING = 100;
        private const int WNDLINK_POLLING_IGNORE = 0;
        private const int WNDLINK_POLLING_READY = 1;
        private const int WNDLINK_POLLING_UNREADY = 2;

        private const int WNDLINK_WPARAM_NEWWINDOW = 150;

        private const int WNDLINK_WPARAM_LINK = 200;
        private const int WNDLINK_LINK_DECLINE = 0;
        private const int WNDLINK_LINK_ACCEPT = 1;

        private const int WNDLINK_WPARAM_UNLINK = 201;

        private const int WNDLINK_WPARAM_SENDVALUE = 300;
        private const int WNDLINK_WPARAM_SENDCHAR = 310;
        private const int WNDLINK_WPARAM_SENDTEXT = 320;
        private const int WNDLINK_WPARAM_POSTVALUE = 400;
        private const int WNDLINK_WPARAM_CLOSEAPP = 500;
        private const int WNDLINK_WPARAM_TITLE_CHANGED = 1000;

        #endregion


        // Field

        private static readonly TimeSpan ReadyMessageReturnSpan = TimeSpan.FromSeconds(30);

        private object _lockObject = new object();
        private IWindow _window;
        /// <summary>
        /// ウィンドウリンクのメッセージID。
        /// リンク待機開始時に設定される。
        /// リンクが切断された際に 0 で初期化される。
        /// </summary>
        private int _WndLinkMsgId;
        /// <summary>リンク中に使用するメッセージID</summary>
        private int _linkingMsgId;
        // SendText用バッファ
        private byte[] _WndLinkSendTextBuf;
        private int _WndLinkSendTextBufPosition;
        private DateTime _lastReturnReadyMessageDateTime;


        // Property

        public IntPtr LinkedWindowHandle
        {
            get { return __property_LinkedWindowHandle; }
            private set
            {
                if (__property_LinkedWindowHandle != value)
                {
                    __property_LinkedWindowHandle = value;
                    this.OnLinkedWindowChanged();
                }
            }
        }
        public bool IsLinked
        {
            get { return (LinkedWindowHandle != IntPtr.Zero); }
        }

        #region (Property Field)

        private IntPtr __property_LinkedWindowHandle;

        #endregion


        // Constructor

        public WindowLink(IWindow window)
        {
            _window = window;
        }


        // Method

        /// <summary>
        /// リンクウィンドウの WndProc 処理時に呼ぶ。
        /// </summary>
        /// <param name="m"></param>
        /// <returns>補足した場合は true</returns>
        public bool WndProc(ref System.Windows.Forms.Message m)
        {
            if (_WndLinkMsgId != 0)
            {
                if (m.Msg == _WndLinkMsgId)
                    return OnCommonMessageReceived(ref m);

                if (m.Msg == _linkingMsgId)
                    return OnLinkingMessageReceived(ref m);
            }
            return false;
        }
        /// <summary>
        /// 相手からのリンク開始メッセージを待つ。
        /// 既にリンク中の場合は待機前に切断する。
        /// </summary>
        public void WaitLink()
        {
            if (IsLinked) UnLink();
            _WndLinkMsgId = Win32.User.RegisterWindowMessage(WM_USER_WNDLINK);
        }
        /// <summary>
        /// リンクを切断
        /// </summary>
        public void UnLink()
        {
            if (IsLinked)
            {
                // リンク相手に切断メッセージを送信
                Win32.User.PostMessage(
                    LinkedWindowHandle, _WndLinkMsgId,
                    WNDLINK_WPARAM_UNLINK, _window.Handle.ToInt32());

                // 自身に切断メッセージを送信
                Win32.User.SendMessage(
                    _window.Handle, _WndLinkMsgId,
                    WNDLINK_WPARAM_UNLINK, LinkedWindowHandle);
            }
        }
        /// <summary>
        /// リンク先に "タイトル変更時メッセージ" を送信する。
        /// </summary>
        public void SendTitleChanged()
        {
            if (this.IsLinked)
            {
                Win32.User.PostMessage(
                    LinkedWindowHandle, _linkingMsgId,
                    WNDLINK_WPARAM_TITLE_CHANGED, 0);
            }
        }

        private void OnLink(IntPtr targetHandle)
        {
            this.LinkedWindowHandle = targetHandle;
        }
        private void OnUnLink()
        {
            this.LinkedWindowHandle = IntPtr.Zero;
            _lastReturnReadyMessageDateTime = DateTime.MinValue;
        }
        /// <summary>
        /// リンク先の変更時
        /// </summary>
        private void OnLinkedWindowChanged()
        {
            _linkingMsgId = (this.IsLinked)
                          ? Win32.User.RegisterWindowMessage(WM_USER + '_' + LinkedWindowHandle)
                          : 0;
            _window.OnLinkedWindowChanged();
        }
        /// <summary>
        /// リンク先からの "テキストメッセージ" 受信時
        /// </summary>
        private void OnWindowLinkText(byte[] text)
        {
            //string str = Encoding.GetEncoding("Shift_JIS").GetString(text);

            _window.OnTextReceived(text);
        }
        private bool OnCommonMessageReceived(ref System.Windows.Forms.Message m)
        {
            switch (m.WParam.ToInt32())
            {
                case WNDLINK_WPARAM_POLLING:
                    #region OnPollingMessageReceived
                    {
                        if (!IsLinked)
                        {
                            IntPtr callerHWnd = m.LParam;

                            bool ready;
                            lock (_lockObject)
                            {
                                ready = (DateTime.Now - _lastReturnReadyMessageDateTime > ReadyMessageReturnSpan) &&
                                        _window.OnPollingMessageReceived(callerHWnd, GetWindowText(callerHWnd));

                                if (ready)
                                {
                                    _lastReturnReadyMessageDateTime = DateTime.Now;
                                }
                            }

                            m.Result = new IntPtr(ready
                                                   ? WNDLINK_POLLING_READY
                                                   : WNDLINK_POLLING_IGNORE);
                        }
                        else
                        {
                            m.Result = new IntPtr(WNDLINK_POLLING_UNREADY);
                        }
                    }
                    #endregion
                    return true;

                case WNDLINK_WPARAM_NEWWINDOW:
                    #region OnNewWindowMessageReceived
                    {
                        m.Result = new IntPtr(_window.OnNewWindowMessageReceived() ? WNDLINK_OK : WNDLINK_ERROR);
                    }
                    #endregion
                    return true;

                case WNDLINK_WPARAM_LINK:
                    #region OnLinkMessageReceived
                    {
                        IntPtr callerHWnd = m.LParam;

                        bool accept = !IsLinked &&
                                      _window.OnLinkMessageReceived(callerHWnd, GetWindowText(callerHWnd));

                        if (accept)
                        {
                            this.OnLink(callerHWnd);
                            m.Result = new IntPtr(WNDLINK_LINK_ACCEPT);
                        }
                        else
                        {
                            m.Result = new IntPtr(WNDLINK_LINK_DECLINE);
                        }
                    }
                    #endregion
                    return true;

                case WNDLINK_WPARAM_UNLINK:
                    #region OnUnLinkMessageReceived
                    {
                        IntPtr callerHWnd = m.LParam;
                        if (this.LinkedWindowHandle == callerHWnd)
                        {
                            this.OnUnLink();
                            _WndLinkMsgId = 0;
                            this.WaitLink();
                        }
                    }
                    #endregion
                    return true;
            }
            return false;
        }
        private bool OnLinkingMessageReceived(ref System.Windows.Forms.Message m)
        {
            switch (m.WParam.ToInt32())
            {
                case WNDLINK_WPARAM_SENDCHAR:
                    #region OnSendCharMessageReceived
                    {
                        byte[] buf = _WndLinkSendTextBuf;

                        if (buf != null)
                        {
                            int pos = _WndLinkSendTextBufPosition;

                            if (0 <= pos && pos < buf.Length)
                            {
                                buf[pos++] = (byte)m.LParam.ToInt32();

                                if (pos < buf.Length)
                                {
                                    _WndLinkSendTextBufPosition = pos;
                                }
                                else
                                {
                                    _WndLinkSendTextBufPosition = 0;
                                    _WndLinkSendTextBuf = null;

                                    OnWindowLinkText(buf);
                                }
                                m.Result = new IntPtr(WNDLINK_OK);
                            }
                            else
                            {
                                _WndLinkSendTextBufPosition = 0;
                                _WndLinkSendTextBuf = null;

                                m.Result = new IntPtr(WNDLINK_ERROR);
                            }
                        }
                        else
                        {
                            m.Result = new IntPtr(WNDLINK_ERROR);
                        }
                    }
                    #endregion
                    return true;

                case WNDLINK_WPARAM_SENDTEXT:
                    #region OnSendTextMessageReceived
                    {
                        int len = m.LParam.ToInt32();
                        _WndLinkSendTextBuf = new byte[len];
                        _WndLinkSendTextBufPosition = 0;

                        m.Result = new IntPtr(WNDLINK_OK);
                    }
                    #endregion
                    return true;

                case WNDLINK_WPARAM_SENDVALUE:
                    #region OnSendValueMessageReceived
                    {
                        m.Result = _window.OnValueReceived(m.LParam);
                    }
                    #endregion
                    return true;

                case WNDLINK_WPARAM_CLOSEAPP:
                    _window.OnCloseAppReceived();
                    return true;
            }
            return false;
        }


        // Static Method

        private static string GetWindowText(IntPtr hWnd)
        {
            var sb = new StringBuilder(255);
            Win32.User.GetWindowText(hWnd, sb, 255);
            return sb.ToString();
        }



        // Interface

        public interface IWindow
        {
            IntPtr Handle { get; }

            /// <summary>
            /// ポーリングメッセージ受信時
            /// </summary>
            /// <param name="callerHandle">呼び出し元ウィンドウのハンドル</param>
            /// <param name="callerTitle">呼び出し元ウィンドウのタイトル</param>
            /// <returns>リンクできる場合は真</returns>
            bool OnPollingMessageReceived(IntPtr callerHandle, string callerTitle);
            /// <summary>
            /// 新規ウィンドウリクエストメッセージ受信時
            /// </summary>
            /// <returns>新しいウィンドウが表示できた場合は真</returns>
            bool OnNewWindowMessageReceived();
            /// <summary>
            /// リンクメッセージ受信時
            /// </summary>
            /// <param name="callerHandle">呼び出し元ウィンドウのハンドル</param>
            /// <param name="callerTitle">呼び出し元ウィンドウのタイトル</param>
            /// <returns>リンクを了承する場合は真</returns>
            bool OnLinkMessageReceived(IntPtr callerHandle, string callerTitle);
            void OnLinkedWindowChanged();
            void OnTextReceived(byte[] text);
            IntPtr OnValueReceived(IntPtr value);
            void OnCloseAppReceived();
        }
    }
}
