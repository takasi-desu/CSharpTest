using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditer301
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool m_text_changed = false;
        string m_file_name = "";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            m_text_changed = true;
        }

        public bool DoOpen(string file_name)
        {
            try
            {
                var text = System.IO.File.ReadAllText(file_name);
                textBox1.Text = text;
                m_file_name = file_name;
                return true;
            }
            catch (Exception) 
            {
                MessageBox.Show("開けませんでした。");
            }
            return false;
        }
        public bool DoSave(string file_name)
        {
            if (file_name == "")
            {
                return DoSaveAs();
            }

            try
            {
                var text = textBox1.Text;
                System.IO.File.WriteAllText(file_name, text);
                m_file_name = file_name;
                return true;
            } 
            catch (Exception)
            {
                MessageBox.Show("保存できませんでした。");
            }
            return false;
        }
        public bool DoSaveAs()
        {
            var sfd = new SaveFileDialog();
            //ファイルの種類
            sfd.Filter =
                "テキスト文書 (*.txt;*.html;*.htm;*.log)|*.txt;*.html;*.htm;*.log|すべてのファイル (*.*)|*.*";
            //ファイルの種類の選択
            sfd.FilterIndex = 0;
            //タイトル
            sfd.Title = "名前を付けて保存";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            sfd.RestoreDirectory = true;
            // 既定の拡張子を設定。
            sfd.DefaultExt = "txt";
            //ダイアログを表示する
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return DoSave(sfd.FileName);
            }
            return false;
        }

        private void 開くOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            var ofd = new OpenFileDialog();
            //[ファイルの種類]に表示される選択肢を指定する
            ofd.Filter =
                "テキスト文書 (*.txt;*.html;*.htm;*.log)|*.txt;*.html;*.htm;*.log|すべてのファイル (*.*)|*.*";
            //[ファイルの種類]ではじめに
            //「Textファイル」が選択されているようにする
            ofd.FilterIndex = 0;
            //タイトルを設定する
            ofd.Title = "開く";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            // 既定の拡張子を設定。
            ofd.DefaultExt = "txt";
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DoOpen(ofd.FileName);
            }
        }

        private void 名前を付けて保存AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSaveAs();
        }

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_text_changed) {
                var result = 
                    MessageBox.Show(
                        "変更されています。保存しますか？",
                        "変更の保存", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        DoSave(m_file_name);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            this.Close();
        }

        private void 上書き保存StCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSave(m_file_name);
        }

        private void 切り取りToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectionLength > 0)
            {
                //選択されている文字列があるときは切り取る
                //（SelectionLengthで調べなくても問題はない）
                textBox1.Cut();
            }
        }

        private void コピーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectionLength > 0)
            {
                //選択されている文字列があるときはコピーする
                //（SelectionLengthで調べなくても問題はない）
                textBox1.Copy();
            }
        }

        private void 貼り付けToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();
            if (data != null && data.GetDataPresent(DataFormats.Text) == true)
            {
                //クリップボードにテキストデータがあるときは貼り付ける
                //（テキストデータの有無を調べなくても問題はない）
                textBox1.Paste();
            }
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectionLength > 0)
            {
                //選択されている文字列があるときは削除する
                //（SelectionLengthで調べなくても問題はない）
                textBox1.SelectedText = "";
            }
        }

        private void すべて選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //テキストをすべて選択
            textBox1.SelectAll();
        }

        private void 元に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine if last operation can be undone in text box.   
            if (textBox1.CanUndo == true)
            {
                // Undo the last operation.
                textBox1.Undo();
                // Clear the undo buffer to prevent last action from being redone.
                textBox1.ClearUndo();
            }
        }

        private void バージョン情報ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // VerForm の新しいインスタンスを生成する
            VerForm cVerForm = new VerForm();

            // VerForm をモーダルで表示する
            cVerForm.ShowDialog();

            // 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
            cVerForm.Dispose();
        }
    }
}
