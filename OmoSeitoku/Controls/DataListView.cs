using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace OmoSeitoku.Controls
{
    /*
     * Data bound ListView control
     * http://www.codeproject.com/KB/miscctrl/Data_bound_ListView.aspx
     * 
     * By Alex Cherkasov | 25 Nov 2003 
     * 
     * License:
     *     The Code Project Open License (CPOL)
     *     http://www.codeproject.com/info/cpol10.aspx
     */


    public sealed class DataColumnHeader : ColumnHeader
    {
        private string mField;


        public string Field
        {
            get
            {
                return mField;
            }

            set
            {
                mField = value;
            }
        }
    }

    // [DefaultMemberAttribute("Item")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:オブジェクトの初期化を簡略化します", Justification = "_")]
    public sealed class DataColumnHeaderCollection : CollectionBase
    {
        public delegate void InvalidateEventHandler();

        private InvalidateEventHandler InvalidateEvent;


        public DataColumnHeader this[int Index]
        {
            get
            {
                return (DataColumnHeader)base.List[Index];
            }
        }

        public event InvalidateEventHandler Invalidate
        {
            add
            {
                InvalidateEvent = (InvalidateEventHandler)Delegate.Combine(InvalidateEvent, value);
            }

            remove
            {
                InvalidateEvent = (InvalidateEventHandler)Delegate.Remove(InvalidateEvent, value);
            }
        }

        private void OnInvalidate()
        {
            InvalidateEvent?.Invoke();
        }

        public void Add(string Field)
        {
            DataColumnHeader dataColumnHeader = new DataColumnHeader();
            dataColumnHeader.Text = Field;
            dataColumnHeader.Field = Field;
            base.List.Add(dataColumnHeader);
        }

        public void Add(string Field, int Width)
        {
            DataColumnHeader dataColumnHeader = new DataColumnHeader();
            dataColumnHeader.Text = Field;
            dataColumnHeader.Field = Field;
            dataColumnHeader.Width = Width;
            base.List.Add(dataColumnHeader);
        }

        public void Add(string Text, string Field)
        {
            DataColumnHeader dataColumnHeader = new DataColumnHeader();
            dataColumnHeader.Text = Text;
            dataColumnHeader.Field = Field;
            base.List.Add(dataColumnHeader);
        }

        public void Add(string Text, string Field, int Width)
        {
            DataColumnHeader dataColumnHeader = new DataColumnHeader();
            dataColumnHeader.Text = Text;
            dataColumnHeader.Field = Field;
            dataColumnHeader.Width = Width;
            base.List.Add(dataColumnHeader);
        }

        public void Add(DataColumnHeader Item)
        {
            base.List.Add(Item);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            OnInvalidate();
        }

        protected override void OnInsertComplete(int index, object value)
        {
            OnInvalidate();
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            OnInvalidate();
        }

        protected override void OnClearComplete()
        {
            OnInvalidate();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:読み取られていないプライベート メンバーを削除", Justification = "_")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:オブジェクトの初期化を簡略化します", Justification = "_")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0038:パターン マッチングを使用します", Justification = "_")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "_")]
    public sealed class DataListView : ListView
    {
        [AccessedThroughPropertyAttribute("mBindingList")]
        private IBindingList _mBindingList;

        [AccessedThroughPropertyAttribute("mColumns")]
        private DataColumnHeaderCollection _mColumns;

        private object mDataSource;

        private string mDataMember;

        private bool mAutoDiscover;


        [RefreshPropertiesAttribute(RefreshProperties.Repaint)]
        [CategoryAttribute("Data")]
        [TypeConverterAttribute("System.Windows.Forms.Design.DataSourceConverter,System.Design")]
        public object DataSource
        {
            get
            {
                return mDataSource;
            }

            set
            {
                mDataSource = RuntimeHelpers.GetObjectValue(value);
                base.BeginUpdate();
                try
                {
                    SetSource();
                    if (mAutoDiscover)
                    {
                        DoAutoDiscover();
                    }
                    DataBind();
                }
                finally
                {
                    base.EndUpdate();
                }
            }
        }

        [CategoryAttribute("Data")]
        //ALEX	[EditorAttribute("System.Windows.Forms.Design.DataMemberListEditor,System.Design", typeof(System.Drawing.Design.UITypeEditor,System.Drawing,WVersionO1.0.3300.0,WCultureOneutral,WPublicKeyTokenOb03f5f7f11d50a3a))]
        public string DataMember
        {
            get
            {
                return mDataMember;
            }

            set
            {
                mDataMember = value;
                SetSource();
                if (mAutoDiscover)
                {
                    DoAutoDiscover();
                }
                DataBind();
            }
        }

        [CategoryAttribute("Data")]
        public bool AutoDiscover
        {
            get
            {
                return mAutoDiscover;
            }

            set
            {
                if (mAutoDiscover == false && value == true)
                {
                    mAutoDiscover = value;
                    DoAutoDiscover();
                }
                else
                {
                    mAutoDiscover = value;
                    if (mAutoDiscover == false)
                    {
                        mColumns.Clear();
                    }
                }
            }
        }

        private IBindingList mBindingList
        {
            get
            {
                return _mBindingList;
            }

            set
            {
                if (_mBindingList != null)
                {
                    _mBindingList.ListChanged -= new ListChangedEventHandler(mBindingList_ListChanged);
                }
                _mBindingList = value;
                if (_mBindingList != null)
                {
                    _mBindingList.ListChanged += new ListChangedEventHandler(mBindingList_ListChanged);
                }
            }
        }

        private DataColumnHeaderCollection mColumns
        {
            get
            {
                return _mColumns;
            }

            set
            {
                if (_mColumns != null)
                {
                    _mColumns.Invalidate -= new DataColumnHeaderCollection.InvalidateEventHandler(mItems_Invalidate);
                }
                _mColumns = value;
                if (_mColumns != null)
                {
                    _mColumns.Invalidate += new DataColumnHeaderCollection.InvalidateEventHandler(mItems_Invalidate);
                }
            }
        }

        public new DataColumnHeaderCollection Columns
        {
            get
            {
                return mColumns;
            }
        }

        public DataListView()
        {
            mAutoDiscover = true;
            mColumns = new DataColumnHeaderCollection();
            base.View = View.Details;
            base.FullRowSelect = true;
            base.MultiSelect = false;
        }

        private void DataBind()
        {

            base.BeginUpdate();
            try
            {
                base.Clear();
                if (mDataSource != null && mColumns.Count != 0)
                {
                    IList iList = InnerDataSource();
                    int j2 = mColumns.Count - 1;
                    for (int i1 = 0; i1 <= j2; i1++)
                    {
                        base.Columns.Add(mColumns[i1]);
                    }
                    int i2 = iList.Count - 1;
                    for (int j1 = 0; j1 <= i2; j1++)
                    {
                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = GetField(RuntimeHelpers.GetObjectValue(iList[j1]), mColumns[0].Field).ToString();
                        listViewItem.Tag = iList[j1];
                        int k = mColumns.Count - 1;
                        for (int i1 = 1; i1 <= k; i1++)
                        {
                            listViewItem.SubItems.Add(GetField(RuntimeHelpers.GetObjectValue(iList[j1]), mColumns[i1].Field).ToString());
                        }
                        base.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                base.EndUpdate();
            }
        }

        private void SetSource()
        {
            mBindingList = InnerDataSource() as IBindingList;
        }

        private IList InnerDataSource()
        {
            IList iList;

            if (mDataSource is DataSet)
            {
                if (mDataMember.Length > 0)
                {

                    iList = ((IListSource)((DataSet)mDataSource).Tables[mDataMember]).GetList();
                }
                else
                {
                    iList = ((IListSource)((DataSet)mDataSource).Tables[0]).GetList();
                }
            }
            else if (mDataSource is IListSource)
            {
                iList = ((IListSource)mDataSource).GetList();
            }
            else if (mDataSource is ICollection)
            {
                object[] objs = new object[((ICollection)mDataSource).Count];
                ((ICollection)mDataSource).CopyTo(objs, 0);
                iList = objs;
            }
            else
            {
                iList = (IList)mDataSource;
            }

            return iList;
        }

        private void mItems_Invalidate()
        {
            DataBind();
        }

        private void DoAutoDiscover()
        {
            IList iList = InnerDataSource();
            mColumns.Clear();
            if (iList != null)
            {
                if (iList is DataView)
                {
                    DoAutoDiscover((DataView)iList);
                }
                else
                {
                    DoAutoDiscover(iList);
                }
            }
        }

        private void DoAutoDiscover(DataView ds)
        {
            int j = ds.Table.Columns.Count - 1;
            for (int i = 0; i <= j; i++)
            {
                DataColumnHeader dataColumnHeader = new DataColumnHeader();
                dataColumnHeader.Text = ds.Table.Columns[i].Caption;
                dataColumnHeader.Field = ds.Table.Columns[i].ColumnName;
                mColumns.Add(dataColumnHeader);
            }
        }

        private void DoAutoDiscover(IList ds)
        {
            if (ds.Count > 0)
            {
                object local = RuntimeHelpers.GetObjectValue(ds[0]);
                if (local is ValueType && local.GetType().IsPrimitive)
                {
                    DataColumnHeader dataColumnHeader1 = new DataColumnHeader();
                    dataColumnHeader1.Text = "Value";
                    mColumns.Add(dataColumnHeader1);
                }
                else if (local is String)
                {
                    DataColumnHeader dataColumnHeader2 = new DataColumnHeader();
                    dataColumnHeader2.Text = "Text";
                    mColumns.Add(dataColumnHeader2);
                }
                else
                {
                    Type type = local.GetType();
                    PropertyInfo[] propertyInfos = type.GetProperties();
                    if (propertyInfos.Length > 0)
                    {
                        for (int i = 0; i < propertyInfos.Length; i++)
                        {
                            mColumns.Add(propertyInfos[i].Name);
                        }
                    }
                    FieldInfo[] fieldInfos = type.GetFields();
                    if (fieldInfos.Length > 0)
                    {
                        for (int i = 0; i < fieldInfos.Length; i++)
                        {
                            mColumns.Add(fieldInfos[i].Name);
                        }
                    }
                }
            }
        }

        private string GetField(object obj, string FieldName)
        {
            string str = String.Empty;
            if (obj is DataRowView)
            {
                str = ((DataRowView)obj)[FieldName].ToString();
            }
            else if (obj is ValueType && obj.GetType().IsPrimitive)
            {
                str = obj.ToString();
            }
            else if (obj is String)
            {
                str = (String)(obj);
            }
            else
            {
                try
                {
                    Type type = obj.GetType();
                    PropertyInfo propertyInfo = type.GetProperty(FieldName);
                    if (propertyInfo == null || !propertyInfo.CanRead)
                    {
                        if (!(type.GetField(FieldName) is FieldInfo fieldInfo))
                        {
                            str = "No such value " + FieldName;
                        }
                        else if (fieldInfo.GetValue(RuntimeHelpers.GetObjectValue(obj)) is object val)
                        {
                            str = val.ToString();
                        }
                    }
                    else
                    {
                        object val = propertyInfo.GetValue(RuntimeHelpers.GetObjectValue(obj), null);
                        if (val != null)
                            str = val.ToString();
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return str;
        }

        private void mBindingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            DataBind();
        }
    }
}
